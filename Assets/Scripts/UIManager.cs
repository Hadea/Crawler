using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject pauseUI;
    public GameObject inventoryUI;

    public int score = 0;
    private Health playerHealth;

    public Text scoreText = null;

    public Text ammoText = null;

    public GameObject iconPrefab = null;
    public Transform iconHolder = null;
    public Vector3 iconOffset = Vector3.zero;
    private List<GameObject> iconInstances;

    private float lastUIToggle;

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

        if (Time.time > lastUIToggle + 0.1f)
        {
            if (Input.GetButtonDown("Cancel"))
            {
                TogglePauseUI();
            }
            else if (Input.GetButtonDown("Inventory"))
            {
                ToggleInventoryUI();
            }
        }
    }

    private void UpdateHearthIconCount()
    {
        int health = playerHealth.current / 2;
        bool half = playerHealth.current % 2 > 0;
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
            lastUIToggle = Time.time;
        }
    }

    public void ToggleInventoryUI()
    {
        if (inventoryUI != null)
        {
            inventoryUI.SetActive(!inventoryUI.activeSelf);
            lastUIToggle = Time.time;
        }
    }
}
