using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace LittleGuyGames
{
public static class LGGAudio
{
    public static void FadeOut(this AudioSource _source, float _duration = 0.3f, float _target = 0.0f, float _startingVolume = 1.0f)
    {
        if(_source.isPlaying && _source.volume > _target)
        {
            _source.volume -= (Time.deltaTime / _duration);
        }
        else
        {
            _source.Stop();
            _source.volume = _startingVolume;
        }
    }

    public static IEnumerator FadeOutVolumeRoutine(this AudioSource _source, float _duration = 0.3f, float _target = 0.0f, Action onFadeEnds = null)
    {
        float startingVolume = _source.volume;
        float i = 1.0f / _duration;

        while(_source.volume > _target)
        {
            _source.volume -= (startingVolume * Time.deltaTime * i);
            yield return null;
        }

        _source.Stop();
        _source.volume = startingVolume;
    }
}
}