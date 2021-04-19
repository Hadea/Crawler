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
#if UNITY_EDITOR
        UnityEditor.Handles.color = color;
        Bounds tmp = DungeonGenerator.RotateBounds90Steps(bounds, rotation.eulerAngles.y);
        UnityEditor.Handles.DrawWireCube(position + tmp.center + Vector3.up * tmp.size.y * 0.5f, tmp.size * 1.0001f);
#endif
    }
}
