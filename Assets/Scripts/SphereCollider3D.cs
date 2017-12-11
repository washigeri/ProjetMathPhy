using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereCollider3D : Collider {

    public Vector3 center = Vector3.zero;

    public float localRadius = 0.5f;

    private float radius;

    protected override float GetMinXYZ(int axe)
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

    protected override float GetMaxXYZ(int axe)
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

    private void Update()
    {
        center = transform.position;
        radius = localRadius * Mathf.Max(transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

}
