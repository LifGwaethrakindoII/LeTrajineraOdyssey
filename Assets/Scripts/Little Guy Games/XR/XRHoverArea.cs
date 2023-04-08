using System.Collections;
using System.Text;
using System.Collections.Generic;
using UnityEngine;

/*============================================================
**
** Class:  XRHoverArea
**
** Purpose: Interactable that acts as a hover area. Internally
** functions very similar to the XRButton, but with less
** additional functionality (such as animations).
**
**
** Author: Lîf Gwaethrakindo
**
==============================================================*/

namespace LittleGuyGames.XR
{
/// <summary>Event invoked when an XRHoverArea event occurs.</summary>
/// <param name="_hoverArea">Callback sender.</param>
/// <param name="ID">Event's ID.</param>
public delegate void OnXRHoverAreaEvent(XRHoverArea _hoverArea, int ID);

public class XRHoverArea : MonoBehaviour
{
    public const int ID_EVENT_HOVERAREA_INTERACTABLE_ENTER = 1 << 0;
    public const int ID_EVENT_HOVERAREA_INTERACTABLE_EXIT = 1 << 1;

    public event OnXRHoverAreaEvent onXRHoverAreaEvent;

    [SerializeField] private PhysicsOverlapSensor _overlapSensor;
    [Space(5f)]
    [SerializeField] private Vector3 _startPoint;
    [SerializeField] private Vector3 _endPoint;
    private Transform _interactor;
    private float _progress;
    private bool _activated;

    /// <summary>Gets and Sets overlapSensor property.</summary>
    public PhysicsOverlapSensor overlapSensor
    {
        get { return _overlapSensor; }
        set { _overlapSensor = value; }
    }

    /// <summary>Gets startPoint property.</summary>
    public Vector3 startPoint { get { return _startPoint; } }

    /// <summary>Gets endPoint property.</summary>
    public Vector3 endPoint { get { return _endPoint; } }

    /// <summary>Gets plane property.</summary>
    public Vector3 plane { get { return transform.TransformPoint(endPoint) - transform.TransformPoint(startPoint); } }

    /// <summary>Gets and Sets interactor property.</summary>
    public Transform interactor
    {
        get { return _interactor; }
        private set { _interactor = value; }
    }

    /// <summary>Gets and Sets progress property.</summary>
    public float progress
    {
        get { return _progress; }
        private set { _progress = value; }
    }

    /// <summary>Gets and Sets activated property.</summary>
    public bool activated
    {
        get { return _activated; }
        set { _activated = value; }
    }

    /// <summary>Draws Gizmos on Editor mode.</summary>
    protected virtual void OnDrawGizmosSelected()
    {
        Vector3 a = transform.TransformPoint(startPoint);
        Vector3 b = transform.TransformPoint(endPoint);
        float r = 0.01f;
        Color color = Color.white;
        color.a = 0.75f;

        Gizmos.color = color;

        Gizmos.DrawSphere(a, r);
        Gizmos.DrawSphere(b, r);
        Gizmos.DrawLine(a, b);
    }

    /// <summary>XRHoverArea's instance initialization when loaded [Before scene loads].</summary>
    private void Awake()
    {
        overlapSensor.onTriggerEvent += OnTriggerEvent;
        overlapSensor.invokeEvents = ColliderEvents.Enter | ColliderEvents.Exit;
        progress = 0.0f;
    }

    /// <summary>Updates XRHoverArea's instance at the end of each frame.</summary>
    protected virtual void LateUpdate()
    {
        if(interactor == null)
        {
            progress = 0.0f;
            return;
        }

        Vector3 a = transform.TransformPoint(startPoint);
        Vector3 b = transform.TransformPoint(endPoint);
        //Vector3 plane = b - a;
        Vector3 position = interactor.position;
        Vector3 direction = position - a;
        Vector3 projectionPlane = Vector3.ProjectOnPlane(direction, plane);
        Vector3 projection = Vector3.ProjectOnPlane(direction, projectionPlane);

        progress = Vector3.Dot(plane, projection) < 0.0f ? 0.0f : projection.magnitude / plane.magnitude;
        progress = Mathf.Clamp(progress, 0.0f, 1.0f);
    }

    /// <summary>Event invoked when a trigger occurs.</summary>
    /// <param name="_event">Type of Collider Event.</param>
    /// <param name="_invoker">Invoker's reference.</param>
    /// <param name="_collider">Other Collider involved in the event.</param>
    private void OnTriggerEvent(ColliderEvents _event, PhysicsOverlapSensor _invoker, Collider _collider)
    {
        Transform interactorTransform = _collider.transform;

        switch(_event)
        {
            case ColliderEvents.Enter:
                if(interactor != null) return;

                interactor = interactorTransform;
                activated = true;
                InvokeXRHoverAreaEvent(ID_EVENT_HOVERAREA_INTERACTABLE_ENTER);
            break;

            case ColliderEvents.Exit:
                if(interactorTransform != interactor)
                {
                    return;

                } else if(interactorTransform == interactor)
                {
                    interactor = null;
                    activated = false;
                    InvokeXRHoverAreaEvent(ID_EVENT_HOVERAREA_INTERACTABLE_EXIT);
                }
            break;
        }
    }

    /// <summary>Invokes Event.</summary>
    /// <param name="ID">Event's ID.</param>
    protected void InvokeXRHoverAreaEvent(int ID)
    {
        if(onXRHoverAreaEvent != null) onXRHoverAreaEvent(this, ID);
    }

    /// <returns>String representing this XRHoverArea.</returns>
    public override string ToString()
    {
        StringBuilder  builder = new StringBuilder();

        builder.Append("Progress: ");
        builder.Append(progress.ToString());

        return builder.ToString();
    }
}
}