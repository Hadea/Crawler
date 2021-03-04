using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [System.Serializable]
    public class PlayerStats
    {
        public PlayerControl player { get; set; }
        public Health health { get; set; }
        public Inventory inventory;
        public Equipment equipment;
        public int coins = 0;

        public PlayerStats()
        {
            inventory = new Inventory();
            equipment = new Equipment();
        }
    }

    public static GameManager instance;

    public PlayerStats player1Stats;

    void Awake()
    {
        instance = this;

        player1Stats = new PlayerStats();
    }

    void Start()
    {
        player1Stats.player = PlayerControl.player;
        player1Stats.health = player1Stats.player.GetComponent<Health>();

        player1Stats.inventory.onItemChanged?.Invoke();
    }

    public void SetupEquippedItem(Item.EquipmentTypes slotType, Item item)
    {
        player1Stats.player.SpawnOrDeleteEquippedItem(slotType, item);
    }
}
