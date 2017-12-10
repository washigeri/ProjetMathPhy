using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    private List<GameObject> colliders;

    // Use this for initialization
    private void Awake()
    {
        Collider[] collidersRes = FindObjectsOfType<Collider>();
        foreach (Collider collider in collidersRes)
        {
            if (collider.gameObject != null && collider.gameObject.activeInHierarchy)
            {
                colliders.Add(collider.gameObject);
            }
        }
    }

    // Update is called once per frame
    private void Update()
    {
    }
}