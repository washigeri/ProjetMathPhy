using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float spawnSpeed = 20f;
    public float cooldown = 1.5f;
    private float count = 0f;

    public float moveSpeed = 10f;

    public float turnSpeed = 4.0f;      // Speed of camera turning when mouse moves in along an axis
    public float panSpeed = 4.0f;       // Speed of the camera when being panned
    public float zoomSpeed = 4.0f;      // Speed of the camera going back and forth

    private Vector3 mouseOrigin;    // Position of cursor when mouse dragging starts
    private bool isPanning;     // Is the camera being panned?
    private bool isRotating;    // Is the camera being rotated?
    private bool isZooming;     // Is the camera zooming?

    //private string Sphere = "Prefabs/Sphere";
    //private bool sphereSelected = true;

    //private string Cube = "Prefabs/Cube";
    //private bool cubeSelected = false;

    private int objectSelectIndex = 0;
    private int prefabsCount = 3;
    private string[] objects = new string[] { "Prefabs/Sphere", "Prefabs/Cube", "Prefabs/OBB" };

    private bool physicsDisabled = false;

    private bool rotateOBBDisabled = true;

    private string item;

    private Vector3 gravity;
    private float airDensity;

    private void Start()
    {
        gravity = PhysicsManager.instance.gravity;
        airDensity = PhysicsManager.instance.airDensity;
    }

    private void Update()
    {
        //if (sphereSelected)
        //    item = Sphere;
        //else if (cubeSelected)
        //    item = Cube;
        // Get the left mouse button
        if (Input.GetMouseButtonDown(0))
        {
            // Get mouse origin
            mouseOrigin = Input.mousePosition;
            isRotating = true;
        }

        // Get the right mouse button
        if (Input.GetMouseButtonDown(1))
        {
            // Get mouse origin
            mouseOrigin = Input.mousePosition;
            isPanning = true;
        }

        // Get the middle mouse button
        if (Input.GetMouseButtonDown(2))
        {
            // Get mouse origin
            mouseOrigin = Input.mousePosition;
            isZooming = true;
        }

        // Disable movements on button release
        if (!Input.GetMouseButton(0)) isRotating = false;
        if (!Input.GetMouseButton(1)) isPanning = false;
        if (!Input.GetMouseButton(2)) isZooming = false;

        // Rotate camera along X and Y axis
        if (isRotating)
        {
            Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - mouseOrigin);

            transform.RotateAround(transform.position, transform.right, -pos.y * turnSpeed);
            transform.RotateAround(transform.position, Vector3.up, pos.x * turnSpeed);
        }

        // Move the camera on it's XY plane
        if (isPanning)
        {
            Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - mouseOrigin);

            Vector3 move = new Vector3(pos.x * panSpeed, pos.y * panSpeed, 0);
            transform.Translate(move, Space.Self);
        }

        // Move the camera linearly along Z axis
        if (isZooming)
        {
            Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - mouseOrigin);

            Vector3 move = pos.y * zoomSpeed * transform.forward;
            transform.Translate(move, Space.World);
        }

        if (count == 0)
        {
            if (Input.GetButtonDown("Jump"))
            {
                GameObject sphere = Instantiate(Resources.Load(objects[objectSelectIndex]), transform.position, Quaternion.identity) as GameObject;
                sphere.GetComponent<RigidBodyScript>().AddForce(transform.forward.normalized * spawnSpeed * 200f);
                count = cooldown;
            }
        }
        else
        {
            count -= Time.deltaTime;
            if (count < 0)
                count = 0;
        }

        if (Input.GetButtonDown("Fire1"))
        {
            if (!physicsDisabled)
            {
                Debug.Log("Physics : OFF");
                PhysicsManager pm = PhysicsManager.instance;
                pm.collisionManager.useFriction = false;
                pm.airDensity = 0;
                pm.gravity = Vector3.zero;
                physicsDisabled = true;
            }
            else
            {
                Debug.Log("Physics : ON");
                PhysicsManager pm = PhysicsManager.instance;
                pm.collisionManager.useFriction = true;
                pm.airDensity = airDensity;
                pm.gravity = gravity;
                physicsDisabled = false;
            }
        }
        if (Input.GetButtonDown("Fire2"))
        {
            //if (sphereSelected)
            //{
            //    Debug.Log("Sphere selected");
            //    sphereSelected = false;
            //    cubeSelected = true;
            //}
            //else
            //{
            //    Debug.Log("Cube selected");
            //    sphereSelected = true;
            //    cubeSelected = false;
            //}
            objectSelectIndex++;
            objectSelectIndex %= prefabsCount;
        }
        if (Input.GetButtonDown("Fire3"))
        {
            rotateOBBDisabled = !rotateOBBDisabled;
            GameObject[] OBBs = GameObject.FindGameObjectsWithTag("OBB");
            foreach(GameObject go in OBBs)
            {
                go.GetComponent<OBBCollider3D>().doRotate = !go.GetComponent<OBBCollider3D>().doRotate;
            }
        }
    }
}