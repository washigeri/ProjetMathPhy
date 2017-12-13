using System.Collections.Generic;
using UnityEngine;

public class ForceManager
{
    private List<GameObject> rigidBodies;

    public ForceManager(RigidBodyScript[] rigidBodyP)
    {
        rigidBodies = new List<GameObject>();
        foreach (RigidBodyScript rb in rigidBodyP)
        {
            if (rb.gameObject != null && rb.gameObject.activeInHierarchy)
            {
                rigidBodies.Add(rb.gameObject);
            }
        }
    }

    public void ForceUpdate()
    {
        foreach (GameObject rbs in rigidBodies)
        {
            rbs.GetComponent<RigidBodyScript>().NewtonUpdate();
        }
    }
}