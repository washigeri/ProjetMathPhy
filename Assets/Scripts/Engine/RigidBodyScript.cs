using System.Collections.Generic;
using System.Linq;
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
    public float linearDrag = 0.47f;
    public float angularDrag = 0.47f;
    public float restitution = 0.5f;
    public float staticFriction = 0.5f;
    public float dynamicFriction = 0.5f;

    [HideInInspector]
    public List<Force> forces;

    [HideInInspector]
    public Vector3 velocity;

    public Vector3 angularSpeed;

    [HideInInspector]
    public float invMass;

    private float radius = 1;
    private float square_size = 1;

    private float cuboid_size_x = 1;
    private float cuboid_size_y = 1;
    private float cuboid_size_z = 1;

    private float cylinder_height = 1;

    [HideInInspector]
    public Matrix4x4 inertiaMatrix;

    [HideInInspector]
    public Matrix4x4 inertiaMatrixInv;

    private Vector3 airDrag;
    private float areaDrag = 0f;

    // Use this for initialization
    private void Awake()
    {
        if (mass == 0)
            invMass = 0;
        else
            invMass = 1f / mass;
        forces = new List<Force>();
        velocity = new Vector3(0f, 0f, 0f);
        angularSpeed = new Vector3(0f, 0f, 0f);

        Vector3 size;

        if (transform.rotation.eulerAngles != Vector3.zero)
        {
            Vector3 previous = transform.rotation.eulerAngles;
            transform.rotation = Quaternion.identity;
            size = GetComponent<Renderer>().bounds.size;
            transform.rotation = Quaternion.Euler(previous);
        }
        else
        {
            size = GetComponent<Renderer>().bounds.size;
        }
        cuboid_size_x = size.x;
        cuboid_size_y = size.y;
        cuboid_size_z = size.z;

        square_size = Mathf.Max(size.x, size.y, size.z);
        radius = Mathf.Max(size.x, size.y, size.z) / 2;

        cylinder_height = size.z;

        switch (shape)
        {
            case Shapes.Cube:
                inertiaMatrix = Matrix4x4.Identity * (mass * Mathf.Pow(square_size, 2) / 6f);
                areaDrag = Mathf.Pow(square_size, 2);
                break;

            case Shapes.Cuboid:
                inertiaMatrix = Matrix4x4.Identity * (mass / 12f);
                inertiaMatrix[0, 0] *= (Mathf.Pow(cuboid_size_x, 2) + Mathf.Pow(cuboid_size_z, 2));
                inertiaMatrix[1, 1] *= (Mathf.Pow(cuboid_size_y, 2) + Mathf.Pow(cuboid_size_z, 2));
                inertiaMatrix[2, 2] *= (Mathf.Pow(cuboid_size_y, 2) + Mathf.Pow(cuboid_size_x, 2));
                areaDrag = Mathf.Pow(Mathf.Max(cuboid_size_x, cuboid_size_y, cuboid_size_z), 2);
                break;

            case Shapes.Sphere:
                inertiaMatrix = Matrix4x4.Identity * (mass * 2f * Mathf.Pow(radius, 2) / 5f);
                areaDrag = Mathf.Pow(radius, 2) * Mathf.PI;
                break;

            case Shapes.Cylinder:
                inertiaMatrix = Matrix4x4.Identity * (mass * ((Mathf.Pow(cylinder_height, 2) / 12f) + (Mathf.Pow(radius, 2) / 4)));
                inertiaMatrix[2, 2] = mass * (Mathf.Pow(radius, 2) / 2);
                areaDrag = Mathf.Max(Mathf.PI * radius * radius, 2 * Mathf.PI * radius * cylinder_height);
                break;
        }
        inertiaMatrixInv = inertiaMatrix.Inverse;
    }

    // Update is called once per frame
    public void NewtonUpdate()
    {
        airDrag = -1f * 0.5f * PhysicsManager.airDensity * linearDrag * areaDrag * velocity.sqrMagnitude * velocity.normalized;
        AddForce(airDrag);
        if (useGravity)
            AddForce(PhysicsManager.gravityMultiplied * mass);

        Vector3 acceleration = ComputeAcceleration();
        Vector3 angularAcceleration = ComputeAngularAcceleration();
        velocity = velocity + (acceleration * Time.deltaTime);
        gameObject.transform.position = Matrix4x4.Translation(velocity * Time.deltaTime) * gameObject.transform.position;
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
        acceleration *= invMass;
        return acceleration;
    }

    private Vector3 ComputeAngularAcceleration()
    {
        Vector3 res = new Vector3(0, 0, 0);
        Vector3 angularDragM = -1f * 0.5f * angularDrag * PhysicsManager.airDensity * areaDrag * (angularSpeed * Mathf.Deg2Rad).sqrMagnitude * angularSpeed.normalized;
        res += angularDragM;
        foreach (Force force in forces)
        {
            //Debug.Log("Force count : " + forces.Count);
            Vector3 goPos = gameObject.transform.position;
            if (!force.applicationPoint.Equals(goPos))
            {
                Vector3 value = Vector3.Cross(new Vector3(force.applicationPoint.x - goPos.x, force.applicationPoint.y - goPos.y, force.applicationPoint.z - goPos.z), force.forceVector);
                res += value;
            }
        }
        res = inertiaMatrixInv * res;
        return res * Mathf.Rad2Deg;
    }

    public void UpdateForces()
    {
        forces.Clear();
    }

    public void AddForce(Vector3 force)
    {
        Vector3 position = gameObject.transform.position;
        Force forceF = new Force(force.x, force.y, force.z, position.x, position.y, position.z);

        forces.Add(forceF);
    }

    public void AddForce(Vector3 force, Vector3 point)
    {
        Force forceF = new Force(force.x, force.y, force.z, point.x, point.y, point.z);

        forces.Add(forceF);
    }
}