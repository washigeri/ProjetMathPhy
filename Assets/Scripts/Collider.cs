using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Collider : MonoBehaviour {

    protected bool Enabled { get; set; }

    protected abstract float GetMinXYZ(int axe);

    protected abstract float GetMaxXYZ(int axe);
}
