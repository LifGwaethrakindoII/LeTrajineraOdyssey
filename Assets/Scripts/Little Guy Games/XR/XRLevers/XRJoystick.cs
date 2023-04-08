using System.Collections;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using LittleGuyGames;

namespace LittleGuyGames.XR
{
/// <summary>Event invoked when axes change.</summary>
/// <param name="_axes">New Axes' value.</param>
/// <param name="_delta">Delta between the previous registered axes.</param>
public delegate void OnAxesChange(Vector2 _axes, Vector2 _delta);

public class XRJoystick : XRSimpleInteractable
{
    public event OnAxesChange onAxesChange;                     /// <summary>OnAxesChange's delegate.</summary>

    [Space(5f)]
    [Header("Joystick's Attributes:")]
    [SerializeField] private Transform _leverBase;              /// <summary>Joystick's Base.</summary>
    [SerializeField] private Transform _lever;                  /// <summary>Lever's Transform.</summary>
    [Space(5f)]
    [Header("Audio's Attributes:")]
    [SerializeField] private AudioSource _audioSource;          /// <summary>AudioSource's Component.</summary>
    [SerializeField] private AudioClip _movingSFX;              /// <summary>Joystick Moving's SFX.</summary>
    [Space(5f)]
    [SerializeField] private float _leverAngularLimits;         /// <summary>Lever's Angular Limits.</summary>
    [SerializeField] private float _restitutionDuration;        /// <summary>Restitution' Duration.</summary>
    [Space(5f)]
    [SerializeField] private XRInteractionUpdateOrder.UpdatePhase phase;        /// <summary>Test Phase.</summary>
    private XRBaseInteractor _interactor;                       /// <summary>Interactor that is interacting with this Joystick.</summary>
    private Vector2 _previousAxes;                              /// <summary>Previous Joystick's Axes [as a normalized Vector].</summary>
    private Vector2 _axes;                                      /// <summary>Joystick's Axes [as a normalized Vector].</summary>
    private bool _selected;                                     /// <summary>Is this Joystick currently selected by an XRInteractor?.</summary>
    protected Coroutine deselectCoroutine;                      /// <summary>De-selection's Coroutine reference.</summary>

    /// <summary>Gets leverBase property.</summary>
    public Transform leverBase { get { return _leverBase; } }

    /// <summary>Gets lever property.</summary>
    public Transform lever { get { return _lever; } }

    /// <summary>Gets audioSource property.</summary>
    public AudioSource audioSource { get { return _audioSource; } }

    /// <summary>Gets movingSFX property.</summary>
    public AudioClip movingSFX { get { return _movingSFX; } }

    /// <summary>Gets leverAngularLimits property.</summary>
    public float leverAngularLimits { get { return _leverAngularLimits; } }

    /// <summary>Gets restitutionDuration property.</summary>
    public float restitutionDuration { get { return _restitutionDuration; } }

    /// <summary>Gets and Sets interactor property.</summary>
    public XRBaseInteractor interactor
    {
        get { return _interactor; }
        protected set { _interactor = value; }
    }

    /// <summary>Gets and Sets previousAxes property.</summary>
    public Vector2 previousAxes
    {
        get { return _previousAxes; }
        protected set { _previousAxes = value; }
    }

    /// <summary>Gets and Sets axes property.</summary>
    public Vector2 axes
    {
        get { return _axes; }
        protected set { _axes = value; }
    }

    /// <summary>Gets and Sets selected property.</summary>
    public bool selected
    {
        get { return _selected; }
        protected set { _selected = value; }
    }

    /// <summary>Gets restituting property.</summary>
    public bool restituting { get { return deselectCoroutine != null; } }

    /// <summary>Draws Gizmos on Editor mode.</summary>
    private void OnDrawGizmos()
    {
        if(leverBase == null || lever == null) return;

        Vector3 position = leverBase.position;
        float rayLength = 1.0f;

        Gizmos.color = Color.green;
        Gizmos.DrawRay(position, leverBase.up * rayLength);
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(position, leverBase.forward * rayLength);
        Gizmos.color = Color.magenta;
        Gizmos.DrawRay(position, lever.forward * rayLength);

        if(interactor == null) return;

        Vector3 direction = interactor.transform.position - position;

        Gizmos.color = Color.cyan;
        Gizmos.DrawRay(position, direction);
    }

    /*/// <summary>Updates XRJoystick's instance at each frame.</summary>
    private void Update()
    {
        if(!selected) return;
    
        UpdateLever();
    }*/

    /// <summary>This method is called by the interaction manager to update the interactable. Please see the interaction manager documentation for more details on update order.</summary>
    /// <param name="_updatePhase">Update's Phase.</param>
    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase _updatePhase)
    {
        base.ProcessInteractable(_updatePhase);

        if(_updatePhase == phase) UpdateLever();

        /*switch(_updatePhase)
        {
            case XRInteractionUpdateOrder.UpdatePhase.Late:
                if(!selected) return;
    
                UpdateLever();
            break;
        }*/
    }

    /// <summary>Updates Lever.</summary>
    private void UpdateLever()
    {
        if(leverBase == null || lever == null || interactor == null) return;

        Vector3 o = leverBase.position;
        Vector3 up = leverBase.up;
        Vector3 direction = interactor.transform.position - o;
        Vector3 projection = Vector3.ProjectOnPlane(direction, up);
        float angle = Vector3.Angle(up, direction);
        float n = Mathf.Clamp(angle / leverAngularLimits, 0.0f, 1.0f);
        projection.Normalize();
        projection *= n;

        Vector3 a = Quaternion.Inverse(leverBase.rotation) * projection;
        previousAxes = axes;
        axes = new Vector2(a.x, a.z);

        Vector2 delta = axes - previousAxes;

        if(delta.sqrMagnitude > 0.0f)
        { /// The Joystick is moving, so lets play the sound and invoke a delta-change event:
            InvokeAxesChangeEvent(delta);
            PlaySound();
        }

        if(angle > leverAngularLimits)
        {
            float t = leverAngularLimits / 90.0f;
            direction = Vector3.Slerp(up, projection, t);
        }

        direction.Normalize();
        lever.rotation = Quaternion.LookRotation(direction);
    }

    /// <summary>Invokes OnAxeschange event.</summary>
    /// <param name="_delta">Difference between previous registered axes and current axes.</param>
    private void InvokeAxesChangeEvent(Vector2 _change)
    {
        if(onAxesChange != null) onAxesChange(axes, _change);
    }

    /// <summary>Callbak invoked when the Lever is Selected.</summary>
    /// <param name="_interactor">Interactor that is starting the selection.</param>
    protected override void OnSelectEntered(XRBaseInteractor _interactor)
    {
        if(interactor != null) return;

        base.OnSelectEntered(_interactor);
        selected = true;
        interactor = _interactor;
        axes = Vector2.zero;
        previousAxes = Vector2.zero;
        this.DispatchCoroutine(ref deselectCoroutine);
    }

    /// <summary>Callbak invoked when the Lever is Deselected.</summary>
    /// <param name="_interactor">Interactor that is ending the selection.</param>
    protected override void OnSelectExited(XRBaseInteractor _interactor)
    {
        if(interactor != _interactor) return;

        base.OnSelectExited(_interactor);
        selected = false;
        interactor = null;
        this.StartCoroutine(DeselectionRoutine(), ref deselectCoroutine);
        //axes = Vector2.zero;
    }

    /// <summary>Plays Sound.</summary>
    /// <param name="_play">Play? true by default.</param>
    private void PlaySound(bool _play = true)
    {
        if(audioSource == null || movingSFX == null) return;

        audioSource.PlayOneShot(movingSFX);

        /*switch(_play)
        {
            case true:
                if(audioSource.clip == movingSFX) return;

                audioSource.Play(movingSFX);
            break;

            case false:
                if(audioSource.clip != movingSFX) return;

                audioSource.Pause();
                audioSource.clip = null;
                audioSource.loop = false;
            break;
        }*/
    }

    /// <summary>Sets Axes.</summary>
    /// <param name="_axes">New Axes.</param>
    public void SetAxes(Vector2 _axes)
    {
        axes = _axes;
    }

    /// <returns>String representing this Joystick.</returns>
    public override string ToString()
    {
        StringBuilder builder = new StringBuilder();

        builder.Append("Axes: ");
        builder.Append(axes.ToString());

        return builder.ToString();
    }

    /// <summary>De-selection's routine.</summary>
    private IEnumerator DeselectionRoutine()
    {
        Vector2 originalAxes = axes;
        Quaternion a = lever.localRotation;
        Quaternion b = Quaternion.Euler(270.0f, 0.0f, 0.0f);
        float i = 1.0f / restitutionDuration;
        float t = 0.0f;

        while(t < 1.0f)
        {
            float st = t * t;
            lever.localRotation = Quaternion.Slerp(a, b, st);
            previousAxes = axes;
            axes = Vector2.Lerp(originalAxes, Vector2.zero, st);
            if(previousAxes != axes) InvokeAxesChangeEvent(axes - previousAxes);
            t += (Time.deltaTime * i);
            yield return null;
        }

        lever.localRotation = b;
        previousAxes = axes;
        axes = Vector2.zero;
        if(previousAxes != axes) InvokeAxesChangeEvent(axes - previousAxes);
        previousAxes = axes;
        this.DispatchCoroutine(ref deselectCoroutine);
    }
}
}