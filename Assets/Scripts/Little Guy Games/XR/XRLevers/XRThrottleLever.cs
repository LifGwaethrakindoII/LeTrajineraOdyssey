using System.Collections;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using LittleGuyGames;

namespace LittleGuyGames.XR
{
/// <summary>Event invoked when Throttle Lever's progress value has changed.</summary>
/// <param name="_progress">New Progress Value.</param>
/// <param name="_index">Index.</param>
public delegate void OnThrottleLeverValueChanged(float _progress, int _index);

public class XRThrottleLever : XRBaseLever
{
    public event OnThrottleLeverValueChanged onThrottleLeverValueChanged;   /// <summary>OnThrottleLeverValueChanged's delegate.</summary>

    [SerializeField][Range(0.0f, 1.0f)] private float[] _ranges;            /// <summary>Throttle's Ranges.</summary>    

    /// <summary>Gets ranges property.</summary>
    public float[] ranges { get { return _ranges; } }

    /// <summary>Updates Lever.</summary>
    protected override void UpdateLever()
    {
        base.UpdateLever();
    }

    /// <summary>Onvokes OnLeverValueChanged event.</summary>
    protected override void InvokeLeverProgressChangeEvent()
    {
        base.InvokeLeverProgressChangeEvent();
        if(onThrottleLeverValueChanged != null) onThrottleLeverValueChanged(progress, GetProgressIndex());

        /*StringBuilder builder = new StringBuilder();

        builder.Append("Invoking OnThrottleLeverValueChanged with the following data: ");
        builder.Append(ToString());
        
        Debug.Log(builder.ToString());*/
    }

    /// <returns>Progress' Index.</returns>
    public int GetProgressIndex()
    {
        if(ranges == null || ranges.Length == 0) return 0;

        float min = 0.0f;

        for(int i = 0; i < ranges.Length; i++)
        {
            if(progress >= min && progress < ranges[i]) return i;
        }

        return ranges.Length - 1;
    }

    /// <returns>String representing this Joystick.</returns>
    public override string ToString()
    {
        StringBuilder builder = new StringBuilder();

        builder.Append("Lever = { ");
        builder.Append("Progress: ");
        builder.Append(progress.ToString());
        builder.Append(", Previous Progress: ");
        builder.Append(previousProgress.ToString());
        builder.Append(", Progress Index: ");
        builder.Append(GetProgressIndex().ToString());
        builder.Append(" }");

        return builder.ToString();
    }
}
}