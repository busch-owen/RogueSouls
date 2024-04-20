using System;
using Cinemachine;
using UnityEngine;

public class RespawnHandler : MonoBehaviour
{
    [SerializeField]
    Transform _respawnPoint;

    PlayerStats _playerStats;

    private CinemachineConfiner2D _confiner;

    [SerializeField]
    PolygonCollider2D _newCameraBounds;

    private void Awake()
    {
        _confiner = FindObjectOfType<CinemachineConfiner2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.GetComponent<PlayerController>()) return;
        
        _confiner.m_BoundingShape2D = _newCameraBounds;
        _playerStats = other.gameObject.GetComponent<PlayerStats>();
        _playerStats.ChangeRespawnPoint(_respawnPoint);
        _playerStats.SetRespawnCameraBounds(_newCameraBounds);
    }
}
