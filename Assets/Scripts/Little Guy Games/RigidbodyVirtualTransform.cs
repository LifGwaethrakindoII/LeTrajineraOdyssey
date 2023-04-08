using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace LittleGuyGames
{
public class RigidbodyVirtualTransform : VirtualTransform
{
    [SerializeField] private Rigidbody _rigidbody;                                          /// <summary>Optional Rigidbody Component.</summary>
    [SerializeField] private List<RigidbodyVirtualTransform> initialRigidbodyChildren;      /// <summary>Initial Children.</summary>
    private HashSet<RigidbodyVirtualTransform> _rigidbodyChildren;                          /// <summary>Rigidbody Children.</summary>
    private RigidbodyVirtualTransform _rigidbodyParent;                                     /// <summary>Virtual Parent.</summary>

    /// <summary>Gets and Sets rigidbody property.</summary>
    public Rigidbody rigidbody
    {
        get { return _rigidbody; }
        set { _rigidbody = value; }
    }

    /// <summary>Gets and Sets rigidbodyChildren property.</summary>
    public HashSet<RigidbodyVirtualTransform> rigidbodyChildren
    {
        get { return _rigidbodyChildren; }
        protected set { _rigidbodyChildren = value; }
    }

    /// <summary>Gets and Sets rigidbodyParent property.</summary>
    public RigidbodyVirtualTransform rigidbodyParent
    {
        get { return _rigidbodyParent; }
        set { _rigidbodyParent = value; }
    }

    /// <summary>RigidbodyVirtualTransform's instance initialization when loaded [Before scene loads].</summary>
    protected override void Awake()
    {
        base.Awake();

        rigidbodyChildren = new HashSet<RigidbodyVirtualTransform>();

        if(initialRigidbodyChildren != null) foreach(RigidbodyVirtualTransform child in initialRigidbodyChildren)
        {
            if(child != null)
            child.SetRigidbodyParent(this);
        }
    }

    /// <summary>Updates RigidbodyVirtualTransform's instance at the end of each frame.</summary>
    protected override void LateUpdate()
    {
        
    }

    /// <summary>Updates RigidbodyVirtualTransform's instance at each Physics Thread's frame.</summary>
    private void FixedUpdate()
    {
        if(rigidbody == null) return;

        Vector3 parentPosition = rigidbody.position;
        Quaternion parentRotation = rigidbody.rotation;

        if(transformChildren != null) foreach(VirtualTransform child in transformChildren)
        {
            if(child == null) continue;

            child.transform.position = (parentPosition + (parentRotation * child.localPosition));
            child.transform.rotation = (parentRotation * child.localRotation);
        }

        if(rigidbodyChildren != null) foreach(RigidbodyVirtualTransform child in rigidbodyChildren)
        { 
            Rigidbody body = child.rigidbody;

            if(body == null) continue;

            Vector3 position = body.position;
            Quaternion rotation = body.rotation;

            body.MovePosition(parentPosition + (parentRotation * child.localPosition));
            body.MoveRotation(parentRotation * child.localRotation);
        }
    }

    /// <summary>Sets Virtual Rigidbody Parent.</summary>
    /// <param name="_virtualTransform">New Parent.</param>
    public void SetRigidbodyParent(RigidbodyVirtualTransform _virtualTransform)
    {
        if(_virtualTransform == null) return;

        Rigidbody parentRigidbody = _virtualTransform.rigidbody;

        if(rigidbodyParent != null && _virtualTransform != rigidbodyParent) Unparent();

        if(rigidbody == null || parentRigidbody == null) return;

        Vector3 delta = rigidbody.position - _virtualTransform.rigidbody.position;

        localPosition = Quaternion.Inverse(parentRigidbody.rotation) * delta;
        localRotation = rigidbody.rotation * Quaternion.Inverse(parentRigidbody.rotation);

        _virtualTransform.rigidbodyChildren.Add(this);
        rigidbodyParent = _virtualTransform;
    }

    /// <summary>Removes Virtual Parent reference.</summary>
    public void RigidbodyUnparent()
    {
        rigidbodyParent.rigidbodyChildren.Remove(this);
        rigidbodyParent = null;
    }
}
}