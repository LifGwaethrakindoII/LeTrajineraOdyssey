using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LittleGuyGames
{
public enum PhysicsSensorMode
{
    Ray,
    Cube,
    Sphere,
    Capsule
}

public class PhysicsCastSensor : MonoBehaviour
{
    [SerializeField] private PhysicsSensorMode _mode;

    /// <summary>Gets and Sets mode property.</summary>
    public PhysicsSensorMode mode
    {
        get { return _mode; }
        set { _mode = value; }
    }
}
}