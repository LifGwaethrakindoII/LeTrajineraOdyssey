using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace LittleGuyGames
{
public class ParticleSystemsContainer : MonoBehaviour
{
    [SerializeField] private StringParticleSystemDictionary _particleSystemsMap;
    [SerializeField] private ParticleSystem[] _particleSystems;

    /// <summary>Gets and Sets particleSystemsMap property.</summary>
    public StringParticleSystemDictionary particleSystemsMap
    {
        get { return _particleSystemsMap; }
        protected set { _particleSystemsMap = value; }
    }

    /// <summary>Gets and Sets particleSystems property.</summary>
    public ParticleSystem[] particleSystems
    {
        get { return _particleSystems; }
        protected set { _particleSystems = value; }
    }

    /// <summary>Resets ParticleSystemsContainer's instance to its default values.</summary>
    private void Reset()
    {
        GetAllSelfContainedParticleSystems();
    }

    /// <summary>Plays all the PArticle Systems.</summary>
    /// <param name="_withChildren">Pause also the children contained? [true as default].</param>
    public void Play(bool _withChildren = true)
    {
        if(particleSystems == null) return;
        if(!gameObject.activeSelf) gameObject.SetActive(true);

        foreach(ParticleSystem system in particleSystems)
        {
            try
            {
                system.Play(_withChildren);
            }
            catch(Exception e)
            {
                Debug.LogError(gameObject.name + ": " + e.Message, gameObject);
            }
        }
    }

    /// <summary>Simulates the Particle Systems.</summary>
    /// <param name="_withChildren">Pause also the children contained? [true as default].</param>
    /// <param name="t">Time period to advance this simulation by [0.0f by default].</param>
    public void Simulate(bool _withChildren = true, float t = 0.0f)
    {
        if(particleSystems == null) return;
        if(!gameObject.activeSelf) gameObject.SetActive(true);
        
        foreach(ParticleSystem system in particleSystems)
        {
            system.Simulate(0.0f, _withChildren);
        }

    }

    /// <summary>Pauses the Particle Systems.</summary>
    /// <param name="_withChildren">Pause also the children contained? [true as default].</param>
    public void Pause(bool _withChildren = true)
    {
        if(!gameObject.activeSelf) gameObject.SetActive(true);
        if(particleSystems == null) return;
        
        foreach(ParticleSystem system in particleSystems)
        {
            system.Pause(_withChildren);
        }
    }

    /// <summary>Stops the Particle Systems.</summary>
    /// <param name="_withChildren">Pause also the children contained? [true as default].</param>
    public void Stop(bool _withChildren = true)
    {
        if(particleSystems == null) return;
        
        foreach(ParticleSystem system in particleSystems)
        {
            try
            {
                system.Stop(_withChildren);
            }
            catch(Exception e)
            {
                Debug.LogError(gameObject.name + ": " + e.Message, gameObject);
            }
        }
    }

    /// <summary>Clears the Particle Systems.</summary>
    /// <param name="_withChildren">Clear also the children contained? [true as default].</param>
    public void Clear(bool _withChildren = true)
    {
        if(particleSystems == null) return;
        
        foreach(ParticleSystem system in particleSystems)
        {
            if(system != null) system.Clear(_withChildren);
        }
    }

    /*public void SetVelocity(Vector3 velocity)
    {
        
    }*/

    [Button("Get Particle-Systems")]
    /// <summary>Retreives self-contained ParticleSystems inside this GameObject.</summary>
    private void GetAllSelfContainedParticleSystems()
    {
        ParticleSystem[] systems = GetComponentsInChildren<ParticleSystem>();

        if(systems == null) return;

        /*particleSystems = new StringParticleSystemDictionary();

        foreach(ParticleSystem system in systems)
        {
            particleSystems.Add(system.gameObject.name, system);
        }*/

        particleSystems = systems;
    }
}
}