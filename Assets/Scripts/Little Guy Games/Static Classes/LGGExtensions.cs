using System.Collections;
using System;
using System.Text;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Unity.XR.CoreUtils;


/*============================================================
**
** Class:  LGGExtensions
**
** Purpose: Static class that contains many utility functions.
**
**
** Author: Lîf Gwaethrakindo
**
==============================================================*/
namespace LittleGuyGames
{
[Flags]
public enum Axes2D
{
    None = 0,
    X = 1,
    Y = 2,
    All = X | Y 
}

[Flags]
public enum Axes3D
{
    None = 0,
    X = 1,
    Y = 2,
    Z = 4,
    XAndY = X | Y,
    XAndZ = X | Z,
    YAndZ = Y | Z,
    All = X | Y | Z
}

[Flags]
public enum TransformProperties
{
    None = 0,
    Position = 1,
    Rotation = 2,
    Scale = 4,

    PositionAndRotation = Position | Rotation,
    PositionAndScale = Position | Scale,
    RotationAndScale = Rotation | Scale,
    All = Position | Rotation | Scale
}

public enum SpeedChange
{
    Acceleration,
    Linear
}

public static class LGGExtensions
{
    public static readonly int HASH_ANIMATION_EMPTY;                /// <summary>Empty's Animation Hash.</summary>

    /// <summary>Static Constructor.</summary>
    static LGGExtensions()
    {
        HASH_ANIMATION_EMPTY = Animator.StringToHash("Empty");
    }

    /// <summary>Applies transform properties of Transform B into Transform A.</summary>
    /// <param name="a">Transform A.</param>
    /// <param name="b">Transform B.</param>
    /// <param name="p">Transform Properties to apply [TransformProperties.PositionAndRotation by default].</param>
    public static void CopyTransformProperties(this Transform a, Transform b, TransformProperties p = TransformProperties.PositionAndRotation)
    {
        if(a == null || b == null || p == TransformProperties.None) return;

        if((p | TransformProperties.Position) == p) a.position = b.position;
        if((p | TransformProperties.Rotation) == p) a.rotation = b.rotation;
        if((p | TransformProperties.Scale) == p) a.localScale = b.localScale;
    }

    /// <summary>Plays Sound on AudioSource.</summary>
    /// <param name="_source">AudioSource's reference.</param>
    /// <param name="_clip">Clip to play.</param>
    /// <param name="_loop">Loop? true by default.</param>
    public static void Play(this AudioSource _source, AudioClip _clip, bool _loop = true)
    {
        if(_clip == null) return;

        _source.clip = _clip;
        _source.loop = _loop;
        _source.Play();
    }

    /// <summary>Destroys Object regardless of editor or play mode.</summary>
    /// <param name="_object">Object to destroy.</param>
    public static void Destroy(this UnityEngine.Object _object)
    {
        if(Application.isPlaying) UnityEngine.Object.Destroy(_object);
        else UnityEngine.Object.DestroyImmediate(_object);
    }

    /// <summary>Sets Layers recursively.</summary>
    /// <param name="obj">GameObject's reference.</param>
    /// <param name="oldLayer">Old Layer.</param>
    /// <param name="newLayer">New Layer.</param>
    public static void SetLayerRecursive(this GameObject obj, int oldLayer, int newLayer)
    {
        foreach(Transform child in obj.transform)
        {
            if(child.gameObject.layer == oldLayer)
            child.gameObject.layer = newLayer;

            child.gameObject.SetLayerRecursive(oldLayer, newLayer);
        }
    }

    /// <summary>Sets Layers recursively.</summary>
    /// <param name="obj">GameObject's reference.</param>
    /// <param name="layer">New Layer.</param>
    public static void SetLayerRecursive(this GameObject obj, int layer)
    {
        obj.SetLayerRecursive(obj.layer, layer);
    }

    /// <param name="_rigidbody">Rigidbody's Reference.</param>
    /// <returns>Local Velocity from Rigidbody.</returns>
    public static Vector3 GetLocalVelocity(this Rigidbody _rigidbody)
    {
        return _rigidbody.transform.InverseTransformDirection(_rigidbody.velocity);
    }

    /// <param name="_rigidbody">Rigidbody's Reference.</param>
    /// <returns>Local Angular Velocity from Rigidbody.</returns>
    public static Vector3 GetLocalAngularVelocity(this Rigidbody _rigidbody)
    {
        return _rigidbody.transform.InverseTransformDirection(_rigidbody.angularVelocity);
    }

    

    /// <summary>Evaluates if any of the given Vector's components is NaN.</summary>
    /// <param name="v">Vector to evaluate.</param>
    /// <returns>True if any of the vector components is NaN.</returns>
    public static bool IsNaN(this Vector3 v)
    {
        if(float.IsNaN(v.x)) return true;
        if(float.IsNaN(v.y)) return true;
        if(float.IsNaN(v.z)) return true;
        return false;
    }

    /// <summary>Evaluates if a vector has any NaN component, if it does it returns a given default vector.</summary>
    /// <param name="v">Vector to evaluate.</param>
    /// <param name="defaultVector">Default vector to return if given vector has any NaN component.</param>
    /// <returns>Filtered Vector.</returns>
    public static Vector3 NaNFilter(this Vector3 v, Vector3 defaultVector = default(Vector3))
    {
        return v.IsNaN() ? defaultVector : v;
    }

    /// <summary>Gets a rotation where the forward vector would be the vector oriented towards the direction.</summary>
    /// <param name="d">Turn direction.</param>
    /// <param name="up">Upwards vector's reference.</param>
    /// <returns>Rotation for the forward vector to be oriented.</returns>
    public static Quaternion LookRotation(Vector3 d, Vector3 up)
    {
        return Quaternion.LookRotation(d, d.x >= 0.0f ? up : -up);
    }

    /// <param name="_rigidbody">Rigidbody's reference.</param>
    /// <returns>String representing referenced Rigidbody.</returns>
    public static string RigidbodyToString(this Rigidbody _rigidbody)
    {
        StringBuilder builder = new StringBuilder();

        builder.AppendLine("Rigidbody: \n{");
        builder.Append("\tMass = ");
        builder.AppendLine(_rigidbody.mass.ToString());
        builder.Append("\tDrag = ");
        builder.AppendLine(_rigidbody.drag.ToString());
        builder.Append("\tAngular Drag = ");
        builder.AppendLine(_rigidbody.angularDrag.ToString());
        builder.Append("\tUse Gravity = ");
        builder.AppendLine(_rigidbody.useGravity.ToString());
        builder.Append("\tVelocity = ");
        builder.AppendLine(_rigidbody.velocity.ToString());
        builder.Append("\tAngular Velocity = ");
        builder.AppendLine(_rigidbody.angularVelocity.ToString());
        builder.Append("}");

        return builder.ToString();
    }

    /// <summary>Serializes given item into JSON format to a file located at provided path.</summary>
    /// <param name="_item">Item to serialize.</param>
    /// <param name="_path">Path to serialize the JSON's content.</param>
    /// <param name="_prettyPrint">If true, format the output for readability. If false, format the output for minimum size. Default is false.</param>
    public static void SerializeToJSON<T>(this T _item, string _path, bool _prettyPrint = false)
    {
        string json = JsonUtility.ToJson(_item, _prettyPrint);
        try { File.WriteAllText(_path, json); }
        catch(Exception exception) { Debug.LogWarning("[LGGExtensions] Catched Exception while trying to serialize to JSON: " + exception.Message ); }
    }

    /// <summary>Deserializes JSON content from file located at provided path.</summary>
    /// <param name="_path">Path where the JSON should be located.</param>
    /// <returns>Deserialized item from JSON's content, if such exists.</returns>
    public static T DeserializeFromJSONFromPath<T>(string _path)
    {
        T item = default(T);
        string json = null;

        try
        {
            json = File.ReadAllText(_path);
            item = JsonUtility.FromJson<T>(json);
        }
        catch(Exception exception) { Debug.LogWarning("[LGGExtensions] Catched Exception while trying to deserialize object of type " + typeof(T) + " : " + exception.Message); }

        return item;
    }

    public static bool InsideLayerMask(int layer, LayerMask mask)
    {
        return (mask | 1 << layer) == mask;
    }

    /// <summary>Performs an action to for each element passed. This method is to avoid lazy array initializations.</summary>
    /// <param name="action">Action to perform to each element of array.</param>
    /// <param name="_array">Array of elements of type T.</param>
    public static void ForEach<T>(Action<T> action, params T[] _array)
    {
        if(action == null) return;

        foreach(T element in _array)
        {
            if(element != null) action(element);
        }
    }

    /// <summary>Evaluates if GameObject has any of the provided tags' array.</summary>
    /// <param name="obj">GameObject to evaluate.</param>
    /// <param name="tags">Tags to evaluate.</param>
    /// <returns>True whether GameObject has any of the tags, false otherwise.</returns>
    public static bool HasAnyOfTags(this GameObject obj, params string[] tags)
    {
        if(tags == null) return false;

        foreach(string tag in tags)
        {
            if(obj.CompareTag(tag)) return true;
        }

        return false;
    }
}
}

/*
    Rubberbanding idea:
    If inverse square law: 1 / d^2
    Then the inverse: d^2

    float Rubberband(x, y, ref v, a, dt, r)
    {
        d = y - x;
        s = Sign(s);
        e = d / r; // If radius 2.5 and d 5 then 5 / 2.5 = 2
    }
*/