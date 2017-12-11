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

    public float radius = 1;
    public float square_size = 1;

    public float cuboid_size_x = 1;
    public float cuboid_size_y = 1;
    public float cuboid_size_z = 1;

    public float cylinder_height = 1;

    private Matrix4x4 inertiaMatrix;
    private Matrix4x4 inertiaMatrixInv;

    // Use this for initialization
    private void Awake()
    {
        forces = new List<Force>();
        velocity = new Vector3(0f, 0f, 0f);
        angularSpeed = new Vector3(0f, 0f, 0f);
        if (useGravity)
            AddForce(PhysicsManger.gravityMultiplied * mass, persistent: true);
        switch (shape)
        {
            case Shapes.Cube:
                inertiaMatrix = Matrix4x4.Identity * (mass * Mathf.Pow(square_size, 2) / 6f);
                break;

            case Shapes.Cuboid:
                inertiaMatrix = Matrix4x4.Identity * (mass / 12f);
                inertiaMatrix[0, 0] *= (Mathf.Pow(cuboid_size_x, 2) + Mathf.Pow(cuboid_size_z, 2));
                inertiaMatrix[1, 1] *= (Mathf.Pow(cuboid_size_y, 2) + Mathf.Pow(cuboid_size_z, 2));
                inertiaMatrix[2, 2] *= (Mathf.Pow(cuboid_size_y, 2) + Mathf.Pow(cuboid_size_x, 2));
                break;

            case Shapes.Sphere:
                inertiaMatrix = Matrix4x4.Identity * (mass * 2 * Mathf.Pow(radius, 2) / 5f);
                break;

            case Shapes.Cylinder:
                inertiaMatrix = Matrix4x4.Identity * (mass * ((Mathf.Pow(cylinder_height, 2) / 12f) + (Mathf.Pow(radius, 2) / 4)));
                inertiaMatrix[2, 2] = mass * (Mathf.Pow(radius, 2) / 2);
                break;
        }
        inertiaMatrixInv = inertiaMatrix.Inverse;
    }

    // Update is called once per frame
    private void Update()
    {
        Vector3 acceleration = ComputeAcceleration();
        velocity = velocity + (acceleration * Time.deltaTime);
        gameObject.transform.position = Matrix4x4.Translation(velocity * Time.deltaTime) * gameObject.transform.position;
        Vector3 angularAcceleration = ComputeAngularAcceleration();
        angularSpeed = angularSpeed + (angularAcceleration * Time.deltaTime);
        gameObject.transform.Rotate(angularSpeed * Time.deltaTime);
        UpdateForces();
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