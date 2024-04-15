using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetDoor : Door
{
    [SerializeField]
    private TargetSwitch[] targetsToHit;
    int targetsHit = 0;
    [SerializeField]
    float secondsToClose =5.0f;

    public void CheckTargets(TargetSwitch targetToCheck)
    {

        if (targetToCheck.IsTriggered)
        {
            targetsHit++;
        }
        if (targetsHit == targetsToHit.Length)
        {
            base.OpenDoor();

            Invoke("CloseAfterDuration", secondsToClose);
        }

    }

    private void FixedUpdate()
    {

    }
    public void CloseAfterDuration()
    {
        base.CloseDoor();
        foreach (TargetSwitch target in targetsToHit)
        {
            if(target.IsTriggered)
            {
                target.ResetSwitch();
            }
        }
        targetsHit = 0;
    }
    public void DecreaseIndex()
    {
        targetsHit--;
    }



}
