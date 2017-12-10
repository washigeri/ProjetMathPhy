using UnityEngine;

public class PhysicsManger : MonoBehaviour
{
    public static Vector3 gravity = new Vector3(0, -9.81f, 0);
    public static int gravityMultiplier = 1;

    [HideInInspector]
    public static Vector3 gravityMultiplied = gravity * gravityMultiplier;

    // Use this for initialization
    private void Start()
    {
        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    private void Update()
    {
    }
}