using UnityEngine;

public struct Line
{
    private const float verticalLineGradient = 1e5f;

    private float gradient;
    private float yIntercept;
    private Vector2 pointOnLine1;
    private Vector2 pointOnLine2;
    private float gradientPerpendicular;
    private bool approachSide;

    public Line(Vector2 pointOnLine, Vector2 pointPerpendicularToLine)
    {
        float dx = pointOnLine.x - pointPerpendicularToLine.x;
        float dy = pointOnLine.y - pointPerpendicularToLine.y;

        if (dx == 0f)
        {
            gradientPerpendicular = verticalLineGradient;
        }
        else
        {
            gradientPerpendicular = dy / dx;
        }

        if (gradientPerpendicular == 0f)
        {
            gradient = verticalLineGradient;
        }
        else
        {
            gradient = -1f / gradientPerpendicular;
        }

        yIntercept = pointOnLine.y - gradient * pointOnLine.x;
        pointOnLine1 = pointOnLine;
        pointOnLine2 = pointOnLine + new Vector2(1f, gradient);

        approachSide = false;
        approachSide = GetSide(pointPerpendicularToLine);
    }

    private bool GetSide(Vector2 p)
    {
        return (p.x - pointOnLine1.x) * (pointOnLine2.y - pointOnLine1.y) > (p.y - pointOnLine1.y) * (pointOnLine2.x - pointOnLine1.x);
    }

    public bool HasCrossedLine(Vector2 p)
    {
        return GetSide(p) != approachSide;
    }

    public float DistanceFromPoint(Vector2 p)
    {
        float yInterceptPerpendicular = p.y - gradientPerpendicular * p.x;
        float intersectX = (yInterceptPerpendicular - yIntercept) / (gradient - gradientPerpendicular);
        float intersectY = gradient * intersectX + yIntercept;
        return Vector2.Distance(p, new Vector2(intersectX, intersectY));
    }

    public void DrawWithGizmos(float length)
    {
        Vector3 lineDir = new Vector3(1f, 0f, gradient).normalized;
        Vector3 lineCenter = pointOnLine1.ToVector3XZ(1f);
        Gizmos.DrawLine(lineCenter - lineDir * length / 2f, lineCenter + lineDir * length / 2f);
    }

}
