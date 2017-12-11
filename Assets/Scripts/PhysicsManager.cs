using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsManager : MonoBehaviour
{
    public static Vector3 gravity = new Vector3(0, -9.81f, 0);
    public static int gravityMultiplier = 1;

    [HideInInspector]
    public static Vector3 gravityMultiplied = gravity * gravityMultiplier;

    private ForceManager forceManager;
    private CollisionManager collisionManager;

    // Use this for initialization
    private void Awake()
    {
        DontDestroyOnLoad(this);
        CustomCollider[] colliders = FindObjectsOfType<CustomCollider>();
        collisionManager = new CollisionManager(colliders);
        RigidBodyScript[] rbs = FindObjectsOfType<RigidBodyScript>();
        forceManager = new ForceManager(rbs);
    }

    // Update is called once per frame
    private void Update()
    {
        forceManager.ForceUpdate();
        collisionManager.CollisionUpdate();
    }
}