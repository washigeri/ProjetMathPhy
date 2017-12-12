using UnityEngine;

public class Force
{
    public Vector3 applicationPoint;
    public Vector3 forceVector;

    public Force(float x, float y, float z, float xpos, float ypos, float zpos)
    {
        this.applicationPoint = new Vector3(xpos, ypos, zpos);
        this.forceVector = new Vector3(x, y, z);
    }

    public override bool Equals(System.Object f)
    {
        bool res = true;
        res = res && ((Force)f).applicationPoint == this.applicationPoint;
        res = res && ((Force)f).forceVector == this.forceVector;

        return res;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}