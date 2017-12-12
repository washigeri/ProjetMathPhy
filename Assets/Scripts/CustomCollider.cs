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

    protected Bounds bounds;

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

    private void UpdateBounds()
    {
        Vector3 translation = transform.position - oldPosition;
        Vector3 rotation = transform.eulerAngles - oldRotation;
        Matrix4x4 ZYX = Matrix4x4.RotationXYZ(rotation.x, rotation.y, rotation.z);
        Matrix4x4 T = Matrix4x4.Translation(translation);
        bounds.center = ZYX * T * bounds.center;
        //bounds.min = ZYXT * bounds.min;
        //bounds.min = ZYXT * bounds.max;
        //bounds.center = T * bounds.center;
    }

    protected virtual void Update()
    {
        UpdateBounds();
        Debug.Log("bounds.min = " + bounds.min);
        Debug.Log("bounds.max = " + bounds.max);
        oldPosition = transform.position;
        oldRotation = transform.eulerAngles;
    }

    protected virtual void Awake()
    {
        Vector3 currentRotation = transform.eulerAngles;
        transform.eulerAngles = Vector3.zero;
        Bounds copy = gameObject.GetComponent<Renderer>().bounds;
        bounds.center = copy.center;
        bounds.size = copy.size;
        bounds.extents = copy.extents;
        bounds.max = copy.max;
        bounds.min = copy.min;
        transform.eulerAngles = currentRotation;
        rb = GetComponent<RigidBodyScript>();
        oldPosition = transform.position;
        oldRotation = transform.eulerAngles;
    }
}