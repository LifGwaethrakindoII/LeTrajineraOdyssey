using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LittleGuyGames.XR
{
public class XRCanvasReorienter : MonoBehaviour
{
    [SerializeField] private Transform _XRCharacterTransform;
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private Vector3 _up;
    [SerializeField] private float _distance;
    [SerializeField] private float _yOffset;
    [SerializeField][Range(0.0f, 1.0f)] private float _dotThreshold;
    [SerializeField] private float _interpolationDuration;

    /// <summary>Gets and Sets XRCharacterTransform property.</summary>
    public Transform XRCharacterTransform
    {
        get { return _XRCharacterTransform; }
        set { _XRCharacterTransform = value; }
    }

    /// <summary>Gets and Sets cameraTransform property.</summary>
    public Transform cameraTransform
    {
        get { return _cameraTransform; }
        set { _cameraTransform = value; }
    }

    /// <summary>Gets and Sets up property.</summary>
    public Vector3 up
    {
        get { return _up; }
        set { _up = value; }
    }

    /// <summary>Gets and Sets distance property.</summary>
    public float distance
    {
        get { return _distance; }
        set { _distance = value; }
    }

    /// <summary>Gets and Sets yOffset property.</summary>
    public float yOffset
    {
        get { return _yOffset; }
        set { _yOffset = value; }
    }

    /// <summary>Gets and Sets dotThreshold property.</summary>
    public float dotThreshold
    {
        get { return _dotThreshold; }
        set { _dotThreshold = Mathf.Clamp01(value); }
    }

    /// <summary>Gets and Sets interpolationDuration property.</summary>
    public float interpolationDuration
    {
        get { return _interpolationDuration; }
        set { _interpolationDuration = value; }
    }

    /// <summary>Draws Gizmos on Editor mode when XRCanvasReorienter's instance is selected.</summary>
    private void OnDrawGizmosSelected()
    {
        if(XRCharacterTransform == null) return;

        Vector3 a = XRCharacterTransform.position;
        Vector3 b = a + XRCharacterTransform.forward * distance;

        Gizmos.DrawLine(a, b);
        Gizmos.DrawRay(b, up * yOffset);
    }

    /// <summary>Resets XRCanvasReorienter's instance to its default values.</summary>
    private void Reset()
    {
        up = Vector3.up;
        distance = 15.0f;
        dotThreshold = 0.15f;
        interpolationDuration = 2.5f;
    }

    /// <summary>Updates XRCanvasReorienter's instance at the end of each frame.</summary>
    private void LateUpdate()
    {
        if(cameraTransform == null || XRCharacterTransform == null) return;

        float dot = Vector3.Dot(cameraTransform.forward, transform.forward);

        if(dot >= dotThreshold) return;

        Vector3 projection = Vector3.ProjectOnPlane(cameraTransform.forward, up);
        Vector3 position =  XRCharacterTransform.position + (projection * distance) + (up * yOffset);
        float t = interpolationDuration * Time.deltaTime;

        transform.position = Vector3.Slerp(transform.position, position, t);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(projection, up), t);
    }

    /// <summary>Sets Camera's Transform.</summary>
    /// <param name="_cameraTransform">Transform's Reference.</param>
    public void Set(Transform _XRCharacterTransform, Transform _cameraTransform)
    {
        XRCharacterTransform = _XRCharacterTransform;
        cameraTransform = _cameraTransform;
    }
}
}