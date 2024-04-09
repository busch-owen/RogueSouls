using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour
{
    [field: SerializeField]
    public GameObject[] _heartQuarters {  get; private set; }

    [SerializeField]
    int _quarters = 4;
    int _maxQuarters;

    EntityStats _playerEntityStats;

    private void OnEnable()
    {
        _maxQuarters = _quarters;
        _playerEntityStats = GetComponentInParent<EntityStats>();
    }
}
