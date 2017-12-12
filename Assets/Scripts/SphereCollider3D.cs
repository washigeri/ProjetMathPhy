using UnityEngine;

public class SphereCollider3D : CustomCollider
{
    public Vector3 center = Vector3.zero;

    public float localRadius = 0.5f;

    public float radius;

    internal override float GetMinXYZ(int axe)
    {
        if (axe == 0)
        {
            return center.x - radius;
        }
        else if (axe == 1)
        {
            return center.y - radius;
        }
        else if (axe == 2)
        {
            return center.z - radius;
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
            return center.x + radius;
        }
        else if (axe == 1)
        {
            return center.y + radius;
        }
        else if (axe == 2)
        {
            return center.z + radius;
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
            res = SquareDistance(this.center, colliderSphere.center) <= Mathf.Pow(localRadius + colliderSphere.localRadius, 2);
        }
        else if (collider is BoxCollider3D)
        {
            res = true;
            //TODO : check between sphere and box
        }
        return res;
    }

    private void Update()
    {
        center = transform.position;
        radius = localRadius * Mathf.Max(transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 1, 0);
        Gizmos.DrawWireSphere(center, radius);
    }
}