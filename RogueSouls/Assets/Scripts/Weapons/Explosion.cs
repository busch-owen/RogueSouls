using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : PoolObject
{
    [SerializeField]
    float timeActive = 2f;
    // Start is called before the first frame update
    public void OnEnable()
    {
        Invoke("OnDeSpawn", timeActive);
    }

}
