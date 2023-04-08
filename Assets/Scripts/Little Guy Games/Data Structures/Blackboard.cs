using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/*============================================================
**
** Class:  Blackboard
**
** Purpose: Blackboard system. Contains a collection of
** KnowledgeSources that will interpret and alter the
** Blackboard when this updates.
**
** NOTE: Knowledge sources are updated in the order they are
** assigned (e.g., the first KnowledgeSource added will be
** the first one to update.
**
** Author: Lîf Gwaethrakindo
**
==============================================================*/
namespace LittleGuyGames
{
[Serializable]
public class Blackboard
{
    [SerializeField] private StringStringDictionary _stringData;
    [SerializeField] private StringIntDictionary _intData;
    [SerializeField] private StringFloatDictionary _floatData;
    [SerializeField] private StringVector2Dictionary _vector2Data;
    [SerializeField] private StringVector3Dictionary _vector3Data;
    [SerializeField] private StringVector4Dictionary _vector4Data;
    [SerializeField] private StringColorDictionary _colorData;
    [SerializeField] private StringBoolDictionary _boolData;
    private HashSet<KnowledgeSource> _knowledgeSources;

    /// <summary>Gets and Sets stringData property.</summary>
    public StringStringDictionary stringData
    {
        get
        {
            if(_stringData == null) _stringData = new StringStringDictionary();
            return _stringData;
        }
        private set { _stringData = value; }
    }

    /// <summary>Gets and Sets intData property.</summary>
    public StringIntDictionary intData
    {
        get
        {
            if(_intData == null) _intData = new StringIntDictionary();
            return _intData;
        }
        private set { _intData = value; }
    }

    /// <summary>Gets and Sets floatData property.</summary>
    public StringFloatDictionary floatData
    {
        get
        {
            if(_floatData == null) _floatData = new StringFloatDictionary();
            return _floatData;
        }
        private set { _floatData = value; }
    }

    /// <summary>Gets and Sets vector2Data property.</summary>
    public StringVector2Dictionary vector2Data
    {
        get
        {
            if(_vector2Data == null) _vector2Data = new StringVector2Dictionary();
            return _vector2Data;
        }
        private set { _vector2Data = value; }
    }

    /// <summary>Gets and Sets vector3Data property.</summary>
    public StringVector3Dictionary vector3Data
    {
        get
        {
            if(_vector3Data == null) _vector3Data = new StringVector3Dictionary();
            return _vector3Data;
        }
        private set { _vector3Data = value; }
    }

    /// <summary>Gets and Sets vector4Data property.</summary>
    public StringVector4Dictionary vector4Data
    {
        get
        {
            if(_vector4Data == null) _vector4Data = new StringVector4Dictionary();
            return _vector4Data;
        }
        private set { _vector4Data = value; }
    }

    /// <summary>Gets and Sets colorData property.</summary>
    public StringColorDictionary colorData
    {
        get
        {
            if(_colorData == null) _colorData = new StringColorDictionary();
            return _colorData;
        }
        private set { _colorData = value; }
    }

    /// <summary>Gets and Sets boolData property.</summary>
    public StringBoolDictionary boolData
    {
        get
        {
            if(_boolData == null) _boolData = new StringBoolDictionary();
            return _boolData;
        }
        private set { _boolData = value; }
    }

    /// <summary>Gets and Sets knowledgeSources property.</summary>
    public HashSet<KnowledgeSource> knowledgeSources
    {
        get
        {
            if(_knowledgeSources == null) _knowledgeSources = new HashSet<KnowledgeSource>();
            return _knowledgeSources;
        }
        private set { _knowledgeSources = value; }
    }

    /// <summary>Sets String Data into Blackboard.</summary>
    /// <param name="key">Data's Key.</param>
    /// <param name="value">New Data.</param>
    public void SetStringData(string key, string value)
    {
        if(stringData.ContainsKey(key)) stringData[key] = value;
        else stringData.Add(key, value);
    }

    /// <summary>Gets Data associated with provided key.</summary>
    /// <param name="key">Data's Key.</param>
    public string GetStringData(string key, string defaultValue = "")
    {
        return stringData.ContainsKey(key) ? stringData[key] : defaultValue;
    }

    /// <summary>Sets Int Data into Blackboard.</summary>
    /// <param name="key">Data's Key.</param>
    /// <param name="value">New Data.</param>
    public void SetIntData(string key, int value)
    {
        if(intData.ContainsKey(key)) intData[key] = value;
        else intData.Add(key, value);
    }

    /// <summary>Gets Data associated with provided key.</summary>
    /// <param name="key">Data's Key.</param>
    public int GetIntData(string key, int defaultValue = 0)
    {
        return intData.ContainsKey(key) ? intData[key] : defaultValue;
    }

    /// <summary>Sets Float Data into Blackboard.</summary>
    /// <param name="key">Data's Key.</param>
    /// <param name="value">New Data.</param>
    public void SetFloatData(string key, float value)
    {
        if(floatData.ContainsKey(key)) floatData[key] = value;
        else floatData.Add(key, value);
    }

    /// <summary>Gets Data associated with provided key.</summary>
    /// <param name="key">Data's Key.</param>
    public float GetFloatData(string key, float defaultValue = 0.0f)
    {
        return floatData.ContainsKey(key) ? floatData[key] : defaultValue;
    }

    /// <summary>Sets Vector2 Data into Blackboard.</summary>
    /// <param name="key">Data's Key.</param>
    /// <param name="value">New Data.</param>
    public void SetVector2Data(string key, Vector2 value)
    {
        if(vector2Data.ContainsKey(key)) vector2Data[key] = value;
        else vector2Data.Add(key, value);
    }

    /// <summary>Gets Data associated with provided key.</summary>
    /// <param name="key">Data's Key.</param>
    public Vector2 GetVector2Data(string key, Vector2 defaultValue = default(Vector2))
    {
        return vector2Data.ContainsKey(key) ? vector2Data[key] : defaultValue;
    }

    /// <summary>Sets Vector3 Data into Blackboard.</summary>
    /// <param name="key">Data's Key.</param>
    /// <param name="value">New Data.</param>
    public void SetVector3Data(string key, Vector3 value)
    {
        if(vector3Data.ContainsKey(key)) vector3Data[key] = value;
        else vector3Data.Add(key, value);
    }

    /// <summary>Gets Data associated with provided key.</summary>
    /// <param name="key">Data's Key.</param>
    public Vector3 GetVector3Data(string key, Vector3 defaultValue = default(Vector3))
    {
        return vector3Data.ContainsKey(key) ? vector3Data[key] : defaultValue;
    }

    /// <summary>Sets Vector4 Data into Blackboard.</summary>
    /// <param name="key">Data's Key.</param>
    /// <param name="value">New Data.</param>
    public void SetVector4Data(string key, Vector4 value)
    {
        if(vector4Data.ContainsKey(key)) vector4Data[key] = value;
        else vector4Data.Add(key, value);
    }

    /// <summary>Gets Data associated with provided key.</summary>
    /// <param name="key">Data's Key.</param>
    public Vector4 GetVector4Data(string key, Vector4 defaultValue = default(Vector4))
    {
        return vector4Data.ContainsKey(key) ? vector4Data[key] : defaultValue;
    }

    /// <summary>Sets Color Data into Blackboard.</summary>
    /// <param name="key">Data's Key.</param>
    /// <param name="value">New Data.</param>
    public void SetColorData(string key, Color value)
    {
        if(colorData.ContainsKey(key)) colorData[key] = value;
        else colorData.Add(key, value);
    }

    /// <summary>Gets Data associated with provided key.</summary>
    /// <param name="key">Data's Key.</param>
    public Color GetColorData(string key, Color defaultValue = default(Color))
    {
        return colorData.ContainsKey(key) ? colorData[key] : defaultValue;
    }

    /// <summary>Sets Bool Data into Blackboard.</summary>
    /// <param name="key">Data's Key.</param>
    /// <param name="value">New Data.</param>
    public void SetData(string key, bool value)
    {
        if(boolData.ContainsKey(key)) boolData[key] = value;
        else boolData.Add(key, value);
    }

    /// <summary>Gets Data associated with provided key.</summary>
    /// <param name="key">Data's Key.</param>
    public bool GetData(string key, bool defaultValue = false)
    {
        return boolData.ContainsKey(key) ? boolData[key] : defaultValue;
    }

    /// <summary>Adds KnowledgeSource.</summary>
    /// <param name="_knowledgeSource">KnowledgeSource to add.</param>
    public void AddKnowledgeSource(KnowledgeSource _knowledgeSource)
    {
        knowledgeSources.Add(_knowledgeSource);
    }

    /// <summary>Removes KnowledgeSource.</summary>
    /// <param name="_knowledgeSource">KnowledgeSource to remove.</param>
    public void RemoveKnowledgeSource(KnowledgeSource _knowledgeSource)
    {
        knowledgeSources.Remove(_knowledgeSource);
    }

    /// <summary>updates Blackboard.</summary>
    public void Update()
    {
        foreach(KnowledgeSource knowledgeSource in knowledgeSources)
        {
            knowledgeSource.Update(this);
        }
    }
}
}