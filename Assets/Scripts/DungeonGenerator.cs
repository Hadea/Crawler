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
    public int maxRetries = 5;

    private List<DungeonRoom> generatedRooms;

    void Start()
    {
        if (generate)
        {
            for (int retries = 0; retries < maxRetries && !Generate(); retries++)
            {
            }
        }
    }

    [ContextMenu("Generate")]
    private bool Generate()
    {
        Clear();
        generatedRooms.Add(startingRoom);
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
                return false;
            }

            RoomConnector randomConnectorStart;
            DungeonRoom newRoom;
            RoomConnector randomConnectorEnd;
            int openIndex;
            bool fits;
            int retries = maxRetries;
            do
            {
                fits = true;
                openIndex = Random.Range(0, openConnections.Count);
                randomConnectorStart = openConnections[openIndex];

                int randomIndex = Random.Range(0, rooms.Length);

                GameObject roomInstance = Instantiate(GetRandomRoom(weightsTotal));
                newRoom = roomInstance.GetComponent<DungeonRoom>();

                randomIndex = Random.Range(0, newRoom.roomConnectors.Length);
                randomConnectorEnd = newRoom.roomConnectors[randomIndex];

                PlaceNewRoom(randomConnectorStart, newRoom, randomConnectorEnd);

                Bounds a = newRoom.bounds;
                if (Mathf.Abs(newRoom.transform.rotation.eulerAngles.y) / 90f % 2f > 0.9f)
                {
                    a.size = a.size.ToVector3ZYX();
                    a.center = a.center.ToVector3ZYX();
                }
                a.size *= 0.999f;
                a.center += newRoom.transform.position;
                foreach (var otherRoom in generatedRooms)
                {
                    Bounds b = otherRoom.bounds;
                    if (Mathf.Abs(otherRoom.transform.rotation.eulerAngles.y) / 90f % 2f > 0.9f)
                    {
                        b.size = b.size.ToVector3ZYX();
                        b.center = b.center.ToVector3ZYX();
                    }
                    b.size *= 0.999f;
                    b.center += otherRoom.transform.position;

                    if (b.Intersects(a))
                    {
                        fits = false;
                        Destroy(roomInstance);
                        retries--;
                        break;
                    }
                }
            } while (!fits && retries > 0);

            if (fits)
            {
                generatedRooms.Add(newRoom);
                openConnections.RemoveAt(openIndex);

                foreach (var connector in newRoom.roomConnectors)
                {
                    if (connector != randomConnectorEnd)
                    {
                        openConnections.Add(connector);
                    }
                }
            }
            else
            {
                Debug.LogWarning("Generation blocked. No space for rooms found.", this);
                return false;
            }
        }

        for (int i = openConnections.Count - 1; i >= 0; i--)
        {
            RoomConnector connectorStart = openConnections[i];
            openConnections.RemoveAt(i);
            GameObject instance = Instantiate(closeoffPrefab);
            DungeonRoom closeoffInstance = instance.GetComponent<DungeonRoom>();
            generatedRooms.Add(closeoffInstance);
            RoomConnector connectorEnd = closeoffInstance.roomConnectors[0];
            PlaceNewRoom(connectorStart, closeoffInstance, connectorEnd);
        }
        return true;
    }

    private GameObject GetRandomRoom(int total)
    {
        int random = Random.Range(0, total);
        total = 0;

        foreach (var chance in rooms)
        {
            total += chance.weight;
            if (random <= total)
            {
                return chance.prefab;
            }
        }
        throw new System.Exception("No room found");
    }

    [ContextMenu("Clear")]
    private void Clear()
    {
        if (generatedRooms != null)
        {
            foreach (var room in generatedRooms)
            {
                if (room != startingRoom)
                {
                    Destroy(room.gameObject);
                }
            }
            generatedRooms.Clear();
        }
        else
        {
            generatedRooms = new List<DungeonRoom>();
        }
    }

    private static void PlaceNewRoom(RoomConnector randomConnectorStart, DungeonRoom roomInstance, RoomConnector randomConnectorEnd)
    {
        Vector3 newRoomOffset = (roomInstance.transform.position - randomConnectorEnd.transform.position);
        roomInstance.transform.position = randomConnectorStart.transform.position + newRoomOffset;

        float angle = Vector3.SignedAngle(randomConnectorStart.transform.forward, randomConnectorEnd.transform.forward, Vector3.up);
        angle = -angle + 180f;
        roomInstance.transform.RotateAround(randomConnectorStart.transform.position, Vector3.up, angle);

        randomConnectorStart.other = randomConnectorEnd;
        randomConnectorEnd.other = randomConnectorStart;

        if (roomInstance.GetComponent<UnityEngine.AI.NavMeshSurface>() != null)
        {
            randomConnectorEnd.AddNavMeshLink();
        }
    }
}
