using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LittleGuyGames
{
[RequireComponent(typeof(Rigidbody))]
public class DisplacementAccumulator : MonoBehaviour
{
    private Dictionary<int, Vector3> _accelerations; /// <summary>Accelerations' Dictionary.</summary>
    private Vector3 _velocity;                      /// <summary>Accumulator's Velocity.</summary>
    private Rigidbody _rigidbody;                   /// <summary>Rigidbody's Component.</summary>

    /// <summary>Gets and Sets accelerations property.</summary>
    public Dictionary<int, Vector3> accelerations
    {
        get { return _accelerations; }
        protected set { _accelerations = value; }
    }

    /// <summary>Gets and Sets velocity property.</summary>
    public Vector3 velocity
    {
        get { return _velocity; }
        set { _velocity = value; }
    }

    /// <summary>Gets rigidbody Component.</summary>
    public Rigidbody rigidbody
    { 
        get
        {
            if(_rigidbody == null) _rigidbody = GetComponent<Rigidbody>();
            return _rigidbody;
        }
    }

    /// <summary>Callback invoked when DisplacementAccumulator's instance is enabled.</summary>
    private void OnEnable()
    {
        FixedLateUpdateInvoker.onFixedLateUpdate += OnFixedLateUpdate;
    }

    /// <summary>Callback invoked when DisplacementAccumulator's instance is disabled.</summary>
    private void OnDisable()
    {
        FixedLateUpdateInvoker.onFixedLateUpdate -= OnFixedLateUpdate;
    }

    /// <summary>DisplacementAccumulator's instance initialization.</summary>
    private void Awake()
    {
        accelerations = new Dictionary<int, Vector3>();
        velocity = Vector3.zero;
        //this.StartCoroutine(WaitForEndOfPhysicsThread());
    }

    /// <summary>Updates DisplacementAccumulator's instance at each Physics Thread's frame.</summary>
    private void FixedUpdate()
    {
        //OnEndOfPhysicsStep();
    }

    /// <summary>Callback invoked at the end of the Physics' Step.</summary>
    private void OnFixedLateUpdate()
    {
        OnEndOfPhysicsStep();
    }

    /// <summary>Callback invoked at the end of the Physics Step.</summary>
    private void OnEndOfPhysicsStep()
    {
        if(velocity.sqrMagnitude == 0.0f) return;

        rigidbody.MovePosition(rigidbody.position + (velocity * Time.fixedDeltaTime));
        velocity *= 0.0f;
    }

    /// <summary>Adds Acceleration.</summary>
    /// <param name="_key">Acceleration's Key.</param>
    /// <param name="_acceleration">Acceleration's vector [should not be scaled by time delta].</param>
    public void AddAcceleration(int _key, Vector3 _acceleration)
    {
        Vector3 acceleration = Vector3.zero;

        if(!accelerations.TryGetValue(_key, out acceleration))
            accelerations.Add(_key, acceleration);

        acceleration += _acceleration * Time.fixedDeltaTime;

        accelerations[_key] = acceleration;
    }

    /// <summary>Removes Acceleration Entry.</summary>
    /// <param name="_key">Acceleration's Key.</param>
    public void RemoveAcceleration(int _key)
    {
        if(accelerations.ContainsKey(_key))
        accelerations.Remove(_key);
    }

    /// <summary>Adds displacement to this accumulator.</summary>
    /// <param name="_displacement">Displacement to accumulate.</param>
    public void AddDisplacement(Vector3 _displacement)
    {
        velocity += _displacement;
    }

    /// <summary>Waits for the end of the Physics's Step and invokes a callback.</summary>
    private IEnumerator WaitForEndOfPhysicsThread()
    {
        while(true)
        {
            OnEndOfPhysicsStep();
            yield return LGGPhysics.WAIT_PHYSICS_THREAD;
        }
    }
}
}