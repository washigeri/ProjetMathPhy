using System.Collections.Generic;
using UnityEngine;

public class ForceManager
{
    private List<GameObject> rigidBodies;

    public void AddRB(RigidBodyScript[] rigidBody)
    {
        foreach (RigidBodyScript rb in rigidBody)
        {
            if (!rigidBodies.Contains(rb.gameObject))
                rigidBodies.Add(rb.gameObject);
        }
    }

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