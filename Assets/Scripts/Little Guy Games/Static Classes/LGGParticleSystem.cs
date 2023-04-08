using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using WingsOfIcaria;

namespace LittleGuyGames
{
public static class LGGParticleSystem
{
    public static float CalculateTimeToReachForceField(Transform particleSystem, ParticleSystemForceField forceField, float gravity = 9.81f)
    {
        // Get the current position of the particle system
        Vector3 p0 = particleSystem.transform.position;

        // Get the position of the force field
        Vector3 pf = forceField.transform.position;

        // Calculate the initial velocity of the particle system
        Vector3 v0 = (pf - p0) / Time.deltaTime;

        // Get the gravity vector of the force field
        Vector3 g = (p0 - pf);

        // Calculate the acceleration of the particle system
        Vector3 d = g.normalized;
        float m = forceField.gravity.constant * gravity;
        float a = m * d.y;

        // Calculate the time it will take for the particle system to reach the force field
        float t = -v0.y / a;

        return t;
    }
}
}