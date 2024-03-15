using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour
{
    [SerializeField]
    GameObject[] _heartQuarters;

    int _quarters = 4;
    int _maxQuarters;

    public bool HeartEmpty { get; private set; }
    public bool HeartFull { get; private set; } = true;

    [field: SerializeField]
    public int QuarterToRestore { get; private set; } = 0;

    private void OnEnable()
    {
        _maxQuarters = _quarters;
    }

    public void DisableQuarters()
    {
        if (_quarters >= 1)
        {
            _heartQuarters[_quarters - 1]?.SetActive(false);
            _quarters--;
            HeartEmpty = _quarters <= 0;
            HeartFull = _quarters == 4;
            QuarterToRestore++;
            QuarterToRestore = Mathf.Clamp(QuarterToRestore, 0, 3);
        }
    }

    public void EnableQuarters(int quarterIndex)
    {
        if(quarterIndex < 4)
        {
            HeartEmpty = _quarters <= 0;
            _quarters++;
            HeartFull = _quarters == 4;
            QuarterToRestore--;
            QuarterToRestore = Mathf.Clamp(QuarterToRestore, 0, 3);
            _heartQuarters[quarterIndex].SetActive(true);
            Debug.Log("Enabled Quarter: " + quarterIndex);
        }
    }
}
