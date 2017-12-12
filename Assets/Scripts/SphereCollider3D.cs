using UnityEngine;

public class SphereCollider3D : CustomCollider
{
    public Vector3 center = Vector3.zero;

    public float radius = 0.5f;

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

    internal override CollisionInfo IsColliding(CustomCollider collider)
    {
        bool isColliding = false;
        if (collider is SphereCollider3D)
        {
            var colliderSphere = (SphereCollider3D)collider;
            isColliding = SquareDistance(this.center, colliderSphere.center) <= Mathf.Pow(radius + colliderSphere.radius, 2);
            if (isColliding)
            {
                return new CollisionInfo((this.center * this.radius - colliderSphere.center * colliderSphere.radius) / (this.radius + colliderSphere.radius), (this.center - colliderSphere.center).normalized);
            }
        }
        else if (collider is BoxCollider3D)
        {
            var colliderBox = (BoxCollider3D)collider;
            Vector3 closestPoint = ClosestPoint(colliderBox.center);
            bool overlapX = (closestPoint.x > colliderBox.center.x - colliderBox.size.x / 2f) && (closestPoint.x < colliderBox.center.x + colliderBox.size.x / 2f);
            bool overlapY = (closestPoint.y > colliderBox.center.y - colliderBox.size.y / 2f) && (closestPoint.y < colliderBox.center.y + colliderBox.size.y / 2f);
            bool overlapZ = (closestPoint.z > colliderBox.center.z - colliderBox.size.z / 2f) && (closestPoint.z < colliderBox.center.z + colliderBox.size.z / 2f);

            isColliding = overlapX && overlapY && overlapZ;
            if (isColliding)
            {
                return new CollisionInfo(closestPoint, (colliderBox.center - center).normalized);
            }
        }
        return null;
    }

    protected override void Update()
    {
        base.Update();
        center = transform.position;
        radius = Mathf.Max(bounds.extents.x * transform.lossyScale.x, bounds.extents.y * transform.lossyScale.y, bounds.extents.z * transform.lossyScale.z);
    }

    internal override Vector3 ClosestPoint(Vector3 point)
    {
        Vector3 direction = (point - center).normalized;
        return center + radius * direction;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 1, 0);
        Gizmos.DrawWireSphere(center, radius);
    }
}