using System.Collections;
using System;
using System.Text;
using System.Collections.Generic;
using UnityEngine;

namespace LittleGuyGames
{
[Serializable]
public class ForceScalar
{
    public ForceMode mode;
    public float acceleration;
    public float decceleration;
    public float minLimit;
    public float maxLimit;
    private float value;
    private float force;

    public static implicit operator float(ForceScalar _force) { return _force.value; }

    public float GetValue() { return value; }

    public static float AccelerateTowards(ref ForceScalar fs, float x, float dt)
    {
        float sign = Mathf.Sign(x - fs.value);
        fs.value += (fs.acceleration * dt * sign);
        Debug.Log("[ForceScalar] X: " + (fs.acceleration * dt * sign));
        //fs.value = sign == 1.0f ? Mathf.Min(fs.value, x) : Mathf.Max(fs.value, x);
        Debug.Log("[ForceScalar] value: " + fs.value);
        return fs.value;
    }

    public static float Accelerate(ref ForceScalar fs, float dt)
    {
        return AccelerateTowards(ref fs, fs.maxLimit, dt);
    }

    public static float DeccelerateTowards(ref ForceScalar fs, float x, float dt)
    {
        float sign = Mathf.Sign(x - fs.value);
        fs.value -= (fs.decceleration * dt * sign);
        fs.value = sign == 1.0f ? Mathf.Max(fs.value, x) : Mathf.Min(fs.value, x);
        return fs.value;
    }

    public static float Deccelerate(ref ForceScalar fs, float dt)
    {
        return DeccelerateTowards(ref fs, fs.minLimit, dt);
    }

    public static void Reset(ref ForceScalar fs)
    {
        fs.value = 0.0f;
        fs.force = 0.0f;
    }

    /// <returns>String representing this AirplaneAxes.</returns>
    public override string ToString()
    {
        StringBuilder builder = new StringBuilder();

        /*builder.Append("{ Roll: ");
        builder.Append(roll.ToString());
        builder.Append(", Yaw: ");
        builder.Append(yaw.ToString());
        builder.Append(", Pitch: ");
        builder.Append(pitch.ToString());
        builder.Append(" }");*/

        return builder.ToString();
    }
}
}