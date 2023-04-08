using System.Collections;
using System;
using System.Text;
using System.Collections.Generic;
using UnityEngine;

/*============================================================
**
** Struct:  AirplaneAxes
**
** Purpose: A simple 3-Dimensional Vector designed for
** describing Airplane's Axes (in a nutshell: just semantical commodity).
**
**
** Author: Lîf Gwaethrakindo
**
==============================================================*/

namespace LittleGuyGames
{
[Serializable]
public struct AirplaneAxes
{
    public float roll;  /// <summary>Roll's Value [X].</summary>
    public float yaw;   /// <summary>Yaw's Value [Y].</summary>
    public float pitch; /// <summary>Pitch's Value [Z].</summary>

    /// <summary>Vector2 to AirplaneAxes.</summary>
    public static implicit operator AirplaneAxes(Vector2 axes) { return new AirplaneAxes(axes.y, 0.0f, axes.x); }

    /// <summary>AirplaneAxes to Vector2.</summary>
    public static implicit operator Vector2(AirplaneAxes axes) { return new Vector2(axes.roll, axes.pitch); }

    /// <summary>AirplaneAxes to Vector3.</summary>
    public static implicit operator Vector3(AirplaneAxes axes) { return new Vector3(axes.pitch, axes.yaw, axes.roll); }

    /// <summary>Airplane times float operator.</summary>
    public static AirplaneAxes operator * (AirplaneAxes axes, float x)
    {
        axes.roll *= x;
        axes.yaw *= x;
        axes.pitch *= x;

        return axes; 
    }

    /// <summary>Airplane divided by float operator.</summary>
    public static AirplaneAxes operator / (AirplaneAxes axes, float x)
    {
        axes.roll /= x;
        axes.yaw /= x;
        axes.pitch /= x;

        return axes; 
    }

    /// <returns>Normalized version of this axes.</returns>
    public AirplaneAxes normalized
    {
        get
        {
            AirplaneAxes copy = this;
            return copy.Normalize();
        }
    }

    /// <summary>AirplaneAxes' Constructor.</summary>
    /// <param name="_roll">Roll Value.</param>
    /// <param name="_yaw">Yaw Value.</param>
    /// <param name="_pitch">Pitch Value.</param>
    public AirplaneAxes(float _roll, float _yaw, float _pitch)
    {
        roll = _roll;
        yaw = _yaw;
        pitch = _pitch;
    }

    /// <returns>Axes' Square Magnitude.</returns>
    public float SquareMagnitude()
    {
        return (roll * roll) + (yaw * yaw) + (pitch * pitch);
    }

    /// <returns>Axes' Magnitude.</returns>
    public float Magnitude()
    {
        return Mathf.Sqrt(SquareMagnitude());
    }

    /// <returns>Normalized Axes.</returns>
    public AirplaneAxes Normalize()
    {
        return this / Magnitude();
    }

    /// <returns>String representing this AirplaneAxes.</returns>
    public override string ToString()
    {
        StringBuilder builder = new StringBuilder();

        builder.Append("{ Roll: ");
        builder.Append(roll.ToString());
        builder.Append(", Yaw: ");
        builder.Append(yaw.ToString());
        builder.Append(", Pitch: ");
        builder.Append(pitch.ToString());
        builder.Append(" }");

        return builder.ToString();
    }
}
}