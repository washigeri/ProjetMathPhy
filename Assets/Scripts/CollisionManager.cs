using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    private List<GameObject> colliders;

    // Use this for initialization
    private void Awake()
    {
        colliders = new List<GameObject>();
        CustomCollider[] collidersRes = FindObjectsOfType<CustomCollider>();
        foreach (CustomCollider collider in collidersRes)
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
        if (colliders.Count > 1)
        {
            SortAndSweep sortAndSweep = new SortAndSweep(colliders);
            List<SortAndSweep.GameObjectsPair> list = sortAndSweep.CheckForPossibleCollisions();
            Debug.Log("Collisions: " + list.Count);
        }
    }
}