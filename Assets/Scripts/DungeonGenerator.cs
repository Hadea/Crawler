using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    [System.Serializable]
    public struct DungeonRoomPrefabChance
    {
        public GameObject prefab;
        [Range(0, 10)]
        public int weight;
    }

    public DungeonRoomPrefabChance[] rooms;
    public GameObject closeoffPrefab;

    public DungeonRoom startingRoom;

    public bool generate;
    public int roomsNumber = 30;

    private List<GameObject> generatedRooms;

    void Start()
    {
        if (generate)
        {
            Generate();
        }
    }

    [ContextMenu("Generate")]
    private void Generate()
    {
        Clear();
        List<RoomConnector> openConnections = new List<RoomConnector>();
        openConnections.AddRange(startingRoom.roomConnectors);
        int weightsTotal = 0;
        foreach (var dungeonRoom in rooms)
        {
            weightsTotal += dungeonRoom.weight;
        }
        for (int i = 0; i < roomsNumber; i++)
        {
            if (openConnections.Count == 0)
            {
                Debug.LogWarning("Generation blocked. No open connections left.", this);
                return;
            }
            int random = Random.Range(0, openConnections.Count);
            RoomConnector randomConnectorStart = openConnections[random];
            openConnections.RemoveAt(random);

            random = Random.Range(0, rooms.Length);
            generatedRooms.Add(Instantiate(GetRandomRoom(weightsTotal)));
            DungeonRoom roomInstance = generatedRooms[generatedRooms.Count - 1].GetComponent<DungeonRoom>();

            random = Random.Range(0, roomInstance.roomConnectors.Length);
            RoomConnector randomConnectorEnd = roomInstance.roomConnectors[random];

            PlaceNewRoom(randomConnectorStart, roomInstance, randomConnectorEnd);

            foreach (var connector in roomInstance.roomConnectors)
            {
                if (connector != randomConnectorEnd)
                {
                    openConnections.Add(connector);
                }
            }
        }
        for (int i = openConnections.Count - 1; i >= 0; i--)
        {
            RoomConnector connectorStart = openConnections[i];
            openConnections.RemoveAt(i);
            generatedRooms.Add(Instantiate(closeoffPrefab));
            DungeonRoom closeoffInstance = generatedRooms[generatedRooms.Count - 1].GetComponent<DungeonRoom>();
            RoomConnector connectorEnd = closeoffInstance.roomConnectors[0];
            PlaceNewRoom(connectorStart, closeoffInstance, connectorEnd);
        }
    }

    private GameObject GetRandomRoom(int total)
    {
        int random = Random.Range(0, total);
        total = 0;

        foreach (var chance in rooms)
        {
            if (random <= total)
            {
                return chance.prefab;
            }
            total += chance.weight;
        }
        throw new System.Exception("No loot found");
    }

    [ContextMenu("Clear")]
    private void Clear()
    {
        if (generatedRooms != null)
        {
            foreach (var room in generatedRooms)
            {
                Destroy(room);
            }
            generatedRooms.Clear();
        }
        else
        {
            generatedRooms = new List<GameObject>();
        }
    }

    private static void PlaceNewRoom(RoomConnector randomConnectorStart, DungeonRoom roomInstance, RoomConnector randomConnectorEnd)
    {
        randomConnectorStart.other = randomConnectorEnd;
        randomConnectorEnd.other = randomConnectorStart;

        Vector3 newRoomOffset = (roomInstance.transform.position - randomConnectorEnd.transform.position);
        roomInstance.transform.position = randomConnectorStart.transform.position + newRoomOffset;

        float angle = Vector3.SignedAngle(randomConnectorStart.transform.forward, randomConnectorEnd.transform.forward, Vector3.up);
        angle = -angle + 180f;
        roomInstance.transform.RotateAround(randomConnectorStart.transform.position, Vector3.up, angle);
    }
}
