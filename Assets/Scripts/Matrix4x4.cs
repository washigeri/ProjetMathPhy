using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Matrix3x3
{
   
    private float[][] v;

    public float Determinant
    {
        get
        {
            return v[0][0] * v[1][1] * v[2][2] + v[0][1] * v[1][2] * v[2][0] + v[0][2] * v[1][0] * v[2][1] - v[2][0] * v[1][1] * v[0][2] - v[2][1] * v[1][2] * v[0][0] - v[2][2] * v[1][0] * v[0][1];
        }
    }

    public Matrix3x3()
    {
        v = new float[3][];
        for (int i = 0; i < 3; i++)
        {
            v[i] = new float[3];
        }
    }

    public Matrix3x3(Matrix4x4 m, int i, int j)
    {
        v = new float[3][];
        bool incrI = false;
        bool incrJ = false;
        float[][] matrix = m.GetValues();
        for (int k = 0; k < 3; k++)
        {
            v[k] = new float[3];
            incrI = (k >= i);
            for (int l = 0; l < 3; l++)
            {
                incrJ = (l >= j);
                v[k][l] = matrix[k + (incrI ? 1 : 0)][l + (incrJ ? 1 : 0)];
            }
        }
    }
}

public class Matrix4x4 : MonoBehaviour
{

    private void Update()
    {
        Matrix4x4 m = RotationX(45);
        Debug.Log(m);
        Matrix4x4 id = Identity();
        Debug.Log(m * Vector3.one);
    }

    public override String ToString()
    {
        String res = "";
        for (int i = 0; i < 4; i++)
        {
            res += v[i][0] + "   " + v[i][1] + "   " + v[i][2] + "   " + v[i][3] + System.Environment.NewLine;
        }
        return res;
    }

    private float[][] v;

    public float Determinant
    {
        get
        {
            float res = 0f;
            res += v[0][0] * (v[1][1] * v[2][2] * v[3][3] + v[1][2] * v[2][3] * v[3][1] + v[1][3] * v[2][1] * v[3][2] - v[1][3] * v[2][2] * v[3][1] - v[1][2] * v[2][1] * v[3][3] - v[1][1] * v[2][3] * v[3][2]);
            res -= v[1][0] * (v[0][1] * v[2][2] * v[3][3] + v[0][2] * v[2][3] * v[3][1] + v[0][3] * v[2][1] * v[3][2] - v[3][1] * v[2][2] * v[0][3] - v[3][2] * v[2][3] * v[0][1] - v[3][3] * v[2][1] * v[0][2]);
            res += v[2][0] * (v[0][1] * v[1][2] * v[3][3] + v[0][2] * v[1][3] * v[3][1] + v[0][3] * v[1][1] * v[3][2] - v[3][1] * v[1][2] * v[0][3] - v[3][2] * v[1][3] * v[0][1] - v[3][3] * v[1][1] * v[0][2]);
            res -= v[3][0] * (v[0][1] * v[1][2] * v[2][3] + v[0][2] * v[1][3] * v[2][1] + v[0][3] * v[1][1] * v[2][2] - v[2][1] * v[1][2] * v[0][3] - v[2][2] * v[1][3] * v[0][1] - v[2][3] * v[1][1] * v[0][2]);
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
                    res.v[j][i] = new Matrix3x3(this, i, j).Determinant * Mathf.Pow(-1, i + j) / det;
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
            for(int i=0; i < 4; i++)
            {
                for(int j = 0; j < 4; j++)
                {
                    res.v[i][j] = v[j][i];
                }
            }
            return res;
        }
    }

    public Matrix4x4()
    {
        v = new float[4][];
        for (int i = 0; i < 4; i++)
        {
            v[i] = new float[4];
        }
    }

    public float[][] GetValues()
    {
        return v;
    }

    public static Matrix4x4 RotationX(float tetha)
    {
        Matrix4x4 res = new Matrix4x4();
        res.v[0][0] = 1;
        res.v[0][1] = 0;
        res.v[0][2] = 0;
        res.v[0][3] = 0;
        res.v[1][0] = 0;
        res.v[1][1] = Mathf.Cos(tetha * Mathf.Deg2Rad);
        res.v[1][2] = -Mathf.Sin(tetha * Mathf.Deg2Rad);
        res.v[1][3] = 0;
        res.v[2][0] = 0;
        res.v[2][1] = Mathf.Sin(tetha * Mathf.Deg2Rad);
        res.v[2][2] = Mathf.Cos(tetha * Mathf.Deg2Rad);
        res.v[2][3] = 0;
        res.v[3][0] = 0;
        res.v[3][1] = 0;
        res.v[3][2] = 0;
        res.v[3][3] = 1;
        return res;
    }

    public static Matrix4x4 RotationX(float tetha, Vector3 translation)
    {
        Matrix4x4 res = new Matrix4x4();
        res.v[0][0] = 1;
        res.v[0][1] = 0;
        res.v[0][2] = 0;
        res.v[0][3] = translation.x;
        res.v[1][0] = 0;
        res.v[1][1] = Mathf.Cos(tetha * Mathf.Deg2Rad);
        res.v[1][2] = -Mathf.Sin(tetha * Mathf.Deg2Rad);
        res.v[1][3] = translation.y;
        res.v[2][0] = 0;
        res.v[2][1] = Mathf.Sin(tetha * Mathf.Deg2Rad);
        res.v[2][2] = Mathf.Cos(tetha * Mathf.Deg2Rad);
        res.v[2][3] = translation.z;
        res.v[3][0] = 0;
        res.v[3][1] = 0;
        res.v[3][2] = 0;
        res.v[3][3] = 1;
        return res;
    }

    public static Matrix4x4 RotationY(float tetha)
    {
        Matrix4x4 res = new Matrix4x4();
        res.v[0][0] = Mathf.Cos(tetha * Mathf.Deg2Rad);
        res.v[0][1] = 0;
        res.v[0][2] = Mathf.Sin(tetha * Mathf.Deg2Rad);
        res.v[0][3] = 0;
        res.v[1][0] = 0;
        res.v[1][1] = 1;
        res.v[1][2] = 0;
        res.v[1][3] = 0;
        res.v[2][0] = -Mathf.Sin(tetha * Mathf.Deg2Rad);
        res.v[2][1] = 0;
        res.v[2][2] = Mathf.Cos(tetha * Mathf.Deg2Rad);
        res.v[2][3] = 0;
        res.v[3][0] = 0;
        res.v[3][1] = 0;
        res.v[3][2] = 0;
        res.v[3][3] = 1;
        return res;
    }

    public static Matrix4x4 RotationY(float tetha, Vector3 translation)
    {
        Matrix4x4 res = new Matrix4x4();
        res.v[0][0] = Mathf.Cos(tetha * Mathf.Deg2Rad);
        res.v[0][1] = 0;
        res.v[0][2] = Mathf.Sin(tetha * Mathf.Deg2Rad);
        res.v[0][3] = translation.x;
        res.v[1][0] = 0;
        res.v[1][1] = 1;
        res.v[1][2] = 0;
        res.v[1][3] = translation.y;
        res.v[2][0] = -Mathf.Sin(tetha * Mathf.Deg2Rad);
        res.v[2][1] = 0;
        res.v[2][2] = Mathf.Cos(tetha * Mathf.Deg2Rad);
        res.v[2][3] = translation.z;
        res.v[3][0] = 0;
        res.v[3][1] = 0;
        res.v[3][2] = 0;
        res.v[3][3] = 1;
        return res;
    }

    public static Matrix4x4 RotationZ(float tetha)
    {
        Matrix4x4 res = new Matrix4x4();
        res.v[0][0] = Mathf.Cos(tetha * Mathf.Deg2Rad);
        res.v[0][1] = -Mathf.Sin(tetha * Mathf.Deg2Rad);
        res.v[0][2] = 0;
        res.v[0][3] = 0;
        res.v[1][0] = Mathf.Sin(tetha * Mathf.Deg2Rad);
        res.v[1][1] = Mathf.Cos(tetha * Mathf.Deg2Rad);
        res.v[1][2] = 0;
        res.v[1][3] = 0;
        res.v[2][0] = 0;
        res.v[2][1] = 0;
        res.v[2][2] = 1;
        res.v[2][3] = 0;
        res.v[3][0] = 0;
        res.v[3][1] = 0;
        res.v[3][2] = 0;
        res.v[3][3] = 1;
        return res;
    }

    public static Matrix4x4 RotationZ(float tetha, Vector3 translation)
    {
        Matrix4x4 res = new Matrix4x4();
        res.v[0][0] = Mathf.Cos(tetha * Mathf.Deg2Rad);
        res.v[0][1] = -Mathf.Sin(tetha * Mathf.Deg2Rad);
        res.v[0][2] = 0;
        res.v[0][3] = translation.x;
        res.v[1][0] = Mathf.Sin(tetha * Mathf.Deg2Rad);
        res.v[1][1] = Mathf.Cos(tetha * Mathf.Deg2Rad);
        res.v[1][2] = 0;
        res.v[1][3] = translation.y;
        res.v[2][0] = 0;
        res.v[2][1] = 0;
        res.v[2][2] = 1;
        res.v[2][3] = translation.z;
        res.v[3][0] = 0;
        res.v[3][1] = 0;
        res.v[3][2] = 0;
        res.v[3][3] = 1;
        return res;
    }

    public static Matrix4x4 Translation(Vector3 translation)
    {
        Matrix4x4 res = new Matrix4x4();
        res.v[0][0] = 1;
        res.v[0][1] = 0;
        res.v[0][2] = 0;
        res.v[0][3] = translation.x;
        res.v[1][0] = 0;
        res.v[1][1] = 1;
        res.v[1][2] = 0;
        res.v[1][3] = translation.y;
        res.v[2][0] = 0;
        res.v[2][1] = 0;
        res.v[2][2] = 1;
        res.v[2][3] = translation.z;
        res.v[3][0] = 0;
        res.v[3][1] = 0;
        res.v[3][2] = 0;
        res.v[3][3] = 1;
        return res;
    }

    public static Matrix4x4 Scale(Vector3 scale)
    {
        Matrix4x4 res = new Matrix4x4();
        res.v[0][0] = scale.x;
        res.v[0][1] = 0;
        res.v[0][2] = 0;
        res.v[0][3] = 0;
        res.v[1][0] = 0;
        res.v[1][1] = scale.y;
        res.v[1][2] = 0;
        res.v[1][3] = 0;
        res.v[2][0] = 0;
        res.v[2][1] = 0;
        res.v[2][2] = scale.z;
        res.v[2][3] = 0;
        res.v[3][0] = 0;
        res.v[3][1] = 0;
        res.v[3][2] = 0;
        res.v[3][3] = 1;
        return res;
    }

    public static Matrix4x4 Identity()
    {
        return Scale(Vector3.one);
    }

    public bool IsIdentity()
    {
        return Equals(Identity());
    }

    public bool Equals(Matrix4x4 matrix)
    {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (Mathf.Abs(v[i][j] - matrix.v[i][j]) > Mathf.Epsilon)
                {
                    return false;
                }
            }
        }
        return true;
    }

    public bool Equals(Matrix4x4 matrix, float epsilon)
    {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (Mathf.Abs(v[i][j] - matrix.v[i][j]) > epsilon)
                {
                    return false;
                }
            }
        }
        return true;
    }

    public static Matrix4x4 operator + (Matrix4x4 m1, Matrix4x4 m2)
    {
        Matrix4x4 res = new Matrix4x4();
        for (int i=0; i<4; i++)
        {
            for(int j=0; j<4; j++)
            {
                res.v[i][j] = m1.v[i][j] + m2.v[i][j];
            }
        }
        return res;
    }

    public static Matrix4x4 operator * (Matrix4x4 m1, Matrix4x4 m2)
    {
        Matrix4x4 res = new Matrix4x4();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    res.v[i][j] += m1.v[i][k] * m2.v[k][j];
                }
            }
        }
        return res;
    }

    public static Vector3 operator * (Matrix4x4 m, Vector3 v)
    {
        Vector3 res = Vector3.zero;
        res.x = m.v[0][0] * v.x + m.v[0][1] * v.y + m.v[0][2] * v.z;
        res.y = m.v[1][0] * v.x + m.v[1][1] * v.y + m.v[1][2] * v.z;
        res.z = m.v[2][0] * v.x + m.v[2][1] * v.y + m.v[2][2] * v.z;
        return res;
    }

    public static Matrix4x4 operator * (Matrix4x4 m, float x)
    {
        Matrix4x4 res = new Matrix4x4();
        for(int i=0; i<4; i++)
        {
            for(int j=0;j<4; j++)
            {
                res.v[i][j] = m.v[i][j] * x;
            }
        }
        return res;
    }

    public static Matrix4x4 operator * (float x, Matrix4x4 m)
    {
        Matrix4x4 res = new Matrix4x4();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                res.v[i][j] = m.v[i][j] * x;
            }
        }
        return res;
    }

}
