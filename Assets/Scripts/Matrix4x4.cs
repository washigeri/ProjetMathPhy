using System;
using UnityEngine;

public class Matrix3x3
{
    private float[,] v;

    public float this[int i, int j]
    {
        get
        {
            return v[i, j];
        }
        set
        {
            v[i, j] = value;
        }
    }

    public float Determinant
    {
        get
        {
            return this[0, 0] * this[1, 1] * this[2, 2] + this[0, 1] * this[1, 2] * this[2, 0] + this[0, 2] * this[1, 0] * this[2, 1] - this[2, 0] * this[1, 1] * this[0, 2] - this[2, 1] * this[1, 2] * this[0, 0] - this[2, 2] * this[1, 0] * this[0, 1];
        }
    }

    public Matrix3x3()
    {
        v = new float[3, 3];
    }

    public Matrix3x3(Matrix4x4 m, int i, int j)
    {
        v = new float[3, 3];
        bool incrI = false;
        bool incrJ = false;
        for (int k = 0; k < 3; k++)
        {
            incrI = (k >= i);
            for (int l = 0; l < 3; l++)
            {
                incrJ = (l >= j);
                this[k, l] = m[k + (incrI ? 1 : 0), l + (incrJ ? 1 : 0)];
            }
        }
    }
}

public class Matrix4x4
{
    public override String ToString()
    {
        String res = "";
        for (int i = 0; i < 4; i++)
        {
            res += this[i, 0] + "   " + this[i, 1] + "   " + this[i, 2] + "   " + this[i, 3] + System.Environment.NewLine;
        }
        return res;
    }

    private float[,] v;

    public float this[int i, int j]
    {
        get
        {
            return v[i, j];
        }
        set
        {
            v[i, j] = value;
        }
    }

    public static Matrix4x4 Identity
    {
        get
        {
            Matrix4x4 id = new Matrix4x4();
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    id[i, j] = (i == j) ? 1 : 0;
                }
            }
            return id;
        }
    }

    public static Matrix4x4 Zero
    {
        get
        {
            return new Matrix4x4();
        }
    }

    public float Determinant
    {
        get
        {
            float res = 0f;
            res += this[0, 0] * (this[1, 1] * this[2, 2] * this[3, 3] + this[1, 2] * this[2, 3] * this[3, 1] + this[1, 3] * this[2, 1] * this[3, 2] - this[1, 3] * this[2, 2] * this[3, 1] - this[1, 2] * this[2, 1] * this[3, 3] - this[1, 1] * this[2, 3] * this[3, 2]);
            res -= this[1, 0] * (this[0, 1] * this[2, 2] * this[3, 3] + this[0, 2] * this[2, 3] * this[3, 1] + this[0, 3] * this[2, 1] * this[3, 2] - this[3, 1] * this[2, 2] * this[0, 3] - this[3, 2] * this[2, 3] * this[0, 1] - this[3, 3] * this[2, 1] * this[0, 2]);
            res += this[2, 0] * (this[0, 1] * this[1, 2] * this[3, 3] + this[0, 2] * this[1, 3] * this[3, 1] + this[0, 3] * this[1, 1] * this[3, 2] - this[3, 1] * this[1, 2] * this[0, 3] - this[3, 2] * this[1, 3] * this[0, 1] - this[3, 3] * this[1, 1] * this[0, 2]);
            res -= this[3, 0] * (this[0, 1] * this[1, 2] * this[2, 3] + this[0, 2] * this[1, 3] * this[2, 1] + this[0, 3] * this[1, 1] * this[2, 2] - this[2, 1] * this[1, 2] * this[0, 3] - this[2, 2] * this[1, 3] * this[0, 1] - this[2, 3] * this[1, 1] * this[0, 2]);
            return res;
        }
    }

    public Matrix4x4 Inverse
    {
        get
        {
            Matrix4x4 res = new Matrix4x4();
            float det = Determinant;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    res[j, i] = new Matrix3x3(this, i, j).Determinant * Mathf.Pow(-1, i + j) / det;
                }
            }
            return res;
        }
    }

    public Matrix4x4 Transpose
    {
        get
        {
            Matrix4x4 res = new Matrix4x4();
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    res[i, j] = this[j, i];
                }
            }
            return res;
        }
    }

    public Matrix4x4()
    {
        v = new float[4, 4];
    }

    public Matrix4x4(Vector4 col1, Vector4 col2, Vector4 col3, Vector4 col4)
    {
        v = new float[4, 4];
        for (int i = 0; i < 4; i++)
        {
            v[i, 0] = col1[i];
            v[i, 1] = col2[i];
            v[i, 2] = col3[i];
            v[i, 3] = col4[i];
        }
    }

    public static Matrix4x4 RotationX(float tetha)
    {
        Matrix4x4 res = new Matrix4x4();
        res[0, 0] = 1;
        res[0, 1] = 0;
        res[0, 2] = 0;
        res[0, 3] = 0;
        res[1, 0] = 0;
        res[1, 1] = Mathf.Cos(tetha * Mathf.Deg2Rad);
        res[1, 2] = -Mathf.Sin(tetha * Mathf.Deg2Rad);
        res[1, 3] = 0;
        res[2, 0] = 0;
        res[2, 1] = Mathf.Sin(tetha * Mathf.Deg2Rad);
        res[2, 2] = Mathf.Cos(tetha * Mathf.Deg2Rad);
        res[2, 3] = 0;
        res[3, 0] = 0;
        res[3, 1] = 0;
        res[3, 2] = 0;
        res[3, 3] = 1;
        return res;
    }

    public static Matrix4x4 RotationX(float tetha, Vector3 translation)
    {
        Matrix4x4 res = new Matrix4x4();
        res[0, 0] = 1;
        res[0, 1] = 0;
        res[0, 2] = 0;
        res[0, 3] = translation.x;
        res[1, 0] = 0;
        res[1, 1] = Mathf.Cos(tetha * Mathf.Deg2Rad);
        res[1, 2] = -Mathf.Sin(tetha * Mathf.Deg2Rad);
        res[1, 3] = translation.y;
        res[2, 0] = 0;
        res[2, 1] = Mathf.Sin(tetha * Mathf.Deg2Rad);
        res[2, 2] = Mathf.Cos(tetha * Mathf.Deg2Rad);
        res[2, 3] = translation.z;
        res[3, 0] = 0;
        res[3, 1] = 0;
        res[3, 2] = 0;
        res[3, 3] = 1;
        return res;
    }

    public static Matrix4x4 RotationY(float tetha)
    {
        Matrix4x4 res = new Matrix4x4();
        res[0, 0] = Mathf.Cos(tetha * Mathf.Deg2Rad);
        res[0, 1] = 0;
        res[0, 2] = Mathf.Sin(tetha * Mathf.Deg2Rad);
        res[0, 3] = 0;
        res[1, 0] = 0;
        res[1, 1] = 1;
        res[1, 2] = 0;
        res[1, 3] = 0;
        res[2, 0] = -Mathf.Sin(tetha * Mathf.Deg2Rad);
        res[2, 1] = 0;
        res[2, 2] = Mathf.Cos(tetha * Mathf.Deg2Rad);
        res[2, 3] = 0;
        res[3, 0] = 0;
        res[3, 1] = 0;
        res[3, 2] = 0;
        res[3, 3] = 1;
        return res;
    }

    public static Matrix4x4 RotationY(float tetha, Vector3 translation)
    {
        Matrix4x4 res = new Matrix4x4();
        res[0, 0] = Mathf.Cos(tetha * Mathf.Deg2Rad);
        res[0, 1] = 0;
        res[0, 2] = Mathf.Sin(tetha * Mathf.Deg2Rad);
        res[0, 3] = translation.x;
        res[1, 0] = 0;
        res[1, 1] = 1;
        res[1, 2] = 0;
        res[1, 3] = translation.y;
        res[2, 0] = -Mathf.Sin(tetha * Mathf.Deg2Rad);
        res[2, 1] = 0;
        res[2, 2] = Mathf.Cos(tetha * Mathf.Deg2Rad);
        res[2, 3] = translation.z;
        res[3, 0] = 0;
        res[3, 1] = 0;
        res[3, 2] = 0;
        res[3, 3] = 1;
        return res;
    }

    public static Matrix4x4 RotationZ(float tetha)
    {
        Matrix4x4 res = new Matrix4x4();
        res[0, 0] = Mathf.Cos(tetha * Mathf.Deg2Rad);
        res[0, 1] = -Mathf.Sin(tetha * Mathf.Deg2Rad);
        res[0, 2] = 0;
        res[0, 3] = 0;
        res[1, 0] = Mathf.Sin(tetha * Mathf.Deg2Rad);
        res[1, 1] = Mathf.Cos(tetha * Mathf.Deg2Rad);
        res[1, 2] = 0;
        res[1, 3] = 0;
        res[2, 0] = 0;
        res[2, 1] = 0;
        res[2, 2] = 1;
        res[2, 3] = 0;
        res[3, 0] = 0;
        res[3, 1] = 0;
        res[3, 2] = 0;
        res[3, 3] = 1;
        return res;
    }

    public static Matrix4x4 RotationZ(float tetha, Vector3 translation)
    {
        Matrix4x4 res = new Matrix4x4();
        res[0, 0] = Mathf.Cos(tetha * Mathf.Deg2Rad);
        res[0, 1] = -Mathf.Sin(tetha * Mathf.Deg2Rad);
        res[0, 2] = 0;
        res[0, 3] = translation.x;
        res[1, 0] = Mathf.Sin(tetha * Mathf.Deg2Rad);
        res[1, 1] = Mathf.Cos(tetha * Mathf.Deg2Rad);
        res[1, 2] = 0;
        res[1, 3] = translation.y;
        res[2, 0] = 0;
        res[2, 1] = 0;
        res[2, 2] = 1;
        res[2, 3] = translation.z;
        res[3, 0] = 0;
        res[3, 1] = 0;
        res[3, 2] = 0;
        res[3, 3] = 1;
        return res;
    }

    public static Matrix4x4 Translation(Vector3 translation)
    {
        Matrix4x4 res = new Matrix4x4();
        res[0, 0] = 1;
        res[0, 1] = 0;
        res[0, 2] = 0;
        res[0, 3] = translation.x;
        res[1, 0] = 0;
        res[1, 1] = 1;
        res[1, 2] = 0;
        res[1, 3] = translation.y;
        res[2, 0] = 0;
        res[2, 1] = 0;
        res[2, 2] = 1;
        res[2, 3] = translation.z;
        res[3, 0] = 0;
        res[3, 1] = 0;
        res[3, 2] = 0;
        res[3, 3] = 1;
        return res;
    }

    public static Matrix4x4 Scale(Vector3 scale)
    {
        Matrix4x4 res = new Matrix4x4();
        res[0, 0] = scale.x;
        res[0, 1] = 0;
        res[0, 2] = 0;
        res[0, 3] = 0;
        res[1, 0] = 0;
        res[1, 1] = scale.y;
        res[1, 2] = 0;
        res[1, 3] = 0;
        res[2, 0] = 0;
        res[2, 1] = 0;
        res[2, 2] = scale.z;
        res[2, 3] = 0;
        res[3, 0] = 0;
        res[3, 1] = 0;
        res[3, 2] = 0;
        res[3, 3] = 1;
        return res;
    }

    public bool IsIdentity()
    {
        return Equals(Identity, Mathf.Pow(10, -5));
    }

    public bool Equals(Matrix4x4 m)
    {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (Mathf.Abs(this[i, j] - m[i, j]) > Mathf.Epsilon)
                {
                    return false;
                }
            }
        }
        return true;
    }

    public bool Equals(Matrix4x4 m, float epsilon)
    {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (Mathf.Abs(this[i, j] - m[i, j]) > epsilon)
                {
                    return false;
                }
            }
        }
        return true;
    }

    public static Matrix4x4 operator +(Matrix4x4 m1, Matrix4x4 m2)
    {
        Matrix4x4 res = new Matrix4x4();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                res[i, j] = m1[i, j] + m2[i, j];
            }
        }
        return res;
    }

    public static Matrix4x4 operator *(Matrix4x4 m1, Matrix4x4 m2)
    {
        Matrix4x4 res = new Matrix4x4();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    res[i, j] += m1[i, k] * m2[k, j];
                }
            }
        }
        return res;
    }

    public static Vector3 operator *(Matrix4x4 m, Vector3 v)
    {
        Vector4 product = m * new Vector4(v.x, v.y, v.z, 1);
        Vector3 res = new Vector3(product.x, product.y, product.z);
        return res;
    }

    public static Vector4 operator *(Matrix4x4 m, Vector4 v)
    {
        Vector4 res = Vector4.zero;
        for (int i = 0; i < 4; i++)
        {
            res[i] = m[i, 0] * v[0] + m[i, 1] * v[1] + m[i, 2] * v[2] + m[i, 3] * v[3];
        }
        return res;
    }

    public static Matrix4x4 operator *(Matrix4x4 m, float x)
    {
        Matrix4x4 res = new Matrix4x4();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                res[i, j] = m[i, j] * x;
            }
        }
        return res;
    }

    public static Matrix4x4 operator *(float x, Matrix4x4 m)
    {
        Matrix4x4 res = new Matrix4x4();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                res[i, j] = m[i, j] * x;
            }
        }
        return res;
    }
}