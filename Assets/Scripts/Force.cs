using UnityEngine;

public class Force
{
    public Vector3 applicationPoint;
    public Vector3 forceVector;
    public bool persistent = false;

    public Force(float x, float y, float z, float xpos, float ypos, float zpos, bool persistent)
    {
        this.applicationPoint = new Vector3(xpos, ypos, zpos);
        this.forceVector = new Vector3(x, y, z);
        this.persistent = persistent;
    }
}