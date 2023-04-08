using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace LittleGuyGames
{
[Flags]
public enum ColliderEvents
{
    None = 0,
    Enter = 1,
    Exit = 2,
    Stay = 4,

    EnterAndExit = Enter | Exit,
    EnterAndStay = Enter | Stay,
    ExitAndStay = Exit | Stay,
    All = Enter | Exit | Stay
}

/// <summary>Event invoked when a trigger occurs.</summary>
/// <param name="_event">Type of Collider Event.</param>
/// <param name="_invoker">Invoker's reference.</param>
/// <param name="_collider">Other Collider involved in the event.</param>
public delegate void OnTriggerEvent(ColliderEvents _event, ColliderEventsInvoker _invoker, Collider _collider);

public class ColliderEventsInvoker : MonoBehaviour
{
    public event OnTriggerEvent onTriggerEvent;                /// <summary>OnTriggerEvent's Delegate.</summary>

    [SerializeField] private ColliderEvents _invokeEvents;     /// <summary>Collider-Events to invoke.</summary>

    /// <summary>Gets and Sets invokeEvents property.</summary>
    public ColliderEvents invokeEvents
    {
        get { return _invokeEvents; }
        set { _invokeEvents = value; }
    }

    /// <summary>Event triggered when this Collider enters another Collider trigger.</summary>
    /// <param name="col">The other Collider involved in this Event.</param>
    private void OnTriggerEnter(Collider col)
    {
        if((invokeEvents | ColliderEvents.Enter) == invokeEvents) InvokeTriggerEvent(ColliderEvents.Enter, col);
    }

    /// <summary>Event triggered when this Collider exits another Collider trigger.</summary>
    /// <param name="col">The other Collider involved in this Event.</param>
    private void OnTriggerExit(Collider col)
    {
        if((invokeEvents | ColliderEvents.Exit) == invokeEvents) InvokeTriggerEvent(ColliderEvents.Exit, col);
    }

    /// <summary>Event triggered when this Collider stays with another Collider trigger.</summary>
    /// <param name="col">The other Collider involved in this Event.</param>
    private void OnTriggerStay(Collider col)
    {
        if((invokeEvents | ColliderEvents.Stay) == invokeEvents) InvokeTriggerEvent(ColliderEvents.Stay, col);
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