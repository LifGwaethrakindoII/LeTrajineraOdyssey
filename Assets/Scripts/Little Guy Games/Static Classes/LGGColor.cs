using System.Collections;
using System;
using System.Text;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

namespace LittleGuyGames
{
public static class LGGColor
{
	/// <summary>Moves Color towards other color.</summary>
	/// <param name="a">Current Color.</param>
	/// <param name="b">Target Color.</param>
	/// <param name="maxDistanceDelta">Maximum distance difference.</param>
	public static Color MoveTowards(Color a, Color b, float maxDelta)
	{
		// Calculate the distance between the current and target colors
        float distance = Vector4.Distance(a, b);

        // If the distance is less than or equal to the maxDelta, return the b color
        if (distance <= maxDelta)
        {
            return b;
        }

        // Otherwise, calculate the direction and new color
        Vector4 direction = b - a;
        direction.Normalize();
        Color d = direction;
        Color newColor = a + d * maxDelta;

        return newColor;
	}

	/// <summary>Returns color scaled by an intensity.</summary>
	/// <param name="c">Color reference.</param>
	/// <param name="i">Intensity.</param>
	public static Color WithIntensity(this Color c, float i)
	{
		float f = Mathf.Pow(2.0f, i);
		return c * f;
	}

	/// <returns>Color with setted alpha.</returns>
	public static Color WithAlpha(this Color c, float a)
	{
		c.a = a;
		return c;
	}
}
}