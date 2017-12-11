using System.Collections.Generic;
using UnityEngine;

public abstract class CustomCollider : MonoBehaviour
{
    protected bool Enabled { get; set; }

    internal abstract float GetMinXYZ(int axe);

    internal abstract float GetMaxXYZ(int axe);

    internal abstract bool IsColliding(CustomCollider collider);

    internal static float SquareDistance(Vector3 point1, Vector3 point2)
    {
        float res = Mathf.Pow(point2.x - point1.x, 2) + Mathf.Pow(point2.y - point1.y, 2) + Mathf.Pow(point2.z + point1.z, 2);
        return res;
    }
}