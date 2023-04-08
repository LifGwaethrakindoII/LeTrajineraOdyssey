using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*============================================================
**
** Class:  PhysicsOverlapSensor
**
** Purpose: This class generates trigger callbacks through
** the use of Physics' Overlap functions (Box, Sphere,
** Capsule...). Useful if you want to avoid either of a pair
** of objects to implement a Rigidbody. 
**
**
** Author: Lîf Gwaethrakindo
**
==============================================================*/

namespace LittleGuyGames
{
public enum UpdateType
{
    Normal,
    Fixed,
    Late
}

public enum PhysicsOverlapMode
{
    Cube,
    Sphere,
    Capsule
}

/// <summary>Event invoked when a trigger occurs.</summary>
/// <param name="_event">Type of Collider Event.</param>
/// <param name="_invoker">Invoker's reference.</param>
/// <param name="_collider">Other Collider involved in the event.</param>
public delegate void OnPhysicsOverlapTriggerEvent(ColliderEvents _event, PhysicsOverlapSensor _invoker, Collider _collider);

public class PhysicsOverlapSensor : MonoBehaviour
{
    public event OnPhysicsOverlapTriggerEvent onTriggerEvent;

    [SerializeField] private UpdateType _updateType;
    [SerializeField] private ColliderEvents _invokeEvents;
    [SerializeField] private PhysicsOverlapMode _mode;
    [SerializeField] private LayerMask _mask;
    [Space(5f)]
    [Header("Cube's Attributes:")]
    [SerializeField] private Vector3 _size;
    [Space(5f)]
    [Header("Sphere's Attributes:")]
    [SerializeField] private float _radius;
    [Space(5f)]
    [Header("Capsule's Attributes:")]
    [SerializeField] private float _length;
    private Dictionary<int, Collider> colliders;
    private HashSet<int> collidersMap;
    private HashSet<int> collidersToRemove;

    /// <summary>Gets and Sets updateType property.</summary>
    public UpdateType updateType
    {
        get { return _updateType; }
        set { _updateType = value; }
    }

    /// <summary>Gets and Sets invokeEvents property.</summary>
    public ColliderEvents invokeEvents
    {
        get { return _invokeEvents; }
        set { _invokeEvents = value; }
    }

    /// <summary>Gets and Sets mode property.</summary>
    public PhysicsOverlapMode mode
    {
        get { return _mode; }
        set { _mode = value; }
    }

    /// <summary>Gets and Sets mask property.</summary>
    public LayerMask mask
    {
        get { return _mask; }
        set { _mask = value; }
    }

    /// <summary>Gets and Sets size property.</summary>
    public Vector3 size
    {
        get { return _size; }
        set { _size = value; }
    }

    /// <summary>Gets and Sets radius property.</summary>
    public float radius
    {
        get { return _radius; }
        set { _radius = value; }
    }

    /// <summary>Gets and Sets length property.</summary>
    public float length
    {
        get { return _length; }
        set { _length = value; }
    }

    /// <summary>Draws Gizmos on Editor mode when PhysicsOverlapSensor's instance is selected.</summary>
    private void OnDrawGizmosSelected()
    {
        Color color = Color.green;
        color.a = 0.35f;

        Gizmos.color = color;
        Vector3 p = transform.position;
        Quaternion r = transform.rotation;

        switch(mode)
        {
            case PhysicsOverlapMode.Cube:
                LGGGizmos.DrawWireBox(p, size, r);
            break;

            case PhysicsOverlapMode.Sphere:
                Gizmos.DrawWireSphere(p, radius);
            break;

            case PhysicsOverlapMode.Capsule:
                LGGGizmos.DrawWireCapsule(p, transform.forward, Quaternion.identity, radius, length);
            break;
        }
    }

    /// <summary>Resets PhysicsOverlapSensor's instance to its default values.</summary>
    private void Reset()
    {
        invokeEvents = ColliderEvents.All;
        mask = Physics.AllLayers;
    }

    /// <summary>XRNonPhysicalDirectInteractor's instance initialization when loaded [Before scene loads].</summary>
    private void Awake()
    {
        colliders = new Dictionary<int, Collider>();
        collidersMap = new HashSet<int>();
        collidersToRemove = new HashSet<int>();
    }

    /// <summary>Updates PhysicsOverlapSensor's instance at each frame.</summary>
    private void Update()
    {
        if(updateType == UpdateType.Normal) UpdateSensor();
    }

    /// <summary>Updates PhysicsOverlapSensor's instance at the end of each frame.</summary>
    private void LateUpdate()
    {
        if(updateType == UpdateType.Late) UpdateSensor();
    }

    /// <summary>Updates PhysicsOverlapSensor's instance at each Physics Thread's frame.</summary>
    private void FixedUpdate()
    {
        if(updateType == UpdateType.Fixed) UpdateSensor();
    }

    private void UpdateSensor()
    {
        if(invokeEvents == ColliderEvents.None) return;

        Collider[] overlappedColliders = null;
        Vector3 p = transform.position;
        Quaternion r = transform.rotation;

        switch(mode)
        {
            case PhysicsOverlapMode.Cube:
                overlappedColliders = Physics.OverlapBox(p, size * 0.5f, r, mask);
            break;

            case PhysicsOverlapMode.Sphere:
                overlappedColliders = Physics.OverlapSphere(p, radius, mask);
            break;

            case PhysicsOverlapMode.Capsule:
                overlappedColliders = Physics.OverlapCapsule(p, p + (transform.forward * length), radius, mask);
            break;
        }

        //if(overlappedColliders == null || overlappedColliders.Length == 0) return;

        collidersMap.Clear();
        collidersToRemove.Clear();
        int instanceID = 0;

        foreach(Collider collider in overlappedColliders)
        {
            instanceID = collider.GetInstanceID();
            collidersMap.Add(instanceID);

            if(!colliders.ContainsKey(instanceID))
            {
                colliders.Add(instanceID, collider);
                if((invokeEvents | ColliderEvents.Enter) == invokeEvents) InvokeTriggerEvent(ColliderEvents.Enter, collider);
            
            } else if((invokeEvents | ColliderEvents.Stay) == invokeEvents)
            {
                InvokeTriggerEvent(ColliderEvents.Stay, collider);
            }
        }

        foreach(KeyValuePair<int, Collider> pair in colliders)
        {
            instanceID = pair.Key;
            bool contains = collidersMap.Count == 0 ? false : collidersMap.Contains(instanceID);

            if(!contains)
            {
                collidersToRemove.Add(instanceID);
                if((invokeEvents | ColliderEvents.Exit) == invokeEvents) InvokeTriggerEvent(ColliderEvents.Exit, pair.Value);
            }
        }

        foreach(int key in collidersToRemove)
        {
            colliders.Remove(key);
        }
    }

    /// <summary>Invokes Trigger Event.</summary>
    /// <param name="_event">Type of Collider Event.</param>
    /// <param name="_collider">Other Collider involved in the event.</param>
    private void InvokeTriggerEvent(ColliderEvents _event, Collider _collider)
    {
        if(onTriggerEvent != null) onTriggerEvent(_event, this, _collider);
    }
}
}