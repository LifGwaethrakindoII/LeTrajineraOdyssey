using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/*============================================================
**
** Class:  UnityObjectBlackboard
**
** Purpose: This Blackboard class has dictionaries for
** Unity-Objects' references (in case you wanna have a
** Blackboard form GameObjects, Transforms, AudioClips, etc.).
**
**
** Author: Lîf Gwaethrakindo
**
==============================================================*/
namespace LittleGuyGames
{
[Serializable]
public class UnityObjectBlackboard : Blackboard
{
    [Space(5f)]
    [Header("Unity-Object's Data:")]
    [SerializeField] private StringGameObjectDictionary _gameObjectData;
    [SerializeField] private StringTransformDictionary _transformData;
    [SerializeField] private StringAudioClipDictionary _audioClipData;
    [SerializeField] private StringUnityObjectDictionary _unityObjectData;

    /// <summary>Gets and Sets gameObjectData property.</summary>
    public StringGameObjectDictionary gameObjectData
    {
        get
        {
            if(_gameObjectData == null) _gameObjectData = new StringGameObjectDictionary();
            return _gameObjectData;
        }
        private set { _gameObjectData = value; }
    }

    /// <summary>Gets and Sets transformData property.</summary>
    public StringTransformDictionary transformData
    {
        get
        {
            if(_transformData == null) _transformData = new StringTransformDictionary();
            return _transformData;
        }
        private set { _transformData = value; }
    }

    /// <summary>Gets and Sets audioClipData property.</summary>
    public StringAudioClipDictionary audioClipData
    {
        get
        {
            if(_audioClipData == null) _audioClipData = new StringAudioClipDictionary();
            return _audioClipData;
        }
        private set { _audioClipData = value; }
    }

    /// <summary>Gets and Sets unityObjectData property.</summary>
    public StringUnityObjectDictionary unityObjectData
    {
        get
        {
            if(_unityObjectData == null) _unityObjectData = new StringUnityObjectDictionary();
            return _unityObjectData;
        }
        private set { _unityObjectData = value; }
    }

    /// <summary>Sets GameObject Data into Blackboard.</summary>
    /// <param name="key">Data's Key.</param>
    /// <param name="value">New Data.</param>
    public void SetGameObjectData(string key, GameObject value)
    {
        if(gameObjectData.ContainsKey(key)) gameObjectData[key] = value;
        else gameObjectData.Add(key, value);
    }

    /// <summary>Gets Data associated with provided key.</summary>
    /// <param name="key">Data's Key.</param>
    public GameObject GetGameObjectData(string key, GameObject defaultValue = null)
    {
        return gameObjectData.ContainsKey(key) ? gameObjectData[key] : defaultValue;
    }

    /// <summary>Sets Transform Data into Blackboard.</summary>
    /// <param name="key">Data's Key.</param>
    /// <param name="value">New Data.</param>
    public void SetTransformData(string key, Transform value)
    {
        if(transformData.ContainsKey(key)) transformData[key] = value;
        else transformData.Add(key, value);
    }

    /// <summary>Gets Data associated with provided key.</summary>
    /// <param name="key">Data's Key.</param>
    public Transform GetTransformData(string key, Transform defaultValue = null)
    {
        return transformData.ContainsKey(key) ? transformData[key] : defaultValue;
    }

    /// <summary>Sets AudioClip Data into Blackboard.</summary>
    /// <param name="key">Data's Key.</param>
    /// <param name="value">New Data.</param>
    public void SetAudioClipData(string key, AudioClip value)
    {
        if(audioClipData.ContainsKey(key)) audioClipData[key] = value;
        else audioClipData.Add(key, value);
    }

    /// <summary>Gets Data associated with provided key.</summary>
    /// <param name="key">Data's Key.</param>
    public AudioClip GetAudioClipData(string key, AudioClip defaultValue = null)
    {
        return audioClipData.ContainsKey(key) ? audioClipData[key] : defaultValue;
    }

    /// <summary>Sets Unity-Object Data into Blackboard.</summary>
    /// <param name="key">Data's Key.</param>
    /// <param name="value">New Data.</param>
    public void SetObjectData(string key, UnityEngine.Object value)
    {
        if(unityObjectData.ContainsKey(key)) unityObjectData[key] = value;
        else unityObjectData.Add(key, value);
    }

    /// <summary>Gets Data associated with provided key.</summary>
    /// <param name="key">Data's Key.</param>
    public UnityEngine.Object GetObjectData(string key, UnityEngine.Object defaultValue = null)
    {
        return unityObjectData.ContainsKey(key) ? unityObjectData[key] : defaultValue;
    }
}
}