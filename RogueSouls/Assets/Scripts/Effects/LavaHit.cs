using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaHit : PoolObject
{
    ParticleSystem mainParticle;

    // Start is called before the first frame update
    void OnEnable()
    {
        if (mainParticle == null)
        {
            mainParticle = GetComponent<ParticleSystem>();
        }
        mainParticle.Stop();
        Invoke("OnDeSpawn", mainParticle.main.duration);
    }
}
