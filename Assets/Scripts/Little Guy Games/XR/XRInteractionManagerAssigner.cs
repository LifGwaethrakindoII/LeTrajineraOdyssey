using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Sirenix.OdinInspector;

namespace LittleGuyGames.XR
{
[RequireComponent(typeof(XRInteractionManager))]
public class XRInteractionManagerAssigner : MonoBehaviour
{
    private XRInteractionManager _interactionManager;   /// <summary>XRInteractionManager's reference.</summary>

    /// <summary>Gets interactionManager Component.</summary>
    public XRInteractionManager interactionManager
    { 
        get
        {
            if(_interactionManager == null) _interactionManager = GetComponent<XRInteractionManager>();
            return _interactionManager;
        }
    }

    [Button("Assign to XR Interactors/Interactables")]
    /// <summary>Assigns XRInteractionManager reference to all XR Interactors and Interactables in scene.</summary>
    public void Assign()
    {
        XRBaseInteractor[] interactors = FindObjectsOfType<XRBaseInteractor>();

        if(interactors != null) foreach(XRBaseInteractor interactor in interactors)
        {
            interactor.interactionManager = interactionManager;
        }

        XRBaseInteractable[] interactables = FindObjectsOfType<XRBaseInteractable>();

        if(interactables != null) foreach(XRBaseInteractable interactable in interactables)
        {
            interactable.interactionManager = interactionManager;
        }
    }
}
}