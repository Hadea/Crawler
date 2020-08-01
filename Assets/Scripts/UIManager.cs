using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager manager;

    [SerializeField]
    private GameObject pauseUI = null;

    public int score = 0;
    private Health playerHealth;

    [SerializeField]
    private Text scoreText = null;

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
        manager = this;
    }

    void Start()
    {
        playerHealth = PlayerControl.player.GetComponent<Health>();
        iconInstances = new List<GameObject>();
    }

    private void Update()
    {
        int health = playerHealth.GetCurrent();
        // if less icon are there as should be
        while (health > iconInstances.Count)
        {
            Vector3 position = iconHolder.position + iconOffset * iconInstances.Count;
            GameObject iconInstance = GameObject.Instantiate(iconPrefab, position, Quaternion.identity, iconHolder);
            iconInstances.Add(iconInstance);
        }
        // if more icons are there as should be
        while (health < iconInstances.Count)
        {
            GameObject iconInstance = iconInstances[iconInstances.Count - 1];
            iconInstances.RemoveAt(iconInstances.Count - 1);
            Destroy(iconInstance);
        }

        if (Input.GetButton("Cancel"))
        {
            if (Time.time > lastPauseUIToggle + 0.5f)
            {
                TogglePauseUI();
            }
        }
    }

    void LateUpdate()
    {
        scoreText.text = score.ToString();
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
