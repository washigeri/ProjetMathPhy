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
            //Vector3 sizeAfterRotation = Vector3.one;
            //float tmp = sizeAfterRotation.z;
            //sizeAfterRotation.z = Mathf.Cos(transform.eulerAngles.x * Mathf.Deg2Rad) * sizeAfterRotation.z + Mathf.Sin(transform.eulerAngles.x * Mathf.Deg2Rad) * sizeAfterRotation.y;
            //sizeAfterRotation.y = Mathf.Sin(transform.eulerAngles.x * Mathf.Deg2Rad) * tmp + Mathf.Cos(transform.eulerAngles.x * Mathf.Deg2Rad) * sizeAfterRotation.y;

            //tmp = sizeAfterRotation.x;
            //sizeAfterRotation.x = Mathf.Cos(transform.eulerAngles.y * Mathf.Deg2Rad) * sizeAfterRotation.x + Mathf.Sin(transform.eulerAngles.y * Mathf.Deg2Rad) * sizeAfterRotation.z;
            //sizeAfterRotation.z = Mathf.Sin(transform.eulerAngles.y * Mathf.Deg2Rad) * tmp + Mathf.Cos(transform.eulerAngles.y * Mathf.Deg2Rad) * sizeAfterRotation.z;

            //tmp = sizeAfterRotation.x;
            //sizeAfterRotation.x = Mathf.Cos(transform.eulerAngles.y * Mathf.Deg2Rad) * sizeAfterRotation.x + Mathf.Sin(transform.eulerAngles.y * Mathf.Deg2Rad) * sizeAfterRotation.y;
            //sizeAfterRotation.y = Mathf.Sin(transform.eulerAngles.y * Mathf.Deg2Rad) * tmp + Mathf.Cos(transform.eulerAngles.y * Mathf.Deg2Rad) * sizeAfterRotation.y;

            //return sizeAfterRotation;
        }
    }

    internal override Vector3 ClosestPoint(Vector3 point)
    {
        Vector3 size = Size;
        Vector3 center = Center;
        Vector3 max = center + size / 2f;
        Vector3 min = center - size / 2f;
        Vector3 result = new Vector3();
        if (point.x > max.x)
        {
            result.x = max.x;
        }
        else if (point.x < min.x)
        {
            result.x = min.x;
        }
        else
        {
            result.x = point.x;
        }
        if (point.y > max.y)
        {
            result.y = max.y;
        }
        else if (point.y < min.y)
        {
            result.y = min.y;
        }
        else
        {
            result.y = point.y;
        }
        if (point.z > max.z)
        {
            result.z = max.z;
        }
        else if (point.z < min.z)
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
            Vector3 closestPoint = ClosestPoint(colliderSphere.Center);
            bool overlapX = (closestPoint.x >= colliderSphere.Center.x - colliderSphere.Radius) && (closestPoint.x <= colliderSphere.Center.x + colliderSphere.Radius);
            bool overlapY = (closestPoint.y >= colliderSphere.Center.y - colliderSphere.Radius) && (closestPoint.y <= colliderSphere.Center.y + colliderSphere.Radius);
            bool overlapZ = (closestPoint.z >= colliderSphere.Center.z - colliderSphere.Radius) && (closestPoint.z <= colliderSphere.Center.z + colliderSphere.Radius);
            isColliding = overlapX && overlapY && overlapZ;
            if (isColliding)
            {
                return new CollisionInfo(closestPoint, (closestPoint - Center).normalized, colliderSphere.Radius - Vector3.Distance(closestPoint, colliderSphere.Center));
            }
        }
        else if (collider is BoxCollider3D)
        {
            var colliderBox = (BoxCollider3D)collider;
            bool overlapX, overlapY, overlapZ;
            overlapX = ((Center.x - Size.x / 2f <= colliderBox.Center.x - colliderBox.Size.x / 2f && colliderBox.Center.x - colliderBox.Size.x / 2f <= Center.x + Size.x / 2f)) ||
                       ((colliderBox.Center.x - colliderBox.Size.x / 2f <= Center.x - Size.x / 2f && Center.x - Size.x / 2f <= colliderBox.Center.x + colliderBox.Size.x / 2f));
            overlapY = ((Center.y - Size.y / 2f <= colliderBox.Center.y - colliderBox.Size.y / 2f && colliderBox.Center.y - colliderBox.Size.y / 2f <= Center.y + Size.y / 2f)) ||
                       ((colliderBox.Center.y - colliderBox.Size.y / 2f <= Center.y - Size.y / 2f && Center.y - Size.y / 2f <= colliderBox.Center.y + colliderBox.Size.y / 2f));
            overlapZ = ((Center.z - Size.z / 2f <= colliderBox.Center.z - colliderBox.Size.z / 2f && colliderBox.Center.z - colliderBox.Size.z / 2f <= Center.z + Size.z / 2f)) ||
                       ((colliderBox.Center.z - colliderBox.Size.z / 2f <= Center.z - Size.z / 2f && Center.z - Size.z / 2f <= colliderBox.Center.z + colliderBox.Size.z / 2f));
            isColliding = overlapX && overlapY && overlapZ;
            if (isColliding)
            {
                Vector3 closestPoint = ClosestPoint(colliderBox.Center);
                float penetrationDepth = Mathf.Min(colliderBox.Size.x / 2f + Size.x / 2f - Mathf.Abs(Center.x - colliderBox.Center.x),
                                                   colliderBox.Size.y / 2f + Size.y / 2f - Mathf.Abs(Center.y - colliderBox.Center.y),
                                                   colliderBox.Size.z / 2f + Size.z / 2f - Mathf.Abs(Center.z - colliderBox.Center.z));
                return new CollisionInfo((Center - colliderBox.Center) / 2f, (colliderBox.Center - closestPoint).normalized, penetrationDepth);
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