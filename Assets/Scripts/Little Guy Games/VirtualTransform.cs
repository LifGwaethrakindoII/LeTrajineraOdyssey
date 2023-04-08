using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace LittleGuyGames
{
public class VirtualTransform : MonoBehaviour, IComparable
{
    [SerializeField] private List<VirtualTransform> initialChildren;        /// <summary>Initial Children.</summary>
    private HashSet<VirtualTransform> _transformChildren;                   /// <summary>Transform Children.</summary>
    private VirtualTransform _parent;                                       /// <summary>Virtual Parent.</summary>
    private Vector3 _localPosition;                                         /// <summary>Local Position.</summary>
    private Quaternion _localRotation;                                      /// <summary>Local Rotation.</summary>

    /// <summary>Gets and Sets transformChildren property.</summary>
    public HashSet<VirtualTransform> transformChildren
    {
        get { return _transformChildren; }
        protected set { _transformChildren = value; }
    }

    /// <summary>Gets and Sets parent property.</summary>
    public VirtualTransform parent
    {
        get { return _parent; }
        set { _parent = value; }
    }

    /// <summary>Gets and Sets localPosition property.</summary>
    public Vector3 localPosition
    {
        get { return _localPosition; }
        set { _localPosition = value; }
    }

    /// <summary>Gets and Sets localRotation property.</summary>
    public Quaternion localRotation
    {
        get { return _localRotation; }
        set { _localRotation = value; }
    }

    /// <summary>VirtualTransform's instance initialization when loaded [Before scene loads].</summary>
    protected virtual void Awake()
    {
        transformChildren = new HashSet<VirtualTransform>();

        if(initialChildren != null) foreach(VirtualTransform child in initialChildren)
        {
            if(child != null)
            child.SetParent(this);
        }
    }

    /// <summary>Updates VirtualTransform's instance at each frame.</summary>
    protected virtual void LateUpdate()
    {
        if(transformChildren == null) return;

        Vector3 parentPosition = transform.position;
        Quaternion parentRotation = transform.rotation;

        foreach(VirtualTransform child in transformChildren)
        {
            if(child == null) continue;

            child.transform.position = (parentPosition + (parentRotation * child.localPosition));
            child.transform.rotation = (parentRotation * child.localRotation);
        }
    }

    /// <summary>Updates VirtualTransform's instance at each Physics Thread's frame.</summary>
    private void FixedUpdate()
    {
        if(transformChildren == null) return;

        Vector3 parentPosition = transform.position;
        Quaternion parentRotation = transform.rotation;

        foreach(VirtualTransform child in transformChildren)
        {
            if(child == null) continue;

            child.transform.position = (parentPosition + (parentRotation * child.localPosition));
            child.transform.rotation = (parentRotation * child.localRotation);
        }
    }

    /// <summary>Sets Virtual Parent.</summary>
    /// <param name="_virtualTransform">New Parent.</param>
    public void SetParent(VirtualTransform _virtualTransform)
    {
        if(_virtualTransform == null || _virtualTransform == this) return;

        transform.parent = null;
        if(parent != null && _virtualTransform != parent) Unparent();

        Vector3 delta = transform.position - _virtualTransform.transform.position;
        Quaternion inverseRotation = Quaternion.Inverse(_virtualTransform.transform.rotation);

        localPosition = inverseRotation * delta;
        localRotation = inverseRotation * transform.rotation/* * inverseRotation*/;

        _virtualTransform.transformChildren.Add(this);
        parent = _virtualTransform;
    }

    public Vector3 GetLocalPosition(Transform _transform)
    {
        Vector3 d = transform.position - _transform.position;
        Quaternion i = Quaternion.Inverse(_transform.rotation);

        return i * d;
    }

    public void SetLocalPosition(Transform _parent, Vector3 _localPosition)
    {
        transform.position = _parent.position + (_parent.rotation * _localPosition);
    }

    public Quaternion GetLocalRotation(Transform _transform)
    {
        Quaternion i = Quaternion.Inverse(_transform.rotation);

        return i * transform.rotation;
    }

    public void SetLocalRotation(Transform _parent, Quaternion _localRotation)
    {
        transform.rotation = _parent.rotation = _localRotation;
    }

    /// <summary>Removes Virtual Parent reference.</summary>
    public void Unparent()
    {
        parent.transformChildren.Remove(this);
        transform.parent = null;
        parent = null;
    }

    /// <summary>Determines whether the specified object is equal to the current object.</summary>
    /// <param name="_object">Object to compare against.</param>
    public override bool Equals (object _object)
    {
        return CompareTo(_object) == 0;
    }

    /// <returns>Hash Code [for HashSet, Dictionary, etc.].</returns>
    public override int GetHashCode()
    {
        return GetInstanceID().GetHashCode();
    }

    /// <summary>Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.</summary>
    /// <param name="_object">Object to compare againsts.</param>
    public int CompareTo(object _object)
    {
        int id = GetInstanceID();
        UnityEngine.Object obj = _object as UnityEngine.Object;
        if(obj == null) return -1;
        int x = obj.GetInstanceID();

        if(id == x)
        {
            return 0;
        } else if(id > x)
        {
            return 1;
        } else if(id < x)
        {
            return -1;
        }

        return -1;
    }
}
}