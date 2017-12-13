using UnityEngine;

public class SphereCollider3D : CustomCollider
{
    public Vector3 center = Vector3.zero;

    public float radius = 0.5f;

    public Vector3 Center
    {
        get
        {
            return center + transform.position;
        }
    }

    public float Radius
    {
        get
        {
            return radius * Mathf.Max(Mathf.Abs(transform.localScale.x), Mathf.Abs(transform.localScale.y), Mathf.Abs(transform.localScale.z));
        }
    }

    internal override float GetMinXYZ(int axe)
    {
        if (axe == 0)
        {
            return Center.x - Radius;
        }
        else if (axe == 1)
        {
            return Center.y - Radius;
        }
        else if (axe == 2)
        {
            return Center.z - Radius;
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
            return Center.x + Radius;
        }
        else if (axe == 1)
        {
            return Center.y + Radius;
        }
        else if (axe == 2)
        {
            return Center.z + Radius;
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
            isColliding = SquareDistance(this.Center, colliderSphere.Center) <= Mathf.Pow(Radius + colliderSphere.Radius, 2);
            if (isColliding)
            {
                return new CollisionInfo((Center * Radius - colliderSphere.Center * colliderSphere.Radius) / (Radius + colliderSphere.Radius), (colliderSphere.Center - Center).normalized, Radius + colliderSphere.Radius - Vector3.Distance(Center, colliderSphere.Center));
            }
        }
        else if (collider is BoxCollider3D)
        {
            var colliderBox = (BoxCollider3D)collider;
            Vector3 closestPoint = colliderBox.ClosestPoint(Center);
            bool overlapX = (closestPoint.x >= Center.x - Radius) && (closestPoint.x <= Center.x + Radius);
            bool overlapY = (closestPoint.y >= Center.y - Radius) && (closestPoint.y <= Center.y + Radius);
            bool overlapZ = (closestPoint.z >= Center.z - Radius) && (closestPoint.z <= Center.z + Radius);
            isColliding = overlapX && overlapY && overlapZ;
            if (isColliding)
            {
                Debug.Log("colliding !");
                return new CollisionInfo(closestPoint, (colliderBox.Center - closestPoint).normalized, Radius - Vector3.Distance(closestPoint, Center));
            }
        }
        return null;
    }

    internal override Vector3 ClosestPoint(Vector3 point)
    {
        return Center + Radius * (point - Center).normalized;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 1, 0);
        Gizmos.DrawWireSphere(Center, Radius);
    }
}