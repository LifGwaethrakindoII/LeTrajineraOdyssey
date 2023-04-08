using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LittleGuyGames
{
[RequireComponent(typeof(LineRenderer))]
public class InGameVisualDebugger : MonoBehaviour
{
    private LineRenderer _lineRenderer;     /// <summary>LineRenderer's Component.</summary>

    /// <summary>Gets lineRenderer Component.</summary>
    public LineRenderer lineRenderer
    { 
        get
        {
            if(_lineRenderer == null) _lineRenderer = GetComponent<LineRenderer>();
            return _lineRenderer;
        }
    }

    /// <summary>Enables Component.</summary>
    /// <param name="_enable">Enable? true by default.</param>
    public void Enable(bool _enable = true)
    {
        gameObject.SetActive(_enable);
    }

    /// <summary>Draws Ray.</summary>
    /// <param name="o">Ray's Origin.</param>
    /// <param name="d">Ray's Direction.</param>
    public void DrawRay(Vector3 o, Vector3 d)
    {
        lineRenderer.positionCount = 2;
        transform.position = o;
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, transform.position + d);
    }
}
}