using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CollisionManager
{
    public bool useFriction = true;
    private bool USE_COMPUTED_FRICTION = true;

    private List<GameObject> colliders;

    public void AddNewCollider(CustomCollider[] collider)
    {
        foreach (CustomCollider coll in collider)
        {
            if (!colliders.Contains(coll.gameObject))
                colliders.Add(coll.gameObject);
        }
    }

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

            foreach (SortAndSweep.GameObjectsPair pair in list)
            {
                var collision = pair.AreColliding();
                if (collision != null)
                {
                    Debug.Log("Cpollision");
                    ResolveCollisionWithImpulse(pair, collision);
                }
            }
        }
    }

    private void ResolveCollisionWithImpulse(SortAndSweep.GameObjectsPair pair, CollisionInfo collision)
    {
        GameObject gameObject1 = pair.gameObject1;
        GameObject gameObject2 = pair.gameObject2;
        RigidBodyScript rb1 = gameObject1.GetComponent<RigidBodyScript>();
        RigidBodyScript rb2 = gameObject2.GetComponent<RigidBodyScript>();

        Vector3 v1 = rb1.velocity;
        Vector3 v2 = rb2.velocity;

        if ((rb1.invMass != 0) || (rb2.invMass != 0))
        {
            Vector3 vrel = v1 - v2;
            float elasticity = Mathf.Min(rb1.restitution, rb2.restitution);
            float k = (elasticity + 1) * Vector3.Dot(vrel, collision.normalVector) / ((rb1.invMass + rb2.invMass) * Vector3.Dot(collision.normalVector, collision.normalVector));

            Vector3 w1 = rb1.angularSpeed;
            Vector3 w2 = rb2.angularSpeed;

            Vector3 u1 = Vector3.Cross(w1 * Mathf.Deg2Rad, collision.collisionPoint - gameObject1.transform.position);
            Vector3 u2 = Vector3.Cross(w2 * Mathf.Deg2Rad, collision.collisionPoint - gameObject2.transform.position);

            Vector3 urel = u1 - u2;
            Vector3 r1 = collision.collisionPoint - rb1.transform.position;
            Vector3 r2 = collision.collisionPoint - rb2.transform.position;

            float krnom = (elasticity + 1) * Vector3.Dot(urel, collision.normalVector);

            Vector3 factor1 = (rb1.invMass + rb2.invMass) * collision.normalVector;
            Vector3 factor2 = Vector3.Cross(rb1.inertiaMatrixInv * (Vector3.Cross(r1, collision.normalVector)), r1);
            Vector3 factor3 = Vector3.Cross(rb2.inertiaMatrixInv * (Vector3.Cross(r2, collision.normalVector)), r2);

            float kr = krnom / Vector3.Dot(factor1 + factor2 + factor3, collision.normalVector);

            rb1.velocity -= k * collision.normalVector * rb1.invMass;
            rb2.velocity += k * collision.normalVector * rb2.invMass;

            rb1.angularSpeed -= (rb1.inertiaMatrixInv * Vector3.Cross(r1, kr * collision.normalVector)) * Mathf.Rad2Deg;
            rb2.angularSpeed += (rb2.inertiaMatrixInv * Vector3.Cross(r2, kr * collision.normalVector)) * Mathf.Rad2Deg;

            if (useFriction)
            {
                if (!USE_COMPUTED_FRICTION)
                {
                    rb1.AddForce(-1f * rb1.staticFriction * u1);
                    rb2.AddForce(-1f * rb2.staticFriction * u2);
                }
                else
                {
                    Vector3 frictionImpulse = ComputeFrictionImpulsion(pair, collision, k);
                    rb1.velocity -= frictionImpulse * rb1.invMass;
                    rb2.velocity += frictionImpulse * rb2.invMass;
                }
            }

            PositionalCorrection(pair.gameObject1, pair.gameObject2, collision);
        }
    }

    private void PositionalCorrection(GameObject gameObject1, GameObject gameObject2, CollisionInfo collision)
    {
        const float percent = 0.2f;
        const float slop = 0.01f;
        float invMass1 = gameObject1.GetComponent<RigidBodyScript>().invMass;
        float invMass2 = gameObject2.GetComponent<RigidBodyScript>().invMass;
        Vector3 correction = Mathf.Max(Mathf.Abs(collision.penetrationDepth) - slop, 0f) / (invMass1 + invMass2) * percent * collision.normalVector;
        gameObject1.transform.position -= invMass1 * correction;
        gameObject2.transform.position += invMass2 * correction;
    }

    private Vector3 ComputeFrictionImpulsion(SortAndSweep.GameObjectsPair pair, CollisionInfo collision, float j)
    {
        RigidBodyScript rb1 = pair.gameObject1.GetComponent<RigidBodyScript>();
        RigidBodyScript rb2 = pair.gameObject2.GetComponent<RigidBodyScript>();

        Vector3 rv = rb2.velocity - rb1.velocity;
        Vector3 tangent = (rv - Vector3.Dot(rv, collision.normalVector) * collision.normalVector).normalized;
        float jt = -1f * Vector3.Dot(rv, tangent);
        jt = jt / (rb1.invMass + rb2.invMass);
        float mu = new Vector2(rb1.staticFriction, rb2.staticFriction).magnitude;

        Vector3 frictionImpulse;
        if (jt < j * mu)
        {
            frictionImpulse = jt * tangent;
        }
        else
        {
            float dynamicFriction = new Vector2(rb1.dynamicFriction, rb2.dynamicFriction).magnitude;
            frictionImpulse = -1f * tangent * dynamicFriction;
        }
        return frictionImpulse;
    }
}