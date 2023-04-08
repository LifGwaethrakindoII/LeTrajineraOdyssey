using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using LittleGuyGames.XR;

namespace LittleGuyGames
{
public class WorldSpaceTextGUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textMesh;         /// <summary>TextMeshPro's reference.</summary>
    [SerializeField] private UnityEngine.Object _reference;     /// <summary>Reference Object.</summary>

    /// <summary>Gets textMesh property.</summary>
    public TextMeshProUGUI textMesh { get { return _textMesh; } }

    /// <summary>Gets reference property.</summary>
    public UnityEngine.Object reference { get { return _reference; } }

    /// <summary>Updates WorldSpaceTextGUI's instance at each frame.</summary>
    private void Update()
    {
        if(textMesh == null || reference == null) return;
    
        textMesh.text = ((GameObject)reference).GetComponent<XRJoystick>().ToString();
    }
}
}