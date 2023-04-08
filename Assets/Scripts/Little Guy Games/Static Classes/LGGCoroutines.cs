using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using Unity.XR.CoreUtils;

/*============================================================
**
** Class:  LGGCoroutines
**
** Purpose: Static class that contains functions specifically
** related to Coroutines.
**
**
** Author: Lîf Gwaethrakindo
**
==============================================================*/

namespace LittleGuyGames
{
public static class LGGCoroutines
{
    public static readonly int HASH_MATERIALPROPERTY_COLOR;

    static LGGCoroutines()
    {
        HASH_MATERIALPROPERTY_COLOR = Shader.PropertyToID("_Color");
    }

    /// <summary>Starts Coroutine and stores it on a Coroutine's reference.</summary>
    /// <param name="_monoBehaviour">MonoBehaviour reference that will start the Coroutine.</param>
    /// <param name="_iterator">IEnumerator that contains the Coroutine's instructions.</param>
    /// <param name="_coroutine">Coroutine's reference.</param>
    /// <returns>Started Coroutine.</returns>
    public static Coroutine StartCoroutine(this MonoBehaviour _monoBehaviour, IEnumerator _iterator, ref Coroutine _coroutine)
    {
       if(_monoBehaviour == null) return null;

        _monoBehaviour.DispatchCoroutine(ref _coroutine);
        _coroutine = _monoBehaviour.StartCoroutine(_iterator);

        return _coroutine;
    }

    /// <summary>Starts Coroutine and stores it on a Coroutine's reference inside a dictionary.</summary>
    /// <param name="_monoBehaviour">MonoBehaviour reference that will start the Coroutine.</param>
    /// <param name="_ID">Key's ID.</param>
    /// <param name="_dictionary">Dictionary's reference.</param>
    /// <param name="_iterator">IEnumerator that contains the Coroutine's instructions.</param>
    /// <returns>Started Coroutine.</returns>
    public static Coroutine StartCoroutine(this MonoBehaviour _monoBehaviour, int _ID, ref Dictionary<int, Coroutine> _dictionary, IEnumerator _iterator)
    {
        if(_monoBehaviour == null) return null;
        if(_dictionary == null) _dictionary = new Dictionary<int, Coroutine>();

        _monoBehaviour.DispatchCoroutine(_ID, ref _dictionary);

        Coroutine coroutine = _monoBehaviour.StartCoroutine(_iterator);

        if(!_dictionary.ContainsKey(_ID)) _dictionary.Add(_ID, coroutine);
        else _dictionary[_ID] = coroutine;

        return coroutine;
    }

    /// <summary>Dispatches Coroutine and empties reference's memory.</summary>
    /// <param name="_monoBehaviour">MonoBehaviour reference that will dispatch the Coroutine.</param>
    /// <param name="_coroutine">Coroutine to dispatch.</param>
    public static void DispatchCoroutine(this MonoBehaviour _monoBehaviour, ref Coroutine _coroutine)
    {
        if(_monoBehaviour == null || _coroutine == null) return;

        _monoBehaviour.StopCoroutine(_coroutine);
        _coroutine = null;
    }

    /// <summary>Dispatches Coroutine and empties reference's memory inside Dictionary.</summary>
    /// <param name="_monoBehaviour">MonoBehaviour reference that will dispatch the Coroutine.</param>
    /// <param name="_ID">Key ID of the Coroutine inside the Dictionary.</param>
    /// <param name="_dictionary">Dictionary that contains Coroutine to dispatch.</param>
    public static void DispatchCoroutine(this MonoBehaviour _monoBehaviour, int _ID, ref Dictionary<int, Coroutine> _dictionary)
    {
        if(_monoBehaviour == null || _dictionary == null || !_dictionary.ContainsKey(_ID)) return;

        Coroutine coroutine = _dictionary[_ID];
        if(coroutine != null) _monoBehaviour.StopCoroutine(coroutine);
        _dictionary.Remove(_ID);
    }

    /// <summary>Runs multiple IEnumerator [coroutines] until all return false [by MoveNext()].</summary>
    /// <param name="_routines">IEnumerators to run.</param>
    public static IEnumerator Routines(params IEnumerator[] _routines)
    {
        if(_routines == null) yield break;

        int max = _routines.Length;
        int current = 0;

        while(current < max)
        {
            current = 0;

            foreach(IEnumerator routine in _routines)
            {
                if(!routine.MoveNext()) current++;
            }

            yield return null;
        }
    }

    /// <summary>Loops Sound.</summary>
    /// <param name="_source">AudioSource that will loop the sound.</param>
    /// <param name="_clip">Sound's AudioClip.</param>
    public static IEnumerator LoopSound(this AudioSource _source, AudioClip _clip)
    {
        if(_source == null || _clip == null) yield break;

        float duration = _clip.length;
        float t = 0.0f;

        _source.PlayOneShot(_clip);

        while(true)
        {
            while(t < 1.0f)
            {
                t += Time.deltaTime;
                yield return null;
            }

            t = 0.0f;
            _source.PlayOneShot(_clip);
        }
    }

    /// <summary>Scales transform towards desired scale.</summary>
    /// <param name="_transform">Transform to scale.</param>
    /// <param name="_scale">Desired scale.</param>
    /// <param name="_duration">Scaling's duration.</param>
    /// <param name="onScaleEnds">Optional callback invoked when the scaling ends, null by default.</param>
    /// <param name="f">Optional easing function, null by default.</param>
    public static IEnumerator Scale(this Transform _transform, Vector3 _scale, float _duration, Action onScaleEnds = null, Func<float, float> f = null)
    {
        Vector3 s = _transform.localScale;
        float t = 0.0f;
        float i = 1.0f / _duration;
        if(f == null) f = LGGMath.F;

        while(t < 1.0f)
        {
            _transform.localScale = Vector3.Lerp(s, _scale, f(t));
            t += (Time.deltaTime * i);
            yield return null;
        }

        _transform.localScale = _scale;
        if(onScaleEnds != null) onScaleEnds();
    }

    /// <summary>Waits for seconds, then excecutes callback.</summary>
    /// <param name="s">Time in seconds to wait. Be sure to pass a time above 0.0f, otherwise there won'r be any wait happening.</param>
    /// <param name="onWaitEnds">Callback to invoke when the wait time has been done.</param>
    public static IEnumerator Wait(this MonoBehaviour _mb, float s, Action onWaitEnds = null)
    {
        if(s > 0.0f)
        {
            float t = 0.0f;

            while(t < s && _mb != null)
            {
                t += Time.deltaTime;
                yield return null;
            }
        }

        if(onWaitEnds != null) onWaitEnds();
        yield break;
    }

    public static IEnumerator DoDuring(this MonoBehaviour _monoBehaviour, Action action, float duration, Action onWaitEnds = null)
    {
        if(duration > 0.0f)
        {
            float t = 0.0f;

            while(t < duration && _monoBehaviour != null)
            {
                action();
                t += Time.deltaTime;
                yield return null;
            }
        }

        if(onWaitEnds != null) onWaitEnds();
    }

    /// \TODO Either deprecate or clean:
    /// <summary>Rotates Transform towards Transform's forward.</summary>
    /// <param name="a">Transform to rotate.</param>
    /// <param name="b">Target Transform.</param>
    /// <param name="d">Rotation's duration.</param>
    /// <param name="onRotationEnds">Optional callback invoked when the rotation ends.</param>
    /// <param name="f">Optional easing function, null by default.</param>
    public static IEnumerator RotateTowardsTransformForward(this Transform a, Transform b, float d, Action onRotationEnds = null, Func<float, float> f = null)
    {
        Quaternion rA = a.rotation;
        Quaternion rB = b.rotation;
        float i = 1.0f / d;
        float t = 0.0f;

        if(f == null) f = LGGMath.F;

        while(t < 1.0f)
        {
            a.rotation = Quaternion.Lerp(rA, rB, f(t));
            t += (Time.deltaTime * i);
            yield return null;
        }

        a.rotation = rB;
        if(onRotationEnds != null) onRotationEnds();
    }

    /// \TODO Either deprecate or clean:
    /// <summary>Rotates XROrigin towards Transform's forward.</summary>
    /// <param name="_XROrigin">XROrigin's reference.</param>
    /// <param name="_transform">Target Transform.</param>
    /// <param name="d">Rotation's duration.</param>
    /// <param name="onRotationEnds">Optional callback invoked when the rotation ends.</param>
    /// <param name="f">Optional easing function, null by default.</param>
    public static IEnumerator RotateXROriginTowardsTransformForward(this XROrigin _XROrigin, Transform _transform, float d, Action onRotationEnds = null, Func<float, float> f = null)
    {
        Quaternion initialOriginRotation = _XROrigin.Origin.transform.rotation;
        Quaternion initialCameraRotation = _XROrigin.Camera.transform.rotation;
        Vector3 initialOriginForward = _XROrigin.transform.forward;
        Vector3 initialCameraForward = _XROrigin.Camera.transform.forward;
        float i = 1.0f / d;
        float t = 0.0f;

        if(f == null) f = LGGMath.F;

        while(t < 1.0f)
        {
            Vector3 up = _transform.up;
            Vector3 forward = 
#if UNITY_EDITOR
                _transform.forward;
#else
                -_transform.forward;
#endif

            _XROrigin.Origin.transform.rotation = Quaternion.Lerp(initialOriginRotation, _transform.rotation, f(t));
            _XROrigin.MatchOriginUpCameraForward(up, Vector3.Lerp(initialOriginForward, forward, f(t)));
            _XROrigin.MatchOriginUpOriginForward(up, Vector3.Lerp(initialCameraForward, forward, f(t)));
            t += (Time.deltaTime * i);

            yield return null;
        }

        _XROrigin.Origin.transform.rotation = _transform.rotation;
        _XROrigin.MatchOriginUpCameraForward(_transform.up, _transform.forward);
        _XROrigin.MatchOriginUpOriginForward(_transform.up, _transform.forward);

        if(onRotationEnds != null) onRotationEnds();
    }

    /// <summary>Cross-Fades alpha of given graphic for a period of time, and invokes callback.</summary>
    /// <param name="_graphic">Graphic's reference.</param>
    /// <param name="_alpha">Target alpha value.</param>
    /// <param name="_ignoreTimeScale">Should ignore Time.scale?.</param>
    /// <param name="onCrossFadeEnds">Optional callback invoked when the Cross-Fade ends.</param>
    public static IEnumerator CrossFadeAlpha(this Graphic _graphic, float _alpha, float _duration, bool _ignoreTimeScale, Action onCrossFadedEnds = null)
    {
        float t = 0.0f;

        _graphic.CrossFadeAlpha(_alpha, _duration, _ignoreTimeScale);

        while(t < _duration)
        {
            t += _ignoreTimeScale ? Time.unscaledDeltaTime : Time.deltaTime;
            yield return null;
        }

        if(onCrossFadedEnds != null) onCrossFadedEnds();
    }

    public static IEnumerator InterpolateMaterialColor(this Material _material, Color _color, float _duration, string _colorProperty = "_Color", Action onInterpolationEnds = null, Func<float, float> f = null)
    {
        int hash = Shader.PropertyToID(_colorProperty);
        Color initialColor = _material.GetColor(hash);
        float i = 1.0f / _duration;
        float t = 0.0f;

        if(f == null) f = LGGMath.F;

        while(t < 1.0f)
        {
            _material.SetColor(hash, Color.Lerp(initialColor, _color, f(t)));
            t += (Time.deltaTime * i);
            yield return null;
        }

        _material.SetColor(hash, _color);
        if(onInterpolationEnds != null) onInterpolationEnds();
    }
}
}