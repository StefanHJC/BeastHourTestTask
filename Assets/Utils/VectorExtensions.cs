using UnityEngine;

public static class VectorExtensions
{
    public static Vector3 WithX(this Vector3 vector, float x)
    {
        vector.x = x;
        return vector;
    }

    public static Vector3 WithY(this Vector3 vector, float y)
    {
        vector.y = y;
        return vector;
    }

    public static Vector3 WithZ(this Vector3 vector, float z)
    {
        vector.z = z;
        return vector;
    }


    public static Vector2 Slerp2d(this Vector2 from, Vector2 to, float v)
    {
        if (from.sqrMagnitude == 0)
        {
            from = to * v;
        }
        else
        {
            from = from.normalized * (to.magnitude * v + from.magnitude * (1 - v));
        }

        var diff = Vector2.SignedAngle(from, to);
        return Quaternion.Euler(0f, 0f, diff * v) * from;
    }
}