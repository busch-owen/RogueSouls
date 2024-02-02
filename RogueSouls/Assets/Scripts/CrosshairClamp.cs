using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairClamp : MonoBehaviour
{
    [SerializeField]
    float minimumAimPosition;

    [SerializeField]
    float maximumAimPosition;
    

    private void FixedUpdate()
    {
        
        //transform.localPosition.Normalize();
    }

    public void LockCrosshairPosition(Vector2 inputPos)
    {
        transform.localPosition = new Vector2(Mathf.Clamp(transform.localPosition.x * inputPos.x, minimumAimPosition, maximumAimPosition), Mathf.Clamp(transform.localPosition.y * inputPos.y, minimumAimPosition, maximumAimPosition));
    }
}
