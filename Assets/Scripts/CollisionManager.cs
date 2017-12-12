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
                    ResolveCollisionWithImulse(pair, collision);
                }
            }
        }
    }

    private void ResolveCollisionWithImulse(SortAndSweep.GameObjectsPair pair, CollisionInfo collision)
    {
        GameObject gameObject1 = pair.gameObject1;
        GameObject gameObject2 = pair.gameObject2;
        RigidBodyScript rb1 = gameObject1.GetComponent<RigidBodyScript>();
        RigidBodyScript rb2 = gameObject2.GetComponent<RigidBodyScript>();

        Vector3 v1 = rb1.velocity;
        Vector3 v2 = rb2.velocity;

        Vector3 vrel = v1 - v2;
        float elasticity = Mathf.Min(rb1.restitution, rb2.restitution);

        float m1 = rb1.mass;
        float m2 = rb2.mass;
        float k = (elasticity + 1) * Vector3.Dot(vrel, collision.normalVector) / ((rb1.invMass + rb2.invMass) * Vector3.Dot(collision.normalVector, collision.normalVector));

        Vector3 w1 = rb1.angularSpeed;
        Vector3 w2 = rb2.angularSpeed;

        Vector3 r1 = collision.collisionPoint - gameObject1.transform.position;
        Vector3 r2 = collision.collisionPoint - gameObject2.transform.position;

        Vector3 u1 = Vector3.Cross(w1, collision.collisionPoint - gameObject1.transform.position);
        Vector3 u2 = Vector3.Cross(w2, collision.collisionPoint - gameObject2.transform.position);

        Vector3 urel = u1 - u2;

        float kr = (elasticity + 1) * Vector3.Dot(urel, collision.normalVector);

        Vector3 factor1 = (rb1.invMass + rb2.invMass) * collision.normalVector;
        Vector3 factor2 = Vector3.Cross(rb1.inertiaMatrixInv * (Vector3.Cross(r1, collision.normalVector)), r1);
        Vector3 factor3 = Vector3.Cross(rb2.inertiaMatrixInv * (Vector3.Cross(r2, collision.normalVector)), r2);

        kr /= Vector3.Dot(factor1 + factor2 + factor3, collision.normalVector);
        rb1.velocity -= k * collision.normalVector * rb1.invMass;
        rb2.velocity += k * collision.normalVector * rb2.invMass;

        rb1.angularSpeed -= rb1.inertiaMatrixInv * Vector3.Cross(r1, kr * collision.normalVector) * Mathf.Rad2Deg;
        rb2.angularSpeed += rb2.inertiaMatrixInv * Vector3.Cross(r2, kr * collision.normalVector) * Mathf.Rad2Deg;

        if (pair.gameObject1.GetComponent<RigidBodyScript>().invMass == 0)
        {
            Debug.Log("Stop1");
            gameObject2.GetComponent<RigidBodyScript>().velocity = Vector3.zero;
            gameObject2.GetComponent<RigidBodyScript>().angularSpeed = Vector3.zero;
            pair.gameObject2.GetComponent<RigidBodyScript>().AddForce(-1f * PhysicsManager.gravityMultiplied * pair.gameObject2.GetComponent<RigidBodyScript>().mass, collision.collisionPoint);
        }
        if (pair.gameObject2.GetComponent<RigidBodyScript>().invMass == 0)
        {
            Debug.Log("Stop2");

            gameObject1.GetComponent<RigidBodyScript>().velocity = Vector3.zero;
            gameObject1.GetComponent<RigidBodyScript>().angularSpeed = Vector3.zero;
            pair.gameObject1.GetComponent<RigidBodyScript>().AddForce(-1f * PhysicsManager.gravityMultiplied * pair.gameObject1.GetComponent<RigidBodyScript>().mass, collision.collisionPoint);
        }
    }

    private void PositionalCorrection(GameObject gameObject1, GameObject gameObject2, CollisionInfo collision)
    {
        const float percent = 0.2f;
        const float slop = 0.01f;
        float invMass1 = gameObject1.GetComponent<RigidBodyScript>().invMass;
        float invMass2 = gameObject2.GetComponent<RigidBodyScript>().invMass;
        Vector3 correction = Mathf.Max(collision.penetrationDepth - slop, 0f) / (invMass1 + invMass2) * percent * collision.normalVector;
        gameObject1.transform.position -= invMass1 * correction;
        gameObject2.transform.position -= invMass2 * correction;
    }
}