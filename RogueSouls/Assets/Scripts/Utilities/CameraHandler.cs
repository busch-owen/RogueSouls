using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    CinemachineConfiner2D _confiner;

    [SerializeField]
    PolygonCollider2D _defaultCameraBounds;

    private void Awake()
    {
        _confiner = GetComponent<CinemachineConfiner2D>();
        _confiner.m_BoundingShape2D = _defaultCameraBounds;
    }
}
