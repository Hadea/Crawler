using UnityEngine;

public class Node : IHeapItem<Node>
{
    public bool walkable;
    public Vector3 worldPosition;
    public Vector2Int gridPosition;
    public int movementPenalty;

    public int gCost;
    public int hCost;
    public Node parent;
    public int heapIndex { get; set; }

    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }

    public Node(bool isWalkable, Vector3 worldPos, Vector2Int gridPos, int penalty)
    {
        walkable = isWalkable;
        worldPosition = worldPos;
        gridPos = gridPosition;
        movementPenalty = penalty;
    }

    public int CompareTo(Node nodeToCompare)
    {
        int compare = fCost.CompareTo(nodeToCompare.fCost);
        if (compare == 0)
        {
            compare = hCost.CompareTo(nodeToCompare.hCost);
        }
        return -compare;
    }
}
