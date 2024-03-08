using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartDisplayHandler : MonoBehaviour
{
    [SerializeField]
    Heart heartDisplayObject;

    List<Heart> heartsSpawned = new List<Heart>();

    PlayerStats playerStats;

    private void OnEnable()
    {
        playerStats = FindObjectOfType<PlayerStats>();
        CreateHeartsForAmountOfHearts();
    }

    private void CreateHeartsForAmountOfHearts()
    {
        for(int i = 0; i < playerStats.AmountOfHearts; i++)
        {
            AddOneHeart();
        }
    }

    public void AddOneHeart()
    {
        Heart heartSpawned = Instantiate(heartDisplayObject, this.transform);
        heartsSpawned.Add(heartSpawned);
    }
}