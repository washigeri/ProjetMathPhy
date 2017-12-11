using UnityEngine;

public abstract class CustomCollider : MonoBehaviour
{
    protected bool Enabled { get; set; }

    internal abstract float GetMinXYZ(int axe);

    internal abstract float GetMaxXYZ(int axe);
}