using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour
{
    [SerializeField]
    GameObject[] _heartQuarters;

    [SerializeField]
    int _quarters = 4;
    int _maxQuarters;

    public bool HeartEmpty { get; private set; }

    [field: SerializeField]
    public bool HeartFull { get; private set; } = true;

    [field: SerializeField]
    public int QuarterToRestore { get; private set; } = -1;

    private void OnEnable()
    {
        _maxQuarters = _quarters;
    }

    public void DisableQuarters()
    {
        if (_quarters > 0)
        {
            Debug.LogFormat("Disabled Quarter {0}", QuarterToRestore);
            _heartQuarters[_quarters - 1]?.SetActive(false);
            _quarters--;
            HeartEmpty = _quarters <= 0;
            HeartFull = _quarters == 4;
            QuarterToRestore++;
        }
    }

    public void EnableQuarters(int quarterIndex)
    {
        if(quarterIndex < 4)
        {
            _quarters++;
            HeartEmpty = _quarters <= 0;
            HeartFull = _quarters >= 4;
            QuarterToRestore--;
            QuarterToRestore = Mathf.Clamp(QuarterToRestore, 0, 3);
            _heartQuarters[quarterIndex].SetActive(true);
            Debug.LogFormat("Enabled Quarter {0}", quarterIndex);
        }
    }
}
