using UnityEngine;

public class SphereCollider3D : CustomCollider
{
    public Vector3 center = Vector3.zero;

    public float Radius
    {
        set
        {
            radius = value;
        }
        get
        {
            return radius;
        }
    }

    public float radius;

    internal override float GetMinXYZ(int axe)
    {
        if (axe == 0)
        {
            return center.x - Radius;
        }
        else if (axe == 1)
        {
            return center.y - Radius;
        }
        else if (axe == 2)
        {
            return center.z - Radius;
        }
        else
        {
            return Mathf.Infinity;
        }
    }

    internal override float GetMaxXYZ(int axe)
    {
        if (axe == 0)
        {
            return center.x + Radius;
        }
        else if (axe == 1)
        {
            return center.y + Radius;
        }
        else if (axe == 2)
        {
            return center.z + Radius;
        }
        else
        {
            return -Mathf.Infinity;
        }
    }

    internal override bool IsColliding(CustomCollider collider)
    {
        bool res = false;
        if (collider is SphereCollider3D)
        {
            var colliderSphere = (SphereCollider3D)collider;
            res = SquareDistance(this.center, colliderSphere.center) <= Mathf.Pow(Radius + colliderSphere.Radius, 2);
        }
        else if (collider is BoxCollider3D)
        {
            var colliderBox = (BoxCollider3D)collider;
            Vector3 closestPoint = ClosestPoint(colliderBox.center);
            if(Vector3.Distance(Vector3.zero, Vector3.zero) < 0f)
            {
                return false;
            }

            res = true;
            //TODO : check between sphere and box
        }
        return res;
    }

    private void Update()
    {
        center = transform.position;
        radius = Mathf.Max(Bounds.extents.x * transform.lossyScale.x, Bounds.extents.y * transform.lossyScale.y, Bounds.extents.z * transform.lossyScale.z);
        Debug.Log("radius = " + Radius);
    }

    internal override Vector3 ClosestPoint(Vector3 point)
    {
        Vector3 direction = (point - center).normalized;
        return center + Radius * direction;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 1, 0);
        Gizmos.DrawWireSphere(center, radius);
    }
}