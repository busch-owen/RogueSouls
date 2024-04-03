using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    Transform target;

    [SerializeField]
    float smoothSpeed;

    private void FixedUpdate()
    {
        transform.position = Vector2.Lerp(transform.position, target.position, smoothSpeed * Time.fixedDeltaTime);
    }
}
