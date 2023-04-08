using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LittleGuyGames
{

public class CameraStabilizer : MonoBehaviour
{
    public GameObject cameraToTrack;
    [SerializeField]
    private float rotationSpeedScalar;

    /// <summary>CameraStabilizer's instance initialization when loaded [Before scene loads].</summary>
    private void Awake()
    {

    }

    /// <summary>Stabilizes Rotation.</summary>
    public void Update()
    {
        if(cameraToTrack == null) return;

        transform.position = cameraToTrack.transform.position;
        transform.localRotation = Quaternion.Lerp(transform.localRotation, 
            Quaternion.Euler(cameraToTrack.transform.rotation.eulerAngles.x, cameraToTrack.transform.rotation.eulerAngles.y, 0f), Time.deltaTime * rotationSpeedScalar);
    }
}
}