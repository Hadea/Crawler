using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public GameObject[] roomPrefabs;
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
        List<RoomConnector> openConnections = new List<RoomConnector>();
        openConnections.AddRange(startingRoom.roomConnectors);
        for (int i = 0; i < roomsNumber; i++)
        {
            if (openConnections.Count == 0)
            {
                print("Generation blocked. No open connections left.");
                return;
            }
            int random = Random.Range(0, openConnections.Count);
            RoomConnector randomConnectorStart = openConnections[random];
            openConnections.RemoveAt(random);

            random = Random.Range(0, roomPrefabs.Length);
            generatedRooms.Add(Instantiate(roomPrefabs[random]));
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
