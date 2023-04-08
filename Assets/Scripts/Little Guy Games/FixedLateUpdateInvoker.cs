using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LittleGuyGames
{
/// <summary>Event invoked on Late Fixed Update.</summary>
public delegate void OnFixedLateUpdate();

public class FixedLateUpdateInvoker : MonoBehaviour
{
    public static event OnFixedLateUpdate onFixedLateUpdate;

    private int currentFrame;
    private float lateFixedTime;

    /// <summary>FixedLateUpdateInvoker's instance initialization when loaded [Before scene loads].</summary>
    private void Awake()
    {
        currentFrame = -1;
        lateFixedTime = 0.0f;
    }

    /// <summary>Callback invoked when scene loads, one frame before the first Update's tick.</summary>
    private void Start()
    {
        lateFixedTime = Time.fixedTime;
    }

    /// <summary>Updates FixedLateUpdateInvoker's instance at each frame.</summary>
    private void Update()
    {
        if(Time.fixedTime > lateFixedTime || Time.fixedTime == 0.0f) InvokeFixedLateUpdate();
        lateFixedTime = Time.fixedTime;
    }

    /// <summary>Updates FixedLateUpdateInvoker's instance at each Physics Thread's frame.</summary>
    private void FixedUpdate()
    {
        if(currentFrame == Time.frameCount) InvokeFixedLateUpdate();
        currentFrame = Time.frameCount;
    }

    /// <summary>Invokes Late Fixed Update's Event.</summary>
    private void InvokeFixedLateUpdate()
    {
        lateFixedTime += Time.fixedDeltaTime;
        if(onFixedLateUpdate != null) onFixedLateUpdate();
    }
}
}