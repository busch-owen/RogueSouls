using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffectHandler : MonoBehaviour
{
    [SerializeField]
    ParticleSystem _runParticleSystem;

    public void PlayRunParticles()
    {
        _runParticleSystem.Play();
    }

    public void StopRunParticles()
    {
        _runParticleSystem.Stop();
    }

    public bool RunParticlesPlaying()
    {
        return _runParticleSystem.isPlaying;
    }
}
