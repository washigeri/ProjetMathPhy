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

    internal static Vector3 ComputeCollisionPoint(CustomCollider collider1, CustomCollider collider2)
    {
        if (collider1 is SphereCollider3D)
        {
            var collider1sphere = (SphereCollider3D)collider1;
            if (collider2 is SphereCollider3D)
            {
                var collider2sphere = (SphereCollider3D)collider2;
                return (collider1sphere.center - collider2sphere.center) / 2f;
            }
            else if (collider2 is BoxCollider3D)
            {
                var collider2Box = (BoxCollider3D)collider2;
                return (collider1sphere.center * collider1sphere.radius + collider2Box.center * collider2Box.radius) / (collider1sphere.radius + collider2Box.radius);
            }
        }
    }
}