using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartDisplayHandler : MonoBehaviour
{
    [SerializeField]
    Heart heartDisplayObject;

    List<Heart> _heartsSpawned = new List<Heart>();

    EntityStats _entityStats;

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

    public void DecrementHeartQuarters(int healthToTakeAway)
    {
        for(int i = 0; i < healthToTakeAway; i++)
        {
            if (_heartsSpawned[_heartToIncrement]._heartEmpty)
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
}