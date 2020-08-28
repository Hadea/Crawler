using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class DungeonRoom : MonoBehaviour
{
    public RoomConnector[] roomConnectors;
    public Bounds bounds;

    void OnDrawGizmosSelected()
    {
        DrawRoomSize();
    }

    void Reset()
    {
        bounds.center = Vector2.zero;
    }

    private void DrawRoomSize()
    {
        Color color = Color.magenta;
        Debug.DrawLine(transform.position + transform.rotation * bounds.GetTopLeft2D().ToVector3XZ(), transform.position + transform.rotation * bounds.GetTopRight2D().ToVector3XZ(), color);
        Debug.DrawLine(transform.position + transform.rotation * bounds.GetTopLeft2D().ToVector3XZ(), transform.position + transform.rotation * bounds.GetBottomLeft2D().ToVector3XZ(), color);
        Debug.DrawLine(transform.position + transform.rotation * bounds.GetTopRight2D().ToVector3XZ(), transform.position + transform.rotation * bounds.GetBottomRight2D().ToVector3XZ(), color);
        Debug.DrawLine(transform.position + transform.rotation * bounds.GetBottomLeft2D().ToVector3XZ(), transform.position + transform.rotation * bounds.GetBottomRight2D().ToVector3XZ(), color);
    }
}
