using UnityEngine;

public class BoxCollider3D : CustomCollider
{
    public Vector3 center = Vector3.zero;

    public Vector3 Center
    {
        get
        {
            return transform.position + center;
        }
    }

    public Vector3 Size
    {
        get
        {
            return GetComponent<Renderer>().bounds.size;
        }
    }

    internal override Vector3 ClosestPoint(Vector3 point)
    {
        Vector3 size = Size;
        Vector3 center = Center;
        Vector3 max = center + size / 2f;
        Vector3 min = center - size / 2f;
        Vector3 result = new Vector3();
        if(point.x > max.x)
        {
            result.x = max.x;
        }
        else if(point.x < min.x)
        {
            result.x = min.x;
        }
        else
        {
            result.x = point.x;
        }
        if(point.y > max.y)
        {
            result.y = max.y;
        }
        else if(point.y < min.y)
        {
            result.y = min.y;
        }
        else
        {
            result.y = point.y;
        }
        if(point.z > max.z)
        {
            result.z = max.z;
        }
        else if(point.z < min.z)
        {
            result.z = min.z;
        }
        else
        {
            result.z = point.z;
        }
        return result;
    }

    internal override float GetMaxXYZ(int axe)
    {
        if (axe == 0)
        {
            return Center.x + Size.x / 2f;
        }
        else if (axe == 1)
        {
            return Center.y + Size.y / 2f;
        }
        else if (axe == 2)
        {
            return Center.z + Size.z / 2f;
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
            return Center.x - Size.x / 2f;
        }
        else if (axe == 1)
        {
            return Center.y - Size.y / 2f;
        }
        else if (axe == 2)
        {
            return Center.z - Size.z / 2f;
        }
        else
        {
            return Mathf.Infinity;
        }
    }

    internal override CollisionInfo IsColliding(CustomCollider collider)
    {
        bool isColliding = false;
        if (collider is SphereCollider3D)
        {
            var colliderSphere = (SphereCollider3D)collider;
            CollisionInfo collision = colliderSphere.IsColliding(this);
            if (collision != null)
            {
                collision.normalVector *= -1f;
            }
            return collision;
        }
        else if (collider is BoxCollider3D)
        {
            var colliderBox = (BoxCollider3D)collider;
            bool overlapX, overlapY, overlapZ;
            overlapX = (Center.x - Size.x / 2f <= colliderBox.Center.x - colliderBox.Size.x / 2f && colliderBox.Center.x - colliderBox.Size.x / 2f <= Center.x + Size.x / 2f);
            overlapY = (Center.y - Size.y / 2f <= colliderBox.Center.y - colliderBox.Size.y / 2f && colliderBox.Center.y - colliderBox.Size.y / 2f <= Center.y + Size.y / 2f);
            overlapZ = (Center.z - Size.z / 2f <= colliderBox.Center.z - colliderBox.Size.z / 2f && colliderBox.Center.z - colliderBox.Size.z / 2f <= Center.z + Size.z / 2f);
            isColliding = overlapX && overlapY && overlapZ;
            if (isColliding)
            {
                float penetrationDepth = Mathf.Max(colliderBox.Size.x / 2f + Size.x / 2f, colliderBox.Size.y / 2f + Size.y / 2f, colliderBox.Size.z / 2f + Size.z / 2f) - Vector3.Distance(Center, colliderBox.Center);
                return new CollisionInfo((Center - colliderBox.Center) / 2f, (Center - colliderBox.Center).normalized, penetrationDepth);
            }
        }
        return null;
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 1, 0);
        Vector3 size = Size;
        Vector3 center = Center;
        Vector3 p1, p2, p3, p4, p5, p6, p7, p8;
        p1 = Center + new Vector3(-size.x / 2f, -size.y / 2f, -size.z / 2f);
        p2 = Center + new Vector3(-size.x / 2f, -size.y / 2f, size.z / 2f);
        p3 = Center + new Vector3(-size.x / 2f, size.y / 2f, size.z / 2f);
        p4 = Center + new Vector3(-size.x / 2f, size.y / 2f, -size.z / 2f);
        p5 = Center + new Vector3(size.x / 2f, size.y / 2f, size.z / 2f);
        p6 = Center + new Vector3(size.x / 2f, -size.y / 2f, size.z / 2f);
        p7 = Center + new Vector3(size.x / 2f, size.y / 2f, -size.z / 2f);
        p8 = Center + new Vector3(size.x / 2f, -size.y / 2f, -size.z / 2f);
        Gizmos.DrawLine(p1, p2);
        Gizmos.DrawLine(p3, p2);
        Gizmos.DrawLine(p1, p4);
        Gizmos.DrawLine(p1, p8);
        Gizmos.DrawLine(p2, p6);
        Gizmos.DrawLine(p3, p4);
        Gizmos.DrawLine(p4, p7);
        Gizmos.DrawLine(p7, p8);
        Gizmos.DrawLine(p3, p5);
        Gizmos.DrawLine(p5, p7);
        Gizmos.DrawLine(p5, p6);
        Gizmos.DrawLine(p6, p8);
    }
}