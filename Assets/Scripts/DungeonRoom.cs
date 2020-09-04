using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[ExecuteInEditMode]
public class DungeonRoom : MonoBehaviour
{
    public RoomConnector[] roomConnectors;
    public Bounds bounds;

    void OnDrawGizmosSelected()
    {
        DrawBounds(bounds, transform.position, transform.rotation, Color.magenta);
    }

    void Reset()
    {
        bounds.center = Vector2.zero;
    }

    public static void DrawBounds(Bounds bounds, Vector3 position, Quaternion rotation, Color color)
    {
        Debug.DrawLine(position + rotation * bounds.GetTopLeft2D().ToVector3XZ(), position + rotation * bounds.GetTopRight2D().ToVector3XZ(), color);
        Debug.DrawLine(position + rotation * bounds.GetTopLeft2D().ToVector3XZ(), position + rotation * bounds.GetBottomLeft2D().ToVector3XZ(), color);
        Debug.DrawLine(position + rotation * bounds.GetTopRight2D().ToVector3XZ(), position + rotation * bounds.GetBottomRight2D().ToVector3XZ(), color);
        Debug.DrawLine(position + rotation * bounds.GetBottomLeft2D().ToVector3XZ(), position + rotation * bounds.GetBottomRight2D().ToVector3XZ(), color);
    }
}
