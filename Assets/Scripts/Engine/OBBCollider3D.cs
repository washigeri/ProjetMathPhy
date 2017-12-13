using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OBBCollider3D : CustomCollider
{
    [HideInInspector]
    public bool doRotate = false;
    [HideInInspector]
    public float rotateSpeed = 40f;

    public Vector3 center = Vector3.zero;

    public Vector3 size = Vector3.one;

    public Vector3[] axis = new Vector3[] { Vector3.right, Vector3.up, Vector3.forward };

    internal Vector3[] Axis
    {
        get
        {
            Vector3[] corners = Corners;
            Vector3[] res = new Vector3[3];
            res[0] = corners[5] - corners[1];
            res[1] = corners[2] - corners[1];
            res[2] = corners[1] - corners[0];
            return res;
        }
    }

    private Vector3[] corners = new Vector3[] { };

    private Vector3[] Corners
    {
        get
        {
            Vector3[] res = new Vector3[corners.Length];
            Matrix4x4 YXZ = Matrix4x4.RotationXYZ(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
            for (int i = 0; i < corners.Length; i++)
            {
                res[i] = YXZ * corners[i];
            }
            return res;
        }
    }

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
            return new Vector3(size.x * transform.localScale.x, size.y * transform.localScale.y, size.z * transform.localScale.z);
        }
    }

    private Vector3 oldRotation;

    protected override void Awake()
    {
        base.Awake();
        corners = new Vector3[] { new Vector3(-size.x / 2f, -size.y / 2f, -size.z / 2f),
                                  new Vector3(-size.x / 2f, -size.y / 2f, size.z / 2f),
                                  new Vector3(-size.x / 2f, size.y / 2f, size.z / 2f),
                                  new Vector3(-size.x / 2f, size.y / 2f, -size.z / 2f),
                                  new Vector3(size.x / 2f, size.y / 2f, size.z / 2f),
                                  new Vector3(size.x / 2f, -size.y / 2f, size.z / 2f),
                                  new Vector3(size.x / 2f, size.y / 2f, -size.z / 2f),
                                  new Vector3(size.x / 2f, -size.y / 2f, -size.z / 2f)
                                };
        oldRotation = transform.eulerAngles;
    }

    internal override float GetMinXYZ(int axe)
    {
        Vector3[] corners = Corners;

        float min;
        if (axe == 0)
        {
            min = corners[0].x;
            foreach (Vector3 v in corners)
            {
                if (v.x < min)
                {
                    min = v.x;
                }
            }
            return min;
        }
        else if (axe == 1)
        {
            min = corners[0].y;
            foreach (Vector3 v in corners)
            {
                if (v.y < min)
                {
                    min = v.y;
                }
            }
            return min;
        }
        else if (axe == 2)
        {
            min = corners[0].z;
            foreach (Vector3 v in corners)
            {
                if (v.z < min)
                {
                    min = v.z;
                }
            }
            return min;
        }
        else
        {
            return Mathf.Infinity;
        }
    }

    private void Update()
    {
        if (doRotate)
        {
            transform.Rotate(Vector3.one * Time.deltaTime * rotateSpeed);
        }
        
    }

    internal override float GetMaxXYZ(int axe)
    {
        Vector3[] corners = Corners;
        float max;
        if (axe == 0)
        {
            max = corners[0].x;
            foreach (Vector3 v in corners)
            {
                if (v.x > max)
                {
                    max = v.x;
                }
            }
            return max;
        }
        else if (axe == 1)
        {
            max = corners[0].y;
            foreach (Vector3 v in corners)
            {
                if (v.y > max)
                {
                    max = v.y;
                }
            }
            return max;
        }
        else if (axe == 2)
        {
            max = corners[0].z;
            foreach (Vector3 v in corners)
            {
                if (v.z > max)
                {
                    max = v.z;
                }
            }
            return max;
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
            Vector3 closestPoint = ClosestPoint(colliderSphere.Center);
            isColliding = Vector3.Distance(closestPoint, colliderSphere.Center) <= colliderSphere.Radius;
            if (isColliding)
            {
                return new CollisionInfo(closestPoint, (colliderSphere.Center - closestPoint).normalized, colliderSphere.Radius - Vector3.Distance(colliderSphere.Center, closestPoint));
            }
        }
        else if (collider is OBBCollider3D)
        {
            var colliderOBB = (OBBCollider3D)collider;
            Vector3[] axis = Axis;
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
        else if(collider is BoxCollider3D)
        {
            var colliderBox = (BoxCollider3D)collider;
            Vector3[] axis = Axis;
            Vector3[] axisCollider = new Vector3[] { this.axis[0], this.axis[1], this.axis[2] };
            Debug.Log("axiscol = " + axisCollider);
            Vector3 halfSize = Size / 2f;
            Vector3 halfSizeBox = colliderBox.Size / 2f;

            float ra, rb;

            Matrix3x3 R = new Matrix3x3();
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    R[i, j] = Vector3.Dot(axis[i], axisCollider[j]);
                }
            }

            Vector3 t = colliderBox.Center - Center;

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
                rb = halfSizeBox[0] * AbsR[i, 0] + halfSizeBox[1] * AbsR[i, 1] + halfSizeBox[2] * AbsR[i, 2];
                if (Mathf.Abs(t[i]) > ra + rb)
                {
                    return null;
                }
            }

            for (int i = 0; i < 3; ++i)
            {
                ra = halfSize[0] * AbsR[i, 0] + halfSize[1] * AbsR[i, 1] + halfSize[2] * AbsR[i, 2];
                rb = halfSizeBox[i];
                if (Mathf.Abs(t.x * R[0, i] + t.y * R[1, i] + t.z * R[2, i]) > ra + rb)
                {
                    return null;
                }
            }

            ra = halfSize.y * AbsR[2, 0] + halfSize.z * AbsR[1, 0];
            rb = halfSizeBox.y * AbsR[0, 2] + halfSizeBox.z * AbsR[0, 1];
            if (Mathf.Abs(t.z * R[1, 0] - t.y * R[2, 0]) > ra + rb)
            {
                return null;
            }

            ra = halfSize.y * AbsR[2, 1] + halfSize.z * AbsR[1, 1];
            rb = halfSizeBox.x * AbsR[0, 2] + halfSizeBox.z * AbsR[0, 0];
            if (Mathf.Abs(t.z * R[1, 1] - t.y * R[2, 1]) > ra + rb)
            {
                return null;
            }

            ra = halfSize.y * AbsR[2, 2] + halfSize.z * AbsR[1, 2];
            rb = halfSizeBox.x * AbsR[0, 1] + halfSizeBox.y * AbsR[0, 0];
            if (Mathf.Abs(t.z * R[1, 2] - t.y * R[2, 2]) > ra + rb)
            {
                return null;
            }
            ra = halfSize.x * AbsR[2, 0] + halfSize.z * AbsR[0, 0];
            rb = halfSizeBox.y * AbsR[1, 2] + halfSizeBox.z * AbsR[1, 1];
            if (Mathf.Abs(t.x * R[2, 0] - t.z * R[0, 0]) > ra + rb)
            {
                return null;
            }
            ra = halfSize.x * AbsR[2, 1] + halfSize.z * AbsR[0, 1];
            rb = halfSizeBox.x * AbsR[1, 2] + halfSizeBox.z * AbsR[1, 0];
            if (Mathf.Abs(t.x * R[2, 1] - t.z * R[0, 1]) > ra + rb)
            {
                return null;
            }
            ra = halfSize.x * AbsR[2, 2] + halfSize.z * AbsR[0, 2];
            rb = halfSizeBox.x * AbsR[1, 1] + halfSizeBox.y * AbsR[1, 0];
            if (Mathf.Abs(t.x * R[2, 2] - t.z * R[0, 2]) > ra + rb)
            {
                return null;
            }
            ra = halfSize.x * AbsR[1, 0] + halfSize.y * AbsR[0, 0];
            rb = halfSizeBox.y * AbsR[2, 2] + halfSizeBox.z * AbsR[2, 1];
            if (Mathf.Abs(t.y * R[0, 0] - t.x * R[1, 0]) > ra + rb)
            {
                return null;
            }
            ra = halfSize.x * AbsR[1, 1] + halfSize.y * AbsR[0, 1];
            rb = halfSizeBox.x * AbsR[2, 2] + halfSizeBox.z * AbsR[2, 0];
            if (Mathf.Abs(t.y * R[0, 1] - t.x * R[1, 1]) > ra + rb)
            {
                return null;
            }
            ra = halfSize.x * AbsR[1, 2] + halfSize.y * AbsR[0, 2];
            rb = halfSizeBox.x * AbsR[2, 1] + halfSizeBox.y * AbsR[2, 0];
            if (Mathf.Abs(t.y * R[0, 2] - t.x * R[1, 2]) > ra + rb)
            {
                return null;
            }

            Vector3 closestPoint = ClosestPoint(colliderBox.Center);
            Vector3 closestPointCollider = colliderBox.ClosestPoint(Center);
            float penetrationDepth = Mathf.Abs(Vector3.Distance(colliderBox.Center, closestPointCollider) + Vector3.Distance(Center, closestPoint) - Vector3.Distance(Center, colliderBox.Center));

            return new CollisionInfo(closestPoint, (colliderBox.Center - closestPoint).normalized, penetrationDepth / 4f);
        }
        return null;
    }

    internal override Vector3 ClosestPoint(Vector3 point)
    {
        Vector3 d = point - Center;
        Vector3 closestPoint = Center;
        Vector3[] axis = Axis;
        for (int i = 0; i < 3; i++)
        {
            closestPoint += Mathf.Clamp(Vector3.Dot(d, axis[i]), -Size[i] / 2f, Size[i] / 2f) * axis[i];
        }
        return closestPoint;
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 1, 0);
        Vector3 center = Center;
        Vector3[] corners = Corners;
        if (corners.Length > 0)
        {
            Gizmos.DrawLine(corners[0] + center, corners[1] + center);
            Gizmos.DrawLine(corners[2] + center, corners[1] + center);
            Gizmos.DrawLine(corners[0] + center, corners[3] + center);
            Gizmos.DrawLine(corners[0] + center, corners[7] + center);
            Gizmos.DrawLine(corners[1] + center, corners[5] + center);
            Gizmos.DrawLine(corners[2] + center, corners[3] + center);
            Gizmos.DrawLine(corners[3] + center, corners[6] + center);
            Gizmos.DrawLine(corners[6] + center, corners[7] + center);
            Gizmos.DrawLine(corners[2] + center, corners[4] + center);
            Gizmos.DrawLine(corners[4] + center, corners[6] + center);
            Gizmos.DrawLine(corners[4] + center, corners[5] + center);
            Gizmos.DrawLine(corners[5] + center, corners[7] + center);

            Vector3[] axis = Axis;
            Gizmos.color = Color.red;
            Gizmos.DrawLine(Vector3.zero, Vector3.zero + axis[0] * size.x);
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(Vector3.zero, Vector3.zero + axis[1] * size.y);
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(Vector3.zero, Vector3.zero + axis[2] * size.z);
        }
    }
}
