using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairClamp : MonoBehaviour
{
    [SerializeField]
    float minimumAimPosition;

    [SerializeField]
    float maximumAimPosition;

    [SerializeField]
    float offset;
    

    private void FixedUpdate()
    {
        transform.localPosition = new Vector2(Mathf.Clamp(transform.localPosition.x, minimumAimPosition, maximumAimPosition), Mathf.Clamp(transform.localPosition.y, minimumAimPosition, maximumAimPosition));
        transform.localPosition.Normalize();
    }
}
