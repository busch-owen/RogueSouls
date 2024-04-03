using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairClamp : MonoBehaviour
{
    [SerializeField]
    float aimClampX;

    [SerializeField]
    float aimClampY;

    private void FixedUpdate()
    {
        //transform.localPosition = new Vector2(Mathf.Clamp(transform.localPosition.x, minimumAimPosition, maximumAimPosition), Mathf.Clamp(transform.localPosition.y, minimumAimPosition, maximumAimPosition));
        //transform.localPosition.Normalize();
    }

    public void ClampCrosshair(Vector2 input)
    {
        transform.localPosition = new Vector2(Mathf.Clamp(transform.localPosition.x, aimClampX * input.x, aimClampX * input.x), Mathf.Clamp(transform.localPosition.y, aimClampY * input.y, aimClampY * input.y));
    }
}
