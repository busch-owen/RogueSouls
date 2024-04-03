using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeHandler : MonoBehaviour
{
    [SerializeField]
    float fixedTimeStep;

    // Update is called once per frame
    void Update()
    {
        Time.fixedDeltaTime = fixedTimeStep * Time.timeScale;
    }
}
