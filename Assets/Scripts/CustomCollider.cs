using System.Collections.Generic;
using UnityEngine;

public abstract class CustomCollider : MonoBehaviour
{
    protected bool Enabled { get; set; }

    protected Bounds Bounds {
        get
        {
            return bounds;
        } 
    }

    private Bounds bounds;

    internal abstract float GetMinXYZ(int axe);

    internal abstract float GetMaxXYZ(int axe);

    internal abstract bool IsColliding(CustomCollider collider);

    internal abstract Vector3 ClosestPoint(Vector3 point);

    internal static float SquareDistance(Vector3 point1, Vector3 point2)
    {
        float res = Mathf.Pow(point2.x - point1.x, 2) + Mathf.Pow(point2.y - point1.y, 2) + Mathf.Pow(point2.z + point1.z, 2);
        return res;
    }

    protected void Awake()
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
    }
}