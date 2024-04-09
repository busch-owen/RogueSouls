using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartDisplayHandler : MonoBehaviour
{
    [SerializeField]
    Heart heartDisplayObject;

    [SerializeField]
    List<Heart> _heartsSpawned = new List<Heart>();

    [SerializeField]
    EntityStats _entityStats;

    int _heartToIncrement = 0;
    int _heartQuarterToIncrement = 3;

    private void Start()
    {
        _entityStats = GetComponentInParent<EntityStats>();
        CreateHeartsForAmountOfHearts();
        CheckHeartQuarters();
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
        Heart heartSpawned = Instantiate(heartDisplayObject, this.transform);
        _heartsSpawned.Add(heartSpawned);
    }

    public void CheckHeartQuarters()
    {
        for(int i = 0; i < _heartsSpawned.Count; i++)
        {
            foreach(GameObject quarter in _heartsSpawned[i]._heartQuarters)
            {
                quarter.SetActive(false);
                Debug.Log("disabling all quarters");
            }
        }
        for (int i = 0; i < _entityStats.Health; i++)
        {
            _heartsSpawned[_heartToIncrement]._heartQuarters[_heartQuarterToIncrement].SetActive(true);
            _heartQuarterToIncrement--;
            if (_heartQuarterToIncrement < 0)
            {
                _heartQuarterToIncrement = 3;
                _heartToIncrement++;
            }
            Debug.LogFormat("re enabled quarter {0} in heart {1}", _heartQuarterToIncrement + 1, _heartToIncrement);
            _heartQuarterToIncrement = Mathf.Clamp(_heartQuarterToIncrement, 0, 3);
            _heartToIncrement = Mathf.Clamp(_heartToIncrement, 0, _entityStats.AmountOfHearts - 1);

            /*for(int j = 0; j < _heartsSpawned.Count; j++)
            {
                for (int k = _heartsSpawned[j]._heartQuarters.Length - 1; k > -1; k--)
                {
                    _heartsSpawned[j]._heartQuarters[k].SetActive(true);
                    
                }
            }*/
        }
    }
}