using UnityEngine;

public class BoxCollider3D : CustomCollider
{
    public Vector3 center = Vector3.zero;
    public Vector3 localSize = Vector3.one;
    private Vector3 size;

    internal override Vector3 ClosestPoint(Vector3 point)
    {
        throw new System.NotImplementedException();
    }

    internal override float GetMaxXYZ(int axe)
    {
        if (axe == 0)
        {
            return Bounds.max.x;
        }
        else if (axe == 1)
        {
            return Bounds.max.y;
        }
        else if (axe == 2)
        {
            return Bounds.max.z;
        }
        else
        {
            return -Mathf.Infinity;
        }
    }

    internal override float GetMinXYZ(int axe)
    {
        if (axe == 0)
        {
            return Bounds.min.x;
        }
        else if (axe == 1)
        {
            return Bounds.min.y;
        }
        else if (axe == 2)
        {
            return Bounds.min.z;
        }
        else
        {
            return Mathf.Infinity;
        }
    }

    internal override bool IsColliding(CustomCollider collider)
    {
        return true;
    }

    private void Update()
    {
        center = transform.position;

        size = new Vector3(localSize.x * transform.lossyScale.x, localSize.y * transform.lossyScale.y, localSize.z * transform.lossyScale.z);
    }
}