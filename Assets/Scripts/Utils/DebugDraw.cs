using UnityEngine;

public static class DebugDraw
{
    public static void Circle(Vector2 startPoint, float radius = 1, Color color = default, float duration = .2f)
    {
        if(color == default) color = Color.white;

        Debug.DrawRay(startPoint, Vector3.right * radius, color, duration);
        Debug.DrawRay(startPoint, Vector3.left * radius, color, duration);
        Debug.DrawRay(startPoint, Vector3.up * radius, color, duration);
        Debug.DrawRay(startPoint, Vector3.down * radius, color, duration);

        Debug.DrawRay(startPoint, (Vector3.up + Vector3.right).normalized * radius, color, duration);
        Debug.DrawRay(startPoint, (Vector3.up + Vector3.left).normalized * radius, color, duration);
        Debug.DrawRay(startPoint, (Vector3.down + Vector3.right).normalized * radius, color, duration);
        Debug.DrawRay(startPoint, (Vector3.down + Vector3.left).normalized * radius, color, duration);
    }

    public static void Square(Vector2 startPoint, float extents, Color color = default, float duration = .2f)
    {
        if (color == default) color = Color.white;

        Vector2 upLeft = startPoint + (Vector2.up + Vector2.left) * extents;
        Vector2 upRight = startPoint + (Vector2.up + Vector2.right) * extents;
        Vector2 downLeft = startPoint + (Vector2.down + Vector2.left) * extents;
        Vector2 downRight = startPoint + (Vector2.down + Vector2.right) * extents;

        Debug.DrawLine(upLeft, upRight, color, duration);
        Debug.DrawLine(upLeft, downLeft, color, duration);
        Debug.DrawLine(upRight, downRight, color, duration);
        Debug.DrawLine(downLeft, downRight, color, duration);
    }
}
