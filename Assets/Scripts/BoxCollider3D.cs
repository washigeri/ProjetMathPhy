using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxCollider3D : Collider {

    public Vector3 center = Vector3.zero;
    public Vector3 localSize = Vector3.one;

    private Vector3 size;

    //A faire
    protected override float GetMaxXYZ(int axe)
    {
        if (axe == 0)
        {
            return 0f;
        }
        return 0f;
    }

    //A faire
    protected override float GetMinXYZ(int axe)
    {
        throw new System.NotImplementedException();
    }

    private void Update()
    {
        center = transform.position;
        size = new Vector3(localSize.x * transform.localScale.x, localSize.y * transform.localScale.y, localSize.z * transform.localScale.z);
    }

}
