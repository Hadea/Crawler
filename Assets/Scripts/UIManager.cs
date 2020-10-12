using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [SerializeField]
    private GameObject pauseUI = null;

    public int score = 0;
    private Health playerHealth;

    [SerializeField]
    private Text scoreText = null;

    [SerializeField]
    private Text ammoText = null;

    [SerializeField]
    private GameObject iconPrefab = null;
    [SerializeField]
    private Transform iconHolder = null;
    [SerializeField]
    private Vector3 iconOffset = Vector3.zero;
    private List<GameObject> iconInstances;

    private float lastPauseUIToggle;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        playerHealth = PlayerControl.player.GetComponent<Health>();
        iconInstances = new List<GameObject>();
    }

    private void Update()
    {
        UpdateHearthIconCount();

        if (Input.GetButton("Cancel"))
        {
            if (Time.time > lastPauseUIToggle + 0.5f)
            {
                TogglePauseUI();
            }
        }
    }

    private void UpdateHearthIconCount()
    {
        int health = playerHealth.currentHealth / 2;
        bool half = playerHealth.currentHealth % 2 > 0;
        // if less icon are there as should be
        if (!half && iconInstances.Count > 0)
        {
            GameObject iconInstance = iconInstances[iconInstances.Count - 1];
            iconInstance.GetComponent<Image>().fillAmount = 1f;
        }
        while (health > iconInstances.Count)
        {
            Vector3 position = iconHolder.position + iconOffset * iconInstances.Count;
            GameObject iconInstance = Instantiate(iconPrefab, position, Quaternion.identity, iconHolder);
            iconInstances.Add(iconInstance);
        }
        if (half)
        {
            Vector3 position = iconHolder.position + iconOffset * iconInstances.Count;
            GameObject iconInstance = Instantiate(iconPrefab, position, Quaternion.identity, iconHolder);
            iconInstance.GetComponent<Image>().fillAmount = 0.5f;
            iconInstances.Add(iconInstance);
        }
        // if more icons are there as should be
        while (health + half.ToInt() < iconInstances.Count)
        {
            GameObject iconInstance = iconInstances[iconInstances.Count - 1];
            iconInstances.RemoveAt(iconInstances.Count - 1);
            Destroy(iconInstance);
        }
        if (half)
        {
            GameObject iconInstance = iconInstances[iconInstances.Count - 1];
            iconInstance.GetComponent<Image>().fillAmount = 0.5f;
        }
    }

    void LateUpdate()
    {
        scoreText.text = score.ToString();
        if (PlayerControl.player != null)
        {
            ammoText.text = PlayerControl.player.ammo.ToString();
        }
    }

    public void TogglePauseUI()
    {
        if (pauseUI != null)
        {
            pauseUI.SetActive(!pauseUI.activeSelf);
            lastPauseUIToggle = Time.time;
        }
    }
}
