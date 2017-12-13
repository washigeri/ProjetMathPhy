using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OBBCollider : CustomCollider {

    public Vector3 center = Vector3.zero;

    public Vector3 size = Vector3.one;

    public Vector3[] axis = new Vector3[] { Vector3.right, Vector3.up, Vector3.forward };

    private Vector3[] Axis
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

    private Vector3[] corners;

    private Vector3[] Corners
    {
        get
        {
            Vector3[] res = new Vector3[corners.Length];
            Matrix4x4 YXZ = Matrix4x4.RotationXYZ(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
            for(int i=0;i<res.Length;i++)
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

    private void Start()
    {
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
        return 0f;
    }

    internal override float GetMaxXYZ(int axe)
    {
        return 0f;
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
                //return new CollisionInfo(closestPoint, )
            }
        }
        return null;
    }

    internal override Vector3 ClosestPoint(Vector3 point)
    {
        Vector3 d = point - Center;
        Vector3 closestPoint = Center;
        Vector3[] axis = Axis;
        for(int i=0;i<3;i++)
        {
            closestPoint += Mathf.Clamp(Vector3.Dot(d, axis[i]), -Size.x / 2f, Size.x / 2f) * axis[i];
        }
        return closestPoint;
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 1, 0);
        //Matrix4x4 XYZ = Matrix4x4.RotationXYZ(oldRotation.x, oldRotation.y, oldRotation.z);
        //Vector3 size = Size;
        Vector3 center = Center;
        //Vector3 p1, p2, p3, p4, p5, p6, p7, p8;
        //p1 = new Vector3(-size.x / 2f, -size.y / 2f, -size.z / 2f);
        //p2 = new Vector3(-size.x / 2f, -size.y / 2f, size.z / 2f);
        //p3 = new Vector3(-size.x / 2f, size.y / 2f, size.z / 2f);
        //p4 = new Vector3(-size.x / 2f, size.y / 2f, -size.z / 2f);
        //p5 = new Vector3(size.x / 2f, size.y / 2f, size.z / 2f);
        //p6 = new Vector3(size.x / 2f, -size.y / 2f, size.z / 2f);
        //p7 = new Vector3(size.x / 2f, size.y / 2f, -size.z / 2f);
        //p8 = new Vector3(size.x / 2f, -size.y / 2f, -size.z / 2f);
        //p1 = XYZ * p1 + Center;
        //p2 = XYZ * p2 + Center;
        //p3 = XYZ * p3 + Center;
        //p4 = XYZ * p4 + Center;
        //p5 = XYZ * p5 + Center;
        //p6 = XYZ * p6 + Center;
        //p7 = XYZ * p7 + Center;
        //p8 = XYZ * p8 + Center;
        Vector3[] corners = Corners;
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
