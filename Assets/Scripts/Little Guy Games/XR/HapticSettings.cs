using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace LittleGuyGames.XR
{
[Serializable]
public struct HapticSettings
{
    [SerializeField][Range(0.0f, 1.0f)] private float _impulse;
    public float duration;

    /// <summary>Gets and Sets impulse property.</summary>
    public float impulse
    {
        get { return _impulse; }
        set { _impulse = Mathf.Clamp(value, 0.0f, 1.0f); }
    }

    /// <summary>HapticSettings' Constructor.</summary>
    /// <param name="_impulse">Impulse's Amplitude.</param>
    /// <param name="_duration">Impulse's Duration.</param>
    public HapticSettings(float _impulse, float _duration) : this()
    {
        impulse = _impulse;
        duration = _duration;
    }
}
}