using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LittleGuyGames
{
public class SelfMotionDisplacer : MonoBehaviour
{
    [SerializeField] private Transform child;       /// <summary>Child's Reference.</summary>
    [SerializeField] private float speed;           /// <summary>Displacement Speed.</summary>
    [SerializeField] private float magnitude;       /// <summary>Displacement Magnitude.</summary>
    private float time;

    /// <summary>Draws Gizmos on Editor mode when SelfMotionDisplacer's instance is selected.</summary>
    private void OnDrawGizmosSelected()
    {
        if(child == null) return;
        
        float r = 0.25f;
        Vector3 p = child.transform.position;
        Vector3 a = p + (Vector3.up * magnitude);
        Vector3 b = p + (Vector3.down * magnitude);

        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(p, a);
        Gizmos.DrawLine(p, b);
        Gizmos.DrawSphere(p + a, r);
        Gizmos.DrawSphere(p + b, r);
    }

    /// <summary>Updates SelfMotionDisplacer's instance at each frame.</summary>
    private void Update()
    {
        if(child == null) return;

        Vector3 p = Vector3.zero;
        p.y = Mathf.Sin(Time.time * speed) * magnitude;
        child.localPosition = p;
    }
}
}