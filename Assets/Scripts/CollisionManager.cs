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
            foreach (SortAndSweep.GameObjectsPair pair in list)
            {
                var collision = pair.AreColliding();
                if (collision != null)
                {
                    Debug.Log("Collision");
                    Vector3 v1 = pair.gameObject1.GetComponent<RigidBodyScript>().velocity;
                    Vector3 v2 = pair.gameObject2.GetComponent<RigidBodyScript>().velocity;

                    Vector3 vrel = v1 - v2;

                    float m1 = pair.gameObject1.GetComponent<RigidBodyScript>().mass;
                    float m2 = pair.gameObject2.GetComponent<RigidBodyScript>().mass;
                    float k = (PhysicsManager.elasticity + 1) * Vector3.Dot(vrel, collision.normalVector) / (((1f / m1) + (1f / m2)) * Vector3.Dot(collision.normalVector, collision.normalVector));

                    Vector3 w1 = pair.gameObject1.GetComponent<RigidBodyScript>().angularSpeed;
                    Vector3 w2 = pair.gameObject2.GetComponent<RigidBodyScript>().angularSpeed;

                    Vector3 r1 = collision.collisionPoint - pair.gameObject1.transform.position;
                    Vector3 r2 = collision.collisionPoint - pair.gameObject2.transform.position;

                    Vector3 u1 = Vector3.Cross(w1, collision.collisionPoint - pair.gameObject1.transform.position);
                    Vector3 u2 = Vector3.Cross(w2, collision.collisionPoint - pair.gameObject2.transform.position);

                    Vector3 urel = u1 - u2;

                    float kr = (PhysicsManager.elasticity + 1) * Vector3.Dot(urel, collision.normalVector);

                    Vector3 factor1 = (1f / m1 + 1f / m2) * collision.normalVector;
                    Vector3 factor2 = Vector3.Cross(pair.gameObject1.GetComponent<RigidBodyScript>().inertiaMatrixInv * (Vector3.Cross(r1, collision.normalVector)), r1);
                    Vector3 factor3 = Vector3.Cross(pair.gameObject2.GetComponent<RigidBodyScript>().inertiaMatrixInv * (Vector3.Cross(r2, collision.normalVector)), r2);

                    kr /= Vector3.Dot(factor1 + factor2 + factor3, collision.normalVector);
                    pair.gameObject1.GetComponent<RigidBodyScript>().velocity += Mathf.Abs(k) * collision.normalVector / m1;
                    pair.gameObject2.GetComponent<RigidBodyScript>().velocity -= Mathf.Abs(k) * collision.normalVector / m2;

                    pair.gameObject1.GetComponent<RigidBodyScript>().angularSpeed += pair.gameObject1.GetComponent<RigidBodyScript>().inertiaMatrixInv * Vector3.Cross(r1, kr * collision.normalVector) * Mathf.Rad2Deg;
                    pair.gameObject2.GetComponent<RigidBodyScript>().angularSpeed -= pair.gameObject2.GetComponent<RigidBodyScript>().inertiaMatrixInv * Vector3.Cross(r2, kr * collision.normalVector) * Mathf.Rad2Deg;

                    pair.gameObject1.GetComponent<RigidBodyScript>().AddForce(PhysicsManager.frictionCoeff * u1);
                    pair.gameObject2.GetComponent<RigidBodyScript>().AddForce(PhysicsManager.frictionCoeff * u2);

                    if (!pair.gameObject1.GetComponent<RigidBodyScript>().isMoving)
                    {
                        pair.gameObject2.GetComponent<RigidBodyScript>().velocity = Vector3.zero;
                        pair.gameObject2.GetComponent<RigidBodyScript>().angularSpeed = Vector3.zero;
                        pair.gameObject2.GetComponent<RigidBodyScript>().AddForce(-1f * PhysicsManager.gravityMultiplied * pair.gameObject2.GetComponent<RigidBodyScript>().mass, collision.collisionPoint);
                    }
                    if (!pair.gameObject2.GetComponent<RigidBodyScript>().isMoving)
                    {
                        pair.gameObject1.GetComponent<RigidBodyScript>().velocity = Vector3.zero;
                        pair.gameObject1.GetComponent<RigidBodyScript>().angularSpeed = Vector3.zero;
                        pair.gameObject1.GetComponent<RigidBodyScript>().AddForce(-1f * PhysicsManager.gravityMultiplied * pair.gameObject1.GetComponent<RigidBodyScript>().mass, collision.collisionPoint);
                    }
                }
            }
        }
    }
}