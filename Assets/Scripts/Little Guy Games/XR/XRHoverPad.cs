using System.Collections;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

/*============================================================
**
** Class:  XRHoverPad
**
** Purpose: HoverArea Interactable that acts as some sort of
** "invisible" joystick. It calculates a 3D axes, where the 
** progress indicates the z-component and the position of the
** interactor along a radius the remaining x and y axes.
**
** NOTE: Do not scale the GameObject containing this
** component, keep it at { 1.0f, 1.0f, 1.0f }.
**
** Author: Lîf Gwaethrakindo
**
==============================================================*/

namespace LittleGuyGames.XR
{
public class XRHoverPad : XRHoverArea
{
	[Space(5f)]
	[Header("Hover Pad's Attributes:")]
	[SerializeField] private float _radius;
	[SerializeField] private Vector3 axes;

	/// <summary>Gets radius property.</summary>
	public float radius { get { return _radius; } }

	/// <summary>Draws Gizmos on Editor mode when XRHoverPad's instance is selected.</summary>
	protected override void OnDrawGizmosSelected()
	{
		base.OnDrawGizmosSelected();

#if UNITY_EDITOR
		Vector3 a = transform.TransformPoint(startPoint);
		Vector3 b = transform.TransformPoint(endPoint);
		Color color = Color.red;
		color.a = 0.5f;

		Handles.color = color;
		Handles.DrawSolidDisc(a, plane, radius);
		Handles.DrawSolidDisc(b, plane, radius);

		Gizmos.color = Color.blue;
		Gizmos.DrawRay(transform.position, transform.forward * 5.0f);
#endif
	}

	/// <summary>Updates XRHoverPad's instance at the end of each frame.</summary>
	protected override void LateUpdate()
	{
		base.LateUpdate();

		if(interactor == null)
		{
			axes = Vector3.zero;
			return;
		}

		Vector3 inversePoint = transform.InverseTransformPoint(interactor.position);
		float inverseRadius = 1.0f / radius;
		axes = new Vector3(
			Mathf.Clamp(inversePoint.x * inverseRadius, -1.0f, 1.0f),
			Mathf.Clamp(inversePoint.z * inverseRadius, -1.0f, 1.0f),
			progress
		);
	}

	public Vector3 GetAxes()
	{
		return axes;
	}

	/// <returns>String representing this XRHoverArea.</returns>
    public override string ToString()
    {
        StringBuilder  builder = new StringBuilder();

        builder.Append(base.ToString());
        builder.Append(", Axes: ");
        builder.Append(axes.ToString());

        return builder.ToString();
    }
}
}