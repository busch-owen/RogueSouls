using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolObject : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnDeSpawn()
    {
        PoolManager.Instance.DeSpawn(this); //if we are told to despawn, talk to the pool manager and despawn
    }
}
