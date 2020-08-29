using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RoomConnector : MonoBehaviour
{
    public RoomConnector other;

    private NavMeshLink navMeshLink;

    public void AddNavMeshLink()
    {
        navMeshLink = gameObject.AddComponent<NavMeshLink>();
        navMeshLink.startPoint = Vector3.back;
        navMeshLink.endPoint = Vector3.forward;
        navMeshLink.width = 1.5f;
        navMeshLink.bidirectional = true;
        //navMeshLink.autoUpdate = true;
    }
}
