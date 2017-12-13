using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PhysicsManager : MonoBehaviour
{
    public static Vector3 gravity = new Vector3(0, -9.81f, 0);
    public static float gravityMultiplier = 1f;
    public static float airDensity = 1.2f;

    [HideInInspector]
    public static Vector3 gravityMultiplied = gravity * gravityMultiplier;

    private ForceManager forceManager;
    private CollisionManager collisionManager;

    // Use this for initialization
    private void Awake()
    {
        CustomCollider[] colliders = FindObjectsOfType<CustomCollider>();
        collisionManager = new CollisionManager(colliders);
        RigidBodyScript[] rbs = FindObjectsOfType<RigidBodyScript>();
        forceManager = new ForceManager(rbs);
    }

    // Update is called once per frame
    private void Update()
    {
        CustomCollider[] colliders = FindObjectsOfType<CustomCollider>();
        RigidBodyScript[] rbs = FindObjectsOfType<RigidBodyScript>();
        forceManager.AddRB(rbs);
        collisionManager.AddNewCollider(colliders);
        forceManager.ForceUpdate();
        collisionManager.CollisionUpdate();
    }
}