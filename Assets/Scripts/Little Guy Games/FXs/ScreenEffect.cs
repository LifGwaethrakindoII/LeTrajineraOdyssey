using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LittleGuyGames
{
[Serializable]
public class ScreenEffectLayer
{
    [SerializeField] public Image[] images;
    [SerializeField] public Vector3 displacement;
    [SerializeField] public float displacementDuration;
    [SerializeField] public float scaleDuration;
    [SerializeField] public float fadeInDuration;
    [SerializeField] public float fadeOutDuration;
    [SerializeField][Range(0.0f, 1.0f)] public float fadeInAlpha;
    private Vector3[] initialPositions;

    /// <summary>Sets Images' initial positions.</summary>
    public void SetInitialPositions()
    {
        initialPositions = new Vector3[images.Length];

        for(int i = 0; i < images.Length; i++)
        {
            initialPositions[i] = images[i].transform.localPosition;
        }
    }

    /// <summary>Lerps Displacment of referenced images.</summary>
    /// <param name="t">Normalized time t [unclmapled].</param>
    public void LerpDisplacement(float t)
    {
        if(initialPositions == null || initialPositions.Length != images.Length) SetInitialPositions();

        for(int i = 0; i < images.Length; i++)
        {
            Vector3 a = initialPositions[i];
            Vector3 b = a + displacement;

            images[i].transform.localPosition = Vector3.Lerp(a, b, LGGMath.EaseInOutSin(t));
        }
    }

    /// <summary>Sets Alpha of all referenced images.</summary>
    /// <param name="a">Target alpha value.</param>
    public void SetAlpha(float a)
    {
        foreach(Image image in images)
        {
            image.color = image.color.WithAlpha(a);
        }
    }

    /// <summary>Sets Scale of all referenced images.</summary>
    /// <param name="s">Target's scale.</param>
    public void SetScale(float s)
    {
        Vector3 scale = Vector3.one * s;

        foreach(Image image in images)
        {
            image.transform.localScale = scale;
        }   
    }

    public IEnumerator ScaleRoutine(float s)
    {
        float a = images[0].transform.localScale.x;
        float t = 0.0f;
        float i = 1.0f / scaleDuration;

        while(t < 1.0f)
        {
            SetScale(Mathf.Lerp(a, s, LGGMath.EaseInOutSin(t)));
            t += (Time.deltaTime * i);
            yield return null;
        }

        SetScale(s);
    }

    public IEnumerator FadeRoutine(float f, float d)
    {
        float t = 0.0f;
        float i = 1.0f / d;
        float a = images[0].color.a;

        while(t < 1.0f)
        {
            SetAlpha(Mathf.Lerp(a, f, LGGMath.EaseInOutSin(t)));
            t += (Time.deltaTime * i);
            yield return null;
        }

        SetAlpha(f);
    }

    public IEnumerator DisplacementRoutine()
    {
        float t = 0.0f;
        float i = 1.0f / displacementDuration;

        while(t < 1.0f)
        {
            Debug.Log("[ScreenEffect] LerpDisplacement: " + t);
            LerpDisplacement(t);
            t += (Time.deltaTime * i);
            yield return null;
        }

        LerpDisplacement(1.0f);
    }
}

public class ScreenEffect : MonoBehaviour
{
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private ScreenEffectLayer[] layers;
    [SerializeField] private float waitBetweenLayers;
    private Coroutine fadeEffect;

    public void FadeIn(Action onFadeEnds = null)
    {
        //mainPanel.SetActive(true);
        this.StartCoroutine(ScreenEffectRoutine(onFadeEnds), ref fadeEffect);
    }

    public void FadeOut()
    {
        foreach(ScreenEffectLayer layer in layers)
        {
            layer.SetScale(0.0f);
            layer.LerpDisplacement(0.0f);
            layer.SetAlpha(0.0f);
        }

        //mainPanel.SetActive(false);
    }

    private IEnumerator ScreenEffectRoutine(Action onFadeEnds = null)
    {
        List<IEnumerator> routines = new List<IEnumerator>();
        float t = 0.0f;

        foreach(ScreenEffectLayer layer in layers)
        {
            routines.Clear();

            layer.LerpDisplacement(0.0f);

            /// Scaling:
            if(layer.scaleDuration > 0.0f)
            {
                layer.SetScale(0.0f);
                routines.Add(layer.ScaleRoutine(1.0f));
            }
            else
            {
                layer.SetScale(1.0f);
            }

            /// Fading:
            if(layer.fadeInDuration > 0.0f)
            {
                routines.Add(layer.FadeRoutine(layer.fadeInAlpha, layer.fadeInDuration));
            }
            else
            {
                layer.SetAlpha(1.0f);
            }
            
            /// Displacement:
            if(layer.displacementDuration > 0.0f)
            {
                routines.Add(layer.DisplacementRoutine());
            }

            IEnumerator routinesExcecution = LGGCoroutines.Routines(routines.ToArray());

            while(routinesExcecution.MoveNext()) yield return null;

            t = 0.0f;

            while(t < waitBetweenLayers)
            {
                t += Time.deltaTime;
                yield return null;
            }
        }

        if(onFadeEnds != null) onFadeEnds();
    }
}
}