using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PhysicsManager : MonoBehaviour
{
    public Vector3 gravity = new Vector3(0, -9.81f, 0);
    public float gravityMultiplier = 1f;
    public float airDensity = 1.2f;

    [HideInInspector]
    public static PhysicsManager instance = null;

    [HideInInspector]
    public Vector3 gravityMultiplied;

    [HideInInspector]
    public ForceManager forceManager;

    [HideInInspector]
    public CollisionManager collisionManager;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this);
    }

    // Use this for initialization
    private void Start()
    {
        gravityMultiplied = gravity * gravityMultiplier;
        CustomCollider[] colliders = FindObjectsOfType<CustomCollider>();
        collisionManager = new CollisionManager(colliders);
        RigidBodyScript[] rbs = FindObjectsOfType<RigidBodyScript>();
        forceManager = new ForceManager(rbs);
    }

    // Update is called once per frame
    private void Update()
    {
        gravityMultiplied = gravity * gravityMultiplier;
        CustomCollider[] colliders = FindObjectsOfType<CustomCollider>();
        RigidBodyScript[] rbs = FindObjectsOfType<RigidBodyScript>();
        forceManager.AddRB(rbs);
        collisionManager.AddNewCollider(colliders);
        forceManager.ForceUpdate();
        collisionManager.CollisionUpdate();
    }
}