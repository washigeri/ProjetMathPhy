using System.Collections.Generic;
using UnityEngine;

public enum Shapes
{
    Cube = 0,
    Sphere = 1,
    Cylinder = 2,
    Cuboid = 3
}

public class RigidBodyScript : MonoBehaviour
{
    public float mass = 1f;
    public bool useGravity = true;
    public Shapes shape;

    [HideInInspector]
    public List<Force> forces;

    private Vector3 velocity;
    private Vector3 angularSpeed;

    private Vector3 bounds = new Vector3(0, 0, 0);

    private Matrix4x4 inertiaMatrix;
    private Matrix4x4 inertiaMatrixInv;

    // Use this for initialization
    private void Awake()
    {
        Renderer render = gameObject.GetComponent<Renderer>();
        if (render != null && render.bounds.size != new Vector3(0, 0, 0))
            bounds = render.bounds.size;
        forces = new List<Force>();
        velocity = new Vector3(0f, 0f, 0f);
        angularSpeed = new Vector3(0f, 0f, 0f);
        if (useGravity)
            AddForce(PhysicsManger.gravityMultiplied * mass, persistent: true);
        switch (shape)
        {
            case Shapes.Cube:
                break;

            case Shapes.Cuboid:
                break;

            case Shapes.Sphere:
                break;

            case Shapes.Cylinder:
                break;
        }
        inertiaMatrixInv = inertiaMatrix.inverse;
    }

    // Update is called once per frame
    private void Update()
    {
        Vector3 acceleration = this.ComputeAcceleration();
        Debug.Log("Acceleration :" + acceleration.ToString());
        velocity = velocity + (acceleration * Time.deltaTime);
        gameObject.transform.position = gameObject.transform.position + (velocity * Time.deltaTime);
        //TODO : MODIFY POSITION
        Vector3 angularAcceleration = ComputeAngularAcceleration();
        angularSpeed = angularSpeed + (angularAcceleration * Time.deltaTime);
        gameObject.transform.rotation *= Quaternion.Euler(angularSpeed * Time.deltaTime);
    }

    private Vector3 ComputeAcceleration()
    {
        Vector3 acceleration = new Vector3();
        foreach (Force force in forces)
        {
            acceleration += force.forceVector;
        }
        acceleration /= mass;
        return acceleration;
    }

    private Vector3 ComputeAngularAcceleration()
    {
        Vector3 res = new Vector3(0, 0, 0);
        foreach (Force force in forces)
        {
            Vector3 goPos = gameObject.transform.position;
            if (!force.applicationPoint.Equals(goPos))
            {
                Vector3 value = Vector3.Cross(new Vector3(force.applicationPoint.x - goPos.x, force.applicationPoint.y - goPos.y, force.applicationPoint.z - goPos.z), force.forceVector);
                res += value;
            }
        }
        res = inertiaMatrixInv * res;
        return res;
    }

    public void UpdateForces()
    {
        foreach (Force force in forces)
        {
            if (!force.persistent)
            {
                forces.Remove(force);
            }
        }
    }

    public void AddForce(Vector3 force, bool persistent = false)
    {
        Vector3 position = gameObject.transform.position;
        forces.Add(new Force(force.x, force.y, force.z, position.x, position.y, position.z, persistent));
    }

    public void AddForce(Vector3 force, Vector3 point, bool persistent = false)
    {
        forces.Add(new Force(force.x, force.y, force.z, point.x, point.y, point.z, persistent));
    }
}