using UnityEngine;

public class BoxCollider3D : CustomCollider
{
    public Vector3 center = Vector3.zero;

    public Vector3 size = Vector3.one;

    public Vector3 Center
    {
        get
        {
            return transform.position + center;
        }
    }

    public Vector3 Size
    {
        get
        {
            return new Vector3(size.x * transform.localScale.x, size.y * transform.localScale.y, size.z * transform.localScale.z);
        }
    }

    internal override Vector3 ClosestPoint(Vector3 point)
    {
        throw new System.NotImplementedException();
    }

    internal override float GetMaxXYZ(int axe)
    {
        if (axe == 0)
        {
            return Center.x + Size.x / 2f;
        }
        else if (axe == 1)
        {
            return Center.y + Size.y / 2f;
        }
        else if (axe == 2)
        {
            return Center.z + Size.z / 2f;
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
            return Center.x - Size.x / 2f;
        }
        else if (axe == 1)
        {
            return Center.y - Size.y / 2f;
        }
        else if (axe == 2)
        {
            return Center.z - Size.z / 2f;
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
            CollisionInfo collision = colliderSphere.IsColliding(this);
            if (collision != null)
            {
                collision.normalVector *= -1f;
            }
            return collision;
        }
        else if (collider is BoxCollider3D)
        {
            var colliderBox = (BoxCollider3D)collider;
            bool overlapX, overlapY, overlapZ;
            overlapX = (Center.x - Size.x / 2f <= colliderBox.Center.x - colliderBox.Size.x / 2f && colliderBox.Center.x - colliderBox.Size.x / 2f <= Center.x + Size.x / 2f);
            overlapY = (Center.y - Size.y / 2f <= colliderBox.Center.y - colliderBox.Size.y / 2f && colliderBox.Center.y - colliderBox.Size.y / 2f <= Center.y + Size.y / 2f);
            overlapZ = (Center.z - Size.z / 2f <= colliderBox.Center.z - colliderBox.Size.z / 2f && colliderBox.Center.z - colliderBox.Size.z / 2f <= Center.z + Size.z / 2f);
            isColliding = overlapX && overlapY && overlapZ;
            if (isColliding)
            {
                return new CollisionInfo((Center - colliderBox.Center) / 2f, (Center - colliderBox.Center).normalized, 0);
            }
        }
        return null;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 1, 0);
        Gizmos.DrawWireCube(Center, Size);
    }
}