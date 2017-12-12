using System.Collections.Generic;
using UnityEngine;

public class CollisionInfo
{
    public Vector3 collisionPoint;
    public Vector3 normalVector;

    public CollisionInfo(Vector3 pt, Vector3 normal)
    {
        collisionPoint = pt;
        normalVector = normal;
    }
}

[RequireComponent(typeof(RigidBodyScript))]

public abstract class CustomCollider : MonoBehaviour
{
    private Vector3 oldPosition;

    private Vector3 oldRotation;

    protected bool Enabled { get; set; }

    protected RigidBodyScript rb;

    internal abstract float GetMinXYZ(int axe);

    internal abstract float GetMaxXYZ(int axe);

    internal abstract CollisionInfo IsColliding(CustomCollider collider);

    internal abstract Vector3 ClosestPoint(Vector3 point);

    internal static float SquareDistance(Vector3 point1, Vector3 point2)
    {
        float res = Mathf.Pow(point2.x - point1.x, 2) + Mathf.Pow(point2.y - point1.y, 2) + Mathf.Pow(point2.z - point1.z, 2);
        return res;
    }

    protected virtual void Awake()
    {
        rb = GetComponent<RigidBodyScript>();
    }
}