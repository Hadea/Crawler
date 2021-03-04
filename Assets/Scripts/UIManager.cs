using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public PauseUI pauseUI;
    public InventoryUI inventoryUI;

    public Text scoreText = null;

    public Text ammoText = null;

    public GameObject iconPrefab = null;
    public Transform iconHolder = null;
    public Vector3 iconOffset = Vector3.zero;
    private List<GameObject> iconInstances;

    private float lastUIToggle;

    void Awake()
    {
        instance = this;
        iconInstances = new List<GameObject>();
    }

    void Update()
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
        GameManager gameManager = GameManager.instance;
        int health = gameManager.player1Stats.health.current / 2;
        bool half = gameManager.player1Stats.health.current % 2 > 0;
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
        scoreText.text = GameManager.instance.player1Stats.coins.ToString();
        if (PlayerControl.player != null)
        {
            ammoText.text = PlayerControl.player.ammo.ToString();
        }
    }

    public void TogglePauseUI()
    {
        if (pauseUI != null)
        {
            pauseUI.gameObject.SetActive(!pauseUI.gameObject.activeSelf);
            lastUIToggle = Time.time;
        }
    }

    public void ToggleInventoryUI()
    {
        if (inventoryUI != null)
        {
            inventoryUI.gameObject.SetActive(!inventoryUI.gameObject.activeSelf);
            lastUIToggle = Time.time;
        }
    }
}
