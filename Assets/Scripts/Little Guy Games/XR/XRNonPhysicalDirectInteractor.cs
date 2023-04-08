using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Utilities;
using UnityEngine.XR.Interaction.Toolkit;

/*============================================================
**
** Class:  XRNonPhysicalDirectInteractor
**
** Purpose: This XRInteractor component has the scope of 
** invoking XR-Events callbacks that are otherwise possible
** with OnTrigger Events (therefore, needing a Rigidbody
** from either the XRInteractable or XRInteractor) without 
** making use of Rigidbody, thanks to Physics' Overlap
** functions.
**
**
** Author: Lîf Gwaethrakindo
**
==============================================================*/

namespace LittleGuyGames.XR
{
public class XRNonPhysicalDirectInteractor : XRDirectInteractor
{
    [Space(5f)]
    [Header("Non-Physical XR Interactor's Attributes:")]
    [SerializeField] private float radius;          /// <summary>Overlap Sphere's Radius.</summary>
    [SerializeField] private LayerMask mask;        /// <summary>Overlap's LayerMask.</summary>
    private Dictionary<int, Collider> colliders;
    private HashSet<int> collidersMap;
    private HashSet<int> collidersToRemove;

    /// <summary>Draws Gizmos on Editor mode when XRNonPhysicalDirectInteractor's instance is selected.</summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    /// <summary>Resets XRNonPhysicalDirectInteractor's instance to its default values.</summary>
    protected override void Reset()
    {
        base.Reset();
        mask = Physics.AllLayers;
        radius = 0.05f;
    }

    /// <summary>XRNonPhysicalDirectInteractor's instance initialization when loaded [Before scene loads].</summary>
    protected override void Awake()
    {
        base.Awake();
        colliders = new Dictionary<int, Collider>();
        collidersMap = new HashSet<int>();
        collidersToRemove = new HashSet<int>();
    }

    /// <summary>Updates XRNonPhysicalDirectInteractor's instance at each frame.</summary>
    protected virtual void Update()
    {
        Collider[] overlappedColliders = Physics.OverlapSphere(transform.position, radius, mask);

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
                OnTriggerEnter(collider);
            }
            else OnTriggerStay(collider);
        }

        foreach(KeyValuePair<int, Collider> pair in colliders)
        {
            instanceID = pair.Key;

            if(collidersMap.Contains(instanceID)) continue;

            collidersToRemove.Add(instanceID);
            OnTriggerExit(pair.Value);
        }

        foreach(int key in collidersToRemove)
        {
            colliders.Remove(key);
        }
    }
}
}