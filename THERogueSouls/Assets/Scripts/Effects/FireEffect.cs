using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireEffect : PoolObject
{
    ParticleSystem mainParticle;

    // Start is called before the first frame update
    void OnEnable()
    {
        if (mainParticle == null)
        {
            mainParticle = GetComponent<ParticleSystem>();
        }
        Invoke("OnDeSpawn", mainParticle.main.duration);
    }
}
