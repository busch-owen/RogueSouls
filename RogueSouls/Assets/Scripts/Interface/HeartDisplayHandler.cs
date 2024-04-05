using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartDisplayHandler : MonoBehaviour
{
    [SerializeField]
    Heart heartDisplayObject;

    List<Heart> _heartsSpawned = new List<Heart>();

    EntityStats _entityStats;

    [SerializeField]
    int _heartToIncrement;

    private void Start()
    {
        _entityStats = GetComponentInParent<EntityStats>();
        CreateHeartsForAmountOfHearts();

    }

    private void CreateHeartsForAmountOfHearts()
    {
        for(int i = 0; i < _entityStats?.AmountOfHearts; i++)
        {
            AddOneHeart();
        }
    }

    public void AddOneHeart()
    {
        _heartToIncrement = _heartsSpawned.Count;
        Heart heartSpawned = Instantiate(heartDisplayObject, this.transform);
        _heartsSpawned.Add(heartSpawned);
    }

    public void DecreaseHeartQuarters(int healthToTakeAway)
    {
        for(int i = 0; i < healthToTakeAway; i++)
        {
            if (_heartsSpawned[_heartToIncrement].HeartEmpty)
            {
                _heartToIncrement--;
                _heartToIncrement = Mathf.Clamp(_heartToIncrement, 0, _heartsSpawned.Count);
                _heartsSpawned[_heartToIncrement].DisableQuarters();
            }
            else
            {
                _heartsSpawned[_heartToIncrement].DisableQuarters();
            }
        }    
    }

    public void IncreaseHeartQuarters(int healthToRestore)
    {
        if (_entityStats.AtFullHealth())
        {
            return;
        }
        for (int i = 0; i < healthToRestore; i++)
        {
            if (_heartsSpawned[_heartToIncrement].HeartFull && _heartToIncrement < _entityStats.AmountOfHearts - 1)
            {
                _heartToIncrement++;
                _heartToIncrement = Mathf.Clamp(_heartToIncrement, 0, 3);
            }
            else
            {
                _heartsSpawned[_heartToIncrement].EnableQuarters(_heartsSpawned[_heartToIncrement].QuarterToRestore);
            }
        }
    }
}