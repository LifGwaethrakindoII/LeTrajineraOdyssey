using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*============================================================
**
** Class:  LGGMath
**
** Purpose: Math's Library.
**
**
** Author: Lîf Gwaethrakindo
**
==============================================================*/

namespace LittleGuyGames
{
public static class LGGMath
{
    public const float EPSILON = 0.001f;                            /// <summary>Custom-defined Epsilon.</summary>

    /// <summary>Press F to pay respects. This is just a default normalized property function in case you don't use easings.</summary>
    public static float F(float x) { return x; }

    /// <returns>Signed Angle [From Connor's GliderFC.cs].</returns>
    public static float SignedAngle(float a)
    {
        return a > 180.0f ? a - 360.0f : a;
    }

    /// <summary>Converts negative angle in degrees into positive representation.</summary>
    /// <param name="a">Angle to convert.</param>
    /// <returns>Negative angle [in degrees] to positive angle [in degrees too].</returns>
    public static float NegativeAngleToPositiveRepresentation(float a)
    {
        /// n(-90) = 270 ... n(a) = 360 - abs(a)
        a = Mathf.Abs(a) % 360.0f;
        return 360.0f - a;
    }

    /// <summary>Converts positive angle in degrees into negative representation.</summary>
    /// <param name="a">Angle to convert.</param>
    /// <returns>Positive angle [in degrees] to negative angle [in degrees too].</returns>
    public static float PositiveAngleToNegativeRepresentation(float a)
    {
        /// p(270) = -90 ... n(a) = abs(a) - 360
        a = Mathf.Abs(a) % 360.0f;
        return a - 360.0f;
    }

    /// <summary>Ease-Out normalized property function.</summary>
    /// <param name="t">Normalized time t.</param>
    public static float EaseOut(float t)
    {
        return Mathf.Sin((t * Mathf.PI) * 0.5f);
    }

    /// <summary>Ease-Out Bounce's function [from Easings.net].</summary>
    /// <param name="t">Normalized time t.</param>
    public static float EaseOutBounce(float t)
    {
        float n1 = 7.5625f;
        float d1 = 2.75f;

        if(t <= 0.0f || t >= 1.0f)
        {
            return t;

        } else if (t < 1.0f / d1)
        {
            return n1 * t * t;

        } else if (t < 2.0f / d1)
        {
            return n1 * (t -= 1.5f / d1) * t + 0.75f;

        } else if (t < 2.5f / d1)
        {
            return n1 * (t -= 2.25f / d1) * t + 0.9375f;

        } else
        {
            return n1 * (t -= 2.625f / d1) * t + 0.984375f;
        }
    }

    public static float EaseInOutSin(float t)
    {
        return -(Mathf.Cos(Mathf.PI * t) - 1.0f) * 0.5f;
    }

    /// <summary>Accelerates x value into y value.</summary>
    /// <param name="x">Value to accelerate.</param>
    /// <param name="y">Target value.</param>
    /// <param name="v">Velocity's reference.</param>
    /// <param name="a">Acceleration rate (x / s^2).</param>
    /// <param name="dt">Time's Delta. Passed as parameter if you want to do it on Physics' step, Main step or with a custom step</param>
    /// <param name="m">Speed Change's Mode. Acceleration by default.</param>
    /// <param name="e">Epsilon's tolerance.</param>
    public static float AccelerateTowards(float x, float y, ref float v, float a, float dt, SpeedChange m = SpeedChange.Acceleration, float e = EPSILON)
    {
        /*Where:
        - d = Delta or error margin between the target and current value
        - s = Sign of the desired direction
        - p = Projection of the current value with the new velocity value (for overlapping evaluation)*/

        float d = y - x;

        if(Mathf.Abs(d) <= e)
        {
            v = 0.0f;
            x = y;
            return x;
        }

        float s = Mathf.Sign(d);

        /*Reset the velocity if the direction is different than the velocity's.
        I do this reset in case the previous accumulated velocity is too much,
        which would make the velocity to take a while before pointing towards
        the same direction.*/
        if(Mathf.Sign(v) != s) v = 0.0f;

        switch(m)
        {
            // Accumulate velocity as the acceleration keeps happening (x / s^2)
            case SpeedChange.Acceleration:
                v += (s * a * dt * dt);
            break;

            // Apply linear acceleration (x / s)
            case SpeedChange.Linear:
                v = s * a * dt;
            break;
        }
        
        float p = x + v;

        /* If the projection would overlap the target value
        (being more or less than the target depending of the direction),
        add delta instead of the velocity (setting x equal to y is also valid)*/
        if(s == 1.0f ? p > y : p < y)
        {
            v = 0.0f;
            x += d;
        }
        else x += v;

        /* Maybe clamping shouldn't be necessary if the overlapping operations are correct.
        Paranoia from my part?*/
        return s == 1.0f ? Mathf.Min(x, y) : Mathf.Max(x, y);
    }

    /// <summary>Converts float number into boolean representation [it will be either 0.0f or 1.0f].</summary>
    /// <param name="x">Number to convert to boolean representation.</param>
    public static float ToNumericBoolean(float x)
    {
        return x != 0.0f ? 1.0f : 0.0f;
    }

    /// <summary>It does the same thing as Unity's PingPong, except that it takes a minimum value into account [instead of the default 0.0f].</summary>
    /// <param name="t">Time.</param>
    /// <param name="min">Min Value.</param>
    /// <param name="max">Max Value.</param>
    public static float PingPong(float t, float min, float max)
    {
        return min + Mathf.PingPong(t, max - min);
    }

    /// <summary>Calculates the radius of cross section given a height.</summary>
    /// <param name="r">Sphere's radius.</param>
    /// <param name="h">Height [distance between the center and the crossing plane].</param>
    /// <param name="t">Optional normalized time t [1.0f by default, which un-alters the height parameter].</param>
    public static float CalculateRadiusFromCrossSection(float r, float h, float t = 1.0f)
    {
        t = Mathf.Clamp(t, 0.0f, 1.0f);
        h = h * t;
        return Mathf.Sqrt((r * r) - (h * h));
    }

    /// <summary>Controlled Random function [you pass the seed].</summary>
    /// <param name="min">Min Random Value.</param>
    /// <param name="max">Max Random Value.</param>
    /// <param name="seed">Custom seed [it does not change with time].</param>
    public static int Rand(int min, int max, int seed)
    {
        return min + (seed % (max - min));
    }

    /// <summary>Controlled Random function [you pass the seed].</summary>
    /// <param name="min">Min Random Value.</param>
    /// <param name="max">Max Random Value.</param>
    /// <param name="seed">Custom seed [it does not change with time].</param>
    public static float Rand(float min, float max, float seed)
    {
        return min + (seed % (max - min));
    }

    /// <summary>Remaps given input from map into given range.</summary>
    /// <param name="x">Input value to remap.</param>
    /// <param name="mMin">Original values mapping's minimum value.</param>
    /// <param name="mMax">Original values mapping's maximum value.</param>
    /// <param name="rMin">Range's minimum value.</param>
    /// <param name="rMax">Range's maximum value.</param>
    /// <returns>Input mapped into given range.</returns>
    public static float RemapValue(float x, float mMin, float mMax, float rMin, float rMax)
    {
        return (((rMax - rMin) * (x - mMin)) / (mMax - mMin)) + rMin;
    }
}
}