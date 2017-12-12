using UnityEngine;

public class BoxCollider3D : CustomCollider
{
    public Vector3 center = Vector3.zero;

    public Vector3 size = Vector3.zero;

    public Vector3 Size
    {
        get
        {
            return new Vector3(size.x * transform.localScale.x, size.y * transform.localScale.y, size.z * transform.localScale.z);
        }
    }

    protected override void Awake()
    {
        base.Awake();
        center = bounds.center;
        size = bounds.size;
    }

    internal override Vector3 ClosestPoint(Vector3 point)
    {
        throw new System.NotImplementedException();
    }

    internal override float GetMaxXYZ(int axe)
    {
        if (axe == 0)
        {
            return bounds.max.x;
        }
        else if (axe == 1)
        {
            return bounds.max.y;
        }
        else if (axe == 2)
        {
            return bounds.max.z;
        }
        else
        {
            return -Mathf.Infinity;
        }
    }

    internal override float GetMinXYZ(int axe)
    {
        if (axe == 0)
        {
            return bounds.min.x;
        }
        else if (axe == 1)
        {
            return bounds.min.y;
        }
        else if (axe == 2)
        {
            return bounds.min.z;
        }
        else
        {
            return Mathf.Infinity;
        }
    }

    internal override CollisionInfo IsColliding(CustomCollider collider)
    {
        bool isColliding = false;
        if (collider is SphereCollider3D)
        {
            var colliderSphere = (SphereCollider3D)collider;
            return colliderSphere.IsColliding(this);
        }
        else if (collider is BoxCollider3D)
        {
            var colliderBox = (BoxCollider3D)collider;
            isColliding = (center.x + size.x / 2f < colliderBox.center.x - colliderBox.size.x / 2f) || (colliderBox.center.x + colliderBox.size.x / 2f < center.x - size.x / 2f);
            if(!isColliding)
            {
                isColliding |= (center.y + size.y / 2f < colliderBox.center.y - colliderBox.size.y / 2f) || (colliderBox.center.y + colliderBox.size.y / 2f < center.y - size.y / 2f);
                if (!isColliding)
                {
                    isColliding |= (center.z + size.z / 2f < colliderBox.center.z - colliderBox.size.z / 2f) || (colliderBox.center.z + colliderBox.size.z / 2f < center.z - size.z / 2f);
                }
            }
            if (isColliding)
            {
                return new CollisionInfo((center - colliderBox.center) / 2f, (center - colliderBox.center).normalized);
            }
        }
        return null;
    }
    
    protected override void Update()
    {
        base.Update();
        center = bounds.center;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 1, 0);
        Gizmos.DrawWireCube(center, size);
    }
}