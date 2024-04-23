using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSoul : MonoBehaviour
{
    HUD _targetHUD;

    [SerializeField]
    string _collectionMessage;

    [SerializeField]
    float _messageDuration;

    [SerializeField]
    bool _isMajorSoul;

    private GameManager _gameManager;

    private void Awake()
    {
        if (!_gameManager)
        {
            _gameManager = FindObjectOfType<GameManager>();
        }
    }

    private void OnEnable()
    {
        _targetHUD = FindObjectOfType<HUD>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.GetComponent<PlayerStats>())
        {
            PlayerStats targetPlayer = other.GetComponent<PlayerStats>();
            targetPlayer.IncreaseHealth(99);
            if(_isMajorSoul)
            {
                targetPlayer.IncreaseMajorSoulCount();
                if(_gameManager)
                    _gameManager.CheckForHealthSouls(targetPlayer.MajorSoulsCollected);
            }
            else
            {
                targetPlayer.IncreaseMinorSoulCount();
            }
            Destroy(gameObject);

            _targetHUD.ShowSpecificMessageOnTextBox(_collectionMessage, _messageDuration);
        }
    }
}
