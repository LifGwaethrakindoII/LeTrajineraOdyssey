using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*============================================================
**
** Class:  LGGGizmos
**
** Purpose: Static class that contains Gizmos-specific
** methods.
**
**
** Author: Lîf Gwaethrakindo
**
==============================================================*/

namespace LittleGuyGames
{
public static class LGGGizmos
{
    /// <summary>Rotates Gizmos' Matrix.</summary>
    /// <param name="_rotation">New Rotation.</param>
    /// <param name="_priorMatrix">Previous Matrix's reference.</param>
    public static void RotateGizmos(Quaternion _rotation, out Matrix4x4 _priorMatrix)
    {
#if UNITY_EDITOR
        Matrix4x4 gizmoTransform = Matrix4x4.TRS(Vector3.zero, _rotation, Vector3.one);
        _priorMatrix = Gizmos.matrix;
        Gizmos.matrix *= gizmoTransform;
#else
        _priorMatrix = default(Matrix4x4);
#endif      
    }

    /// <summary>Draws a Cuadratic Beizer Curve Line between an initial point, a tangent and a final point on the scene.</summary>
    /// <param name="a">Curve's initial point [P0].</param>
    /// <param name="b">Curve's final point [Pf].</param>
    /// <param name="c">Curve's tangent [P1].</param>
    /// <param name="s">Time split. The bigger the time split, the smoother the curve will look.</param>
    public static void DrawCuadraticBeizerCurve(Vector3 a, Vector3 b, Vector3 c, int s = 50)
    {
#if UNITY_EDITOR
        float timeSplitInverseMultiplicative = (1f / (1f * s));

        Color colorA = Gizmos.color;
        Color colorB = colorA;

        for(int i = 0; i < s -1; i++)
        {
            Vector3 initialPoint = LGGVector3.CuadraticBeizer(a, b, c, (i * timeSplitInverseMultiplicative));
            Vector3 finalPoint = LGGVector3.CuadraticBeizer(a, b, c, ((i + 1) * timeSplitInverseMultiplicative));
            Gizmos.DrawLine(initialPoint, finalPoint);
        }

        colorB.a *= 0.5f;
        Gizmos.color = colorB;
        Gizmos.DrawLine(a, c);
        Gizmos.DrawLine(c, b);
        Gizmos.color = colorA;
#endif
    }

    /// <summary>Draws Capsule.</summary>
    /// <param name="_origin">Capsule's Origin.</param>
    /// <param name="_direction">Capsule's Direction.</param>
    /// <param name="_rotation">Capsule's Orientation represented by a quaternion.</param>
    /// <param name="_radius">Capsule's Radius.</param>
    /// <param name="_length">Capsule's Length.</param>
    public static void DrawWireCapsule(Vector3 _origin, Vector3 _direction, Quaternion _rotation,  float _radius, float _length)
    {
#if UNITY_EDITOR
        float length = (_radius * 2.0f) < _length ? (_length - (_radius * 2.0f)) : 0.0f;
        Vector3 normal = _direction.normalized;
        Vector3 rightNormal = Vector3.right * _radius;
        Vector3 upNormal = Vector3.up * _radius;
        Vector3 forwardNormal = Vector3.forward * _radius;
        Vector3 projectedPoint = _origin + (normal * length);
        Matrix4x4 priorMatrix;
        RotateGizmos(_rotation, out priorMatrix);

        Gizmos.DrawWireSphere(_origin, _radius);
        Gizmos.DrawWireSphere(projectedPoint, _radius);
        Gizmos.DrawLine(_origin + rightNormal, projectedPoint + rightNormal);
        Gizmos.DrawLine(_origin - rightNormal, projectedPoint - rightNormal);
        Gizmos.DrawLine(_origin + upNormal, projectedPoint + upNormal);
        Gizmos.DrawLine(_origin - upNormal, projectedPoint - upNormal);
        Gizmos.DrawLine(_origin + forwardNormal, projectedPoint + forwardNormal);
        Gizmos.DrawLine(_origin - forwardNormal, projectedPoint - forwardNormal);

        Gizmos.matrix = priorMatrix;
#endif
    }

    /// <summary>Draws local normals (normals relative to the space of the provided quaternion).</summary>
    /// <param name="p">Where to draw the normals.</param>
    /// <param name="r">Local space quaternion.</param>
    public static void DrawLocalNormals(Vector3 p, Quaternion r, float l = 1.5f)
    {
#if UNITY_EDITOR
        Gizmos.color = Color.red;
        Gizmos.DrawRay(p, r * Vector3.right * l);
        Gizmos.color = Color.green;
        Gizmos.DrawRay(p, r * Vector3.up * l);
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(p, r * Vector3.forward * l);
#endif
    }

    /// <summary>Draws a Rotated Wired Box.</summary>
    /// <param name="_origin">Box's Origin.</param>
    /// <param name="_size">Box's Dimensions.</param>
    /// <param name="_rotation">Box's Rotation.</param>
    public static void DrawWireBox(Vector3 _origin, Vector3 _size, Quaternion _rotation)
    {
#if UNITY_EDITOR
        Vector3 halfExtents = _size * 0.5f;
        Vector3 leftUpForwardVertex = _origin + (_rotation * Vector3.Scale((Vector3.left + Vector3.up + Vector3.forward).normalized, halfExtents));
        Vector3 leftUpBackVertex = _origin + (_rotation * Vector3.Scale((Vector3.left + Vector3.up + Vector3.back).normalized, halfExtents));
        Vector3 leftDownForwardVertex = _origin + (_rotation * Vector3.Scale((Vector3.left + Vector3.down + Vector3.forward).normalized, halfExtents));
        Vector3 leftDownBackVertex = _origin + (_rotation * Vector3.Scale((Vector3.left + Vector3.down + Vector3.back).normalized, halfExtents));
        Vector3 rightUpForwardVertex = _origin + (_rotation * Vector3.Scale((Vector3.right + Vector3.up + Vector3.forward).normalized, halfExtents));
        Vector3 rightUpBackVertex = _origin + (_rotation * Vector3.Scale((Vector3.right + Vector3.up + Vector3.back).normalized, halfExtents));
        Vector3 rightDownForwardVertex = _origin + (_rotation * Vector3.Scale((Vector3.right + Vector3.down + Vector3.forward).normalized, halfExtents));
        Vector3 rightDownBackVertex = _origin + (_rotation * Vector3.Scale((Vector3.right + Vector3.down + Vector3.back).normalized, halfExtents));

        Gizmos.DrawLine(leftUpForwardVertex, leftUpBackVertex);
        Gizmos.DrawLine(leftUpForwardVertex, leftDownForwardVertex);
        Gizmos.DrawLine(leftUpForwardVertex, rightUpForwardVertex);
        Gizmos.DrawLine(leftUpBackVertex, leftDownBackVertex);
        Gizmos.DrawLine(leftUpBackVertex, rightUpBackVertex);
        Gizmos.DrawLine(rightUpBackVertex, rightUpForwardVertex);
        Gizmos.DrawLine(rightUpBackVertex, rightDownBackVertex);
        Gizmos.DrawLine(rightDownBackVertex, rightDownForwardVertex);
        Gizmos.DrawLine(rightDownBackVertex, leftDownBackVertex);
        Gizmos.DrawLine(rightDownForwardVertex, leftDownForwardVertex);
        Gizmos.DrawLine(leftDownForwardVertex, leftDownBackVertex);
        Gizmos.DrawLine(rightDownForwardVertex, rightUpForwardVertex);
#endif
    }
}
}