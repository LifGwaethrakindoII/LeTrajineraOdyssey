using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

/*============================================================
**
** Class:  XRButton
**
** Purpose: XR Interaction's Toolkit Component that acts as a
** physical button.
**
**
** Author: Lîf Gwaethrakindo
**
==============================================================*/

namespace LittleGuyGames.XR
{
public enum XRButtonState
{
    None,
    Pressed,
    Released
}

public class XRButton : XRSimpleInteractable
{
    [Space(5f)]
    [SerializeField] private float _minInteractorDistance;                  /// <summary>Minimum distance between the interactor to keep interacting.</summary>
    [Space(5f)]
    [Header("Button's Attributes:")]
    [SerializeField] private Vector3 _interactionStartingPoint;             /// <summary>Starting Relative Point of Interaction.</summary>
    [SerializeField] private Vector3 _interactionEndingPoint;               /// <summary>Ending Relative Point of Interaction.</summary>
    [SerializeField][Range(0.0f, 1.0f)] private float _pressedProgress;     /// <summary>Pressed's Progress.</summary>
    [SerializeField][Range(0.0f, 1.0f)] private float _releasedProgress;    /// <summary>Released's Progress.</summary>
    [SerializeField] private float _restitutionDuration;                    /// <summary>Restitution's Duration.</summary>
    [Space(5f)]
    [Header("Animation's Attributes:")]
    [SerializeField] private Animator _animator;                            /// <summary>Animator's reference.</summary>
    [SerializeField] private string _animation;                             /// <summary>Animation's Name.</summary>
    [Space(5f)]
    [Header("Audio's Attributes:")]
    [SerializeField] private AudioSource _audioSource;                      /// <summary>AudioSource's Component.</summary>
    [SerializeField] private AudioClip _pressedSFX;                         /// <summary>Pressed's SFX.</summary>
    [SerializeField] private AudioClip _releasedSFX;                        /// <summary>Released's SFX.</summary>
    [Space(5f)]
    [SerializeField] private UnityEvent _onButtonPressed;                   /// <summary>Event invoked when this button is pressed.</summary>
    [SerializeField] private UnityEvent _onButtonReleased;                  /// <summary>Event invoked when the button is released.</summary>
    [Space(5f)]
    [SerializeField] private XRInteractionUpdateOrder.UpdatePhase phase;        /// <summary>Test Phase.</summary>
    private int _animationHash;                                             /// <summary>Animation's Hash.</summary>
    private float _progress;                                                /// <summary>Current Button's Progress.</summary>
    private XRBaseInteractor _interactor;                                   /// <summary>Main Interactor's reference.</summary>
    private XRButtonState _state;                                           /// <summary>Current Button's Stage.</summary>
    protected Coroutine deselectCoroutine;                                  /// <summary>Deselection's Coroutine reference.</summary>

#region Getters/Setters:
    /// <summary>Gets interactionStartingPoint property.</summary>
    public Vector3 interactionStartingPoint { get { return _interactionStartingPoint; } }

    /// <summary>Gets interactionEndingPoint property.</summary>
    public Vector3 interactionEndingPoint { get { return _interactionEndingPoint; } }

    /// <summary>Gets minInteractorDistance property.</summary>
    public float minInteractorDistance { get { return _minInteractorDistance; } }

    /// <summary>Gets pressedProgress property.</summary>
    public float pressedProgress { get { return _pressedProgress; } }

    /// <summary>Gets releasedProgress property.</summary>
    public float releasedProgress { get { return _releasedProgress; } }

    /// <summary>Gets restitutionDuration property.</summary>
    public float restitutionDuration { get { return _restitutionDuration; } }

    /// <summary>Gets and Sets progress property.</summary>
    public float progress
    {
        get { return _progress; }
        protected set { _progress = value; }
    }

    /// <summary>Gets animator property.</summary>
    public Animator animator { get { return _animator; } }

    /// <summary>Gets animation property.</summary>
    public string animation { get { return _animation; } }

    /// <summary>Gets audioSource property.</summary>
    public AudioSource audioSource { get { return _audioSource; } }

    /// <summary>Gets pressedSFX property.</summary>
    public AudioClip pressedSFX { get { return _pressedSFX; } }

    /// <summary>Gets releasedSFX property.</summary>
    public AudioClip releasedSFX { get { return _releasedSFX; } }

    /// <summary>Gets and Sets onButtonPressed property.</summary>
    public UnityEvent onButtonPressed
    {
        get { return _onButtonPressed; }
        set { _onButtonPressed = value; }
    }

    /// <summary>Gets and Sets onButtonReleased property.</summary>
    public UnityEvent onButtonReleased
    {
        get { return _onButtonReleased; }
        set { _onButtonReleased = value; }
    }

    /// <summary>Gets and Sets interactor property.</summary>
    public XRBaseInteractor interactor
    {
        get { return _interactor; }
        protected set { _interactor = value; }
    }

    /// <summary>Gets and Sets animationHash property.</summary>
    public int animationHash
    {
        get { return _animationHash; }
        protected set { _animationHash = value; }
    }

    /// <summary>Gets and Sets state property.</summary>
    public XRButtonState state
    {
        get { return _state; }
        private set { _state = value; }
    }
#endregion

    /// <summary>Draws Gizmos on Editor mode when XRButton's instance is selected.</summary>
    private void OnDrawGizmosSelected()
    {
        Vector3 a = transform.TransformPoint(interactionStartingPoint);
        Vector3 b = transform.TransformPoint(interactionEndingPoint);
        float r = 0.01f;
        Color color = Color.white;
        color.a = 0.75f;

        Gizmos.color = color;

        Gizmos.DrawSphere(a, r);
        Gizmos.DrawSphere(b, r);
        Gizmos.DrawLine(a, b);
    }

    /// <summary>XRButton's instance initialization when loaded [Before scene loads].</summary>
    protected override void Awake()
    {
        base.Awake();
        animationHash = Animator.StringToHash(animation);
        state = XRButtonState.None;
        progress = 0.0f;
    }

    /// <summary>Updates XRButton's instance at each frame.</summary>
    private void UpdateButton()
    {
        EvaluateInteractorDistance();
        EvaluateInteractor();

        switch(state)
        {
            case XRButtonState.Pressed:
                if(progress <= releasedProgress)
                {
                    state = XRButtonState.Released;
                    state = XRButtonState.None;
                    if(audioSource != null && releasedSFX != null) audioSource.PlayOneShot(releasedSFX);
                    onButtonReleased.Invoke();
                    Debug.Log("[XRButton] Released!");
                }
            break;
        }
    }

    /// <summary>This method is called by the interaction manager to update the interactable. Please see the interaction manager documentation for more details on update order.</summary>
    /// <param name="_updatePhase">Update's Phase.</param>
    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase _updatePhase)
    {
        base.ProcessInteractable(_updatePhase);

        if(_updatePhase == phase) UpdateButton();
    }

    /// <summary>Evaluates distance between current Interactor.</summary>
    private void EvaluateInteractorDistance()
    {
        if(interactor == null || GetDistanceSqrToInteractor(interactor) <= (minInteractorDistance * minInteractorDistance)) return;

        OnSelectExited(interactor);
        OnHoverExited(interactor);
    }

    /// <summary>Evaluates Interactor [if registered].</summary>
    private void EvaluateInteractor()
    {
        if(interactor == null /*|| state == XRButtonState.Released*/ || deselectCoroutine != null) return;

        Vector3 a = transform.TransformPoint(interactionStartingPoint);
        Vector3 b = transform.TransformPoint(interactionEndingPoint);
        Vector3 plane = b - a;
        Vector3 position = interactor.transform.position;
        Vector3 direction = position - a;
        Vector3 projectionPlane = Vector3.ProjectOnPlane(direction, plane);
        Vector3 projection = Vector3.ProjectOnPlane(direction, projectionPlane);

        progress = Vector3.Dot(plane, projection) < 0.0f ? 0.0f : projection.magnitude / plane.magnitude;
        animator.Play(animationHash, 0, progress);

        switch(state)
        {
            case XRButtonState.None:
                if(progress >= pressedProgress)
                {
                    state = XRButtonState.Pressed;
                    if(audioSource != null && pressedSFX != null) audioSource.PlayOneShot(pressedSFX);
                    onButtonPressed.Invoke();
                    Debug.Log("[XRButton] Pressed!");
                }
            break;
        }
    }

    /// <summary>Callbak invoked when the Lever is Hovered.</summary>
    /// <param name="_interactor">Interactor that is starting the selection.</param>
    protected override void OnHoverEntered(XRBaseInteractor _interactor)
    {
        base.OnHoverEntered(_interactor);

        if(interactor != null && interactor != _interactor) return;

        interactor = _interactor;
    }

    /// <summary>Callbak invoked when the Lever is Un-hovered.</summary>
    /// <param name="_interactor">Interactor that is ending the selection.</param>
    protected override void OnHoverExited(XRBaseInteractor _interactor)
    {
        base.OnHoverExited(_interactor);

        if(interactor != _interactor || deselectCoroutine != null) return;

        interactor = null;
        state = XRButtonState.Released;
        this.StartCoroutine(DeselectionRoutine(), ref deselectCoroutine);
    }

    /// <summary>Deselection's Routine.</summary>
    protected virtual IEnumerator DeselectionRoutine()
    {
        float a = progress;
        float b = 0.0f;
        float i = 1.0f / (restitutionDuration * progress);
        float t = 0.0f;

        while(t < 1.0f)
        {
            progress = Mathf.Lerp(a, b, t);
            animator.Play(animationHash, 0, progress);
            t += (Time.deltaTime * i);
            yield return null;
        }

        progress = 0.0f;
        animator.Play(animationHash, 0, progress);
        animator.Play(LGGExtensions.HASH_ANIMATION_EMPTY, 0);
        state = XRButtonState.None;
        this.DispatchCoroutine(ref deselectCoroutine);
    }
}
}