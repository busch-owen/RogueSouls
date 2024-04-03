using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour
{
    [SerializeField]
    GameObject[] _heartQuarters;

    int _quarters = 4;
    int _maxQuarters;

    private void OnEnable()
    {
        _maxQuarters = _quarters;
    }

    public bool _heartEmpty { get; private set; }

    public void DisableQuarters()
    {
        _heartQuarters[_quarters - 1].SetActive(false);
        _quarters--;
        _heartEmpty = _quarters <= 0;
    }

    public void EnableQuarters(int quarterIndex)
    {
        int index = quarterIndex - 1;
        _heartQuarters[index].SetActive(true);
        _heartEmpty = false;
    }
}
