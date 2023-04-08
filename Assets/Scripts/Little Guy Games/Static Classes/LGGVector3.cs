using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

/*============================================================
**
** Class:  LGGVector3
**
** Purpose: Static class for Vector3-specific functions.
**
**
** Author: Lîf Gwaethrakindo
**
==============================================================*/

namespace LittleGuyGames
{
public static class LGGVector3
{
    /// <summary>Cuadratic Beizer Function.</summary>
    /// <param name="a">Point A.</param>
    /// <param name="b">Point B.</param>
    /// <param name="c">Tangent point C.</param>
    /// <param name="t">Time t [not internally clamped so you can extrapolate the curve].</param>
    /// <returns>Cuadratic Beizer Curve.</returns>
    public static Vector3 CuadraticBeizer(Vector3 a, Vector3 b, Vector3 c, float t)
    {
        return Vector3.LerpUnclamped(Vector3.LerpUnclamped(a, c, t), Vector3.LerpUnclamped(c, b, t), t);
    }

    /// <returns>Sumation of all Vector's components combined.</returns>
    public static float Sum(this Vector3 v)
    {
        return v.x + v.y + v.z;
    }

    /// <returns>Absolute value of all Vector's components.</returns>
    public static Vector3 Abs(this Vector3 v)
    {
        return new Vector3(Mathf.Abs(v.x), Mathf.Abs(v.y), Mathf.Abs(v.z));
    }

    public static ValueTuple<float, Vector3>[] GetLateralSegments(Vector3 a, Vector3 b, float seed, float maxWidth, float minSegments = 2.0f, bool right = false)
    {
        seed = Mathf.Abs(seed);

        Vector3 d = b - a;
        float m = d.magnitude;
        float min = Mathf.Min(m, minSegments);
        float max = Mathf.Max(m * m, minSegments);
        float s = Mathf.Ceil(LGGMath.Rand(min, max, seed));
        ValueTuple<float, Vector3>[] tuples = new ValueTuple<float, Vector3>[(int)s];
        Quaternion r = Quaternion.LookRotation(d);
        Vector3 up = r * (right ? Vector3.right : Vector3.up);
        Vector3 l = Quaternion.Inverse(r) * d;
        Vector3 c = new Vector3(l.x, l.z);
        Vector3 k = Vector3.Cross(up, d).normalized;

        for(float i = 0.0f; i < s; i++)
        {
            float t = (i / s);
            float x = ((t * c.x * maxWidth) / m ) * maxWidth;
            float y = ((t * c.y * maxWidth) / m ) * maxWidth;
            float n = Mathf.PerlinNoise(x, y) * maxWidth;
            float o = LGGMath.Rand(-1.0f, 1.0f, t * seed);
            o = Mathf.Sign(o);

            tuples[(int)i] = new ValueTuple<float, Vector3>(t, k * n * o);
        }

        return tuples;
    }
}
}