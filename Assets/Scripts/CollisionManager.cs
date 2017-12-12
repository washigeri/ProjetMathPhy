using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CollisionManager
{
    private List<GameObject> colliders;

    public CollisionManager(CustomCollider[] colliderRes)
    {
        colliders = new List<GameObject>();
        foreach (CustomCollider collider in colliderRes)
        {
            if (collider.gameObject != null && collider.gameObject.activeInHierarchy)
            {
                colliders.Add(collider.gameObject);
            }
        }
    }

    public void CollisionUpdate()
    {
        if (colliders.Count > 1)
        {
            SortAndSweep sortAndSweep = new SortAndSweep(colliders);
            List<SortAndSweep.GameObjectsPair> list = sortAndSweep.CheckForPossibleCollisions();
            Debug.Log("Sort and sweep result length : " + list.Count);
            //list = list.Where(o => o.AreColliding()).ToList();
            Debug.Log("Filter length : " + list.Count);
        }
    }
}