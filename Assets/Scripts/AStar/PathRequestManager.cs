using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public struct PathResult
{
    public Vector3[] path;
    public bool pathFound;
    public Action<Vector3[], bool> callback;

    public PathResult(Vector3[] pathArray, bool success, Action<Vector3[], bool> callbackFunk)
    {
        path = pathArray;
        pathFound = success;
        callback = callbackFunk;
    }
}

public struct PathRequest
{
    public Vector3 pathStart;
    public Vector3 pathEnd;
    public Action<Vector3[], bool> callback;

    public PathRequest(Vector3 start, Vector3 end, Action<Vector3[], bool> callbackFunk)
    {
        pathStart = start;
        pathEnd = end;
        callback = callbackFunk;
    }
}

public class PathRequestManager : MonoBehaviour
{
    private static PathRequestManager instance;
    private Queue<PathResult> results = new Queue<PathResult>();
    private Pathfinding pathfinding;

    void Awake()
    {
        instance = this;
        pathfinding = GetComponent<Pathfinding>();
    }

    void Update()
    {
        if (results.Count > 0)
        {
            int itemsInQueue = results.Count;
            lock (results)
            {
                for (int i = 0; i < itemsInQueue; i++)
                {
                    PathResult result = results.Dequeue();
                    result.callback(result.path, result.pathFound);
                }
            }
        }
    }

    public static void RequestPath(PathRequest request)
    {
        ThreadStart threadStart = delegate
        {
            instance.pathfinding.FindPath(request, instance.FinishedProcessingPath);
        };
        threadStart.Invoke();
    }

    public void FinishedProcessingPath(PathResult result)
    {
        lock (results)
        {
            results.Enqueue(result);
        }
    }
}
