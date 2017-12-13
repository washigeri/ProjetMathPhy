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
                return new CollisionInfo(closestPoint, (colliderSphere.Center - closestPoint).normalized, colliderSphere.Radius - Vector3.Distance(closestPoint, colliderSphere.Center));
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
        else if (collider is OBBCollider3D)
        {
            var colliderOBB = (OBBCollider3D)collider;
            Vector3[] axis = new Vector3[] { colliderOBB.axis[0], colliderOBB.axis[1], colliderOBB.axis[2] };
            Vector3[] axisCollider = colliderOBB.Axis;
            Vector3 halfSize = Size / 2f;
            Vector3 halfSizeOBB = colliderOBB.Size / 2f;
            float ra, rb;

            Matrix3x3 R = new Matrix3x3();
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    R[i, j] = Vector3.Dot(axis[i], axisCollider[j]);
                }
            }

            Vector3 t = colliderOBB.Center - Center;

            Vector3 tCopy = new Vector3
            {
                x = Vector3.Dot(t, axis[0]),
                y = Vector3.Dot(t, axis[1]),
                z = Vector3.Dot(t, axis[2])
            };

            float epsilon = 0.00001f;
            Matrix3x3 AbsR = new Matrix3x3();
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    AbsR[i, j] = Mathf.Abs(R[i, j]) + epsilon;
                }
            }

            for (int i = 0; i < 3; ++i)
            {
                ra = halfSize[i];
                rb = halfSizeOBB[0] * AbsR[i, 0] + halfSizeOBB[1] * AbsR[i, 1] + halfSizeOBB[2] * AbsR[i, 2];
                if (Mathf.Abs(t[i]) > ra + rb)
                {
                    return null;
                }
            }

            for (int i = 0; i < 3; ++i)
            {
                ra = halfSize[0] * AbsR[i, 0] + halfSize[1] * AbsR[i, 1] + halfSize[2] * AbsR[i, 2];
                rb = halfSizeOBB[i];
                if (Mathf.Abs(t.x * R[0, i] + t.y * R[1, i] + t.z * R[2, i]) > ra + rb)
                {
                    return null;
                }
            }

            ra = halfSize.y * AbsR[2, 0] + halfSize.z * AbsR[1, 0];
            rb = halfSizeOBB.y * AbsR[0, 2] + halfSizeOBB.z * AbsR[0, 1];
            if (Mathf.Abs(t.z * R[1, 0] - t.y * R[2, 0]) > ra + rb)
            {
                return null;
            }

            ra = halfSize.y * AbsR[2, 1] + halfSize.z * AbsR[1, 1];
            rb = halfSizeOBB.x * AbsR[0, 2] + halfSizeOBB.z * AbsR[0, 0];
            if (Mathf.Abs(t.z * R[1, 1] - t.y * R[2, 1]) > ra + rb)
            {
                return null;
            }

            ra = halfSize.y * AbsR[2, 2] + halfSize.z * AbsR[1, 2];
            rb = halfSizeOBB.x * AbsR[0, 1] + halfSizeOBB.y * AbsR[0, 0];
            if (Mathf.Abs(t.z * R[1, 2] - t.y * R[2, 2]) > ra + rb)
            {
                return null;
            }
            ra = halfSize.x * AbsR[2, 0] + halfSize.z * AbsR[0, 0];
            rb = halfSizeOBB.y * AbsR[1, 2] + halfSizeOBB.z * AbsR[1, 1];
            if (Mathf.Abs(t.x * R[2, 0] - t.z * R[0, 0]) > ra + rb)
            {
                return null;
            }
            ra = halfSize.x * AbsR[2, 1] + halfSize.z * AbsR[0, 1];
            rb = halfSizeOBB.x * AbsR[1, 2] + halfSizeOBB.z * AbsR[1, 0];
            if (Mathf.Abs(t.x * R[2, 1] - t.z * R[0, 1]) > ra + rb)
            {
                return null;
            }
            ra = halfSize.x * AbsR[2, 2] + halfSize.z * AbsR[0, 2];
            rb = halfSizeOBB.x * AbsR[1, 1] + halfSizeOBB.y * AbsR[1, 0];
            if (Mathf.Abs(t.x * R[2, 2] - t.z * R[0, 2]) > ra + rb)
            {
                return null;
            }
            ra = halfSize.x * AbsR[1, 0] + halfSize.y * AbsR[0, 0];
            rb = halfSizeOBB.y * AbsR[2, 2] + halfSizeOBB.z * AbsR[2, 1];
            if (Mathf.Abs(t.y * R[0, 0] - t.x * R[1, 0]) > ra + rb)
            {
                return null;
            }
            ra = halfSize.x * AbsR[1, 1] + halfSize.y * AbsR[0, 1];
            rb = halfSizeOBB.x * AbsR[2, 2] + halfSizeOBB.z * AbsR[2, 0];
            if (Mathf.Abs(t.y * R[0, 1] - t.x * R[1, 1]) > ra + rb)
            {
                return null;
            }
            ra = halfSize.x * AbsR[1, 2] + halfSize.y * AbsR[0, 2];
            rb = halfSizeOBB.x * AbsR[2, 1] + halfSizeOBB.y * AbsR[2, 0];
            if (Mathf.Abs(t.y * R[0, 2] - t.x * R[1, 2]) > ra + rb)
            {
                return null;
            }

            Vector3 closestPoint = ClosestPoint(colliderOBB.Center);
            Vector3 closestPointCollider = colliderOBB.ClosestPoint(Center);
            float penetrationDepth = Mathf.Abs(Vector3.Distance(colliderOBB.Center, closestPointCollider) + Vector3.Distance(Center, closestPoint) - Vector3.Distance(Center, colliderOBB.Center));

            return new CollisionInfo(closestPoint, (colliderOBB.Center - closestPoint).normalized, penetrationDepth / 4f);
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