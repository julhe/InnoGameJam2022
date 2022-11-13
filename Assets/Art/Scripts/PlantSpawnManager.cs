using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlantSpawnManager : MonoBehaviour
{
    public static PlantSpawnManager Instance;
    [SerializeField] int lowerTierChance = 33;
    [SerializeField] int sameTierChance = 33;

    [SerializeField] List<GameObject> tier0Plants = new List<GameObject>();
    [SerializeField] List<GameObject> tier1Plants = new List<GameObject>();
    [SerializeField] List<GameObject> tier2Plants = new List<GameObject>();

    List<List<GameObject>> tierList = new List<List<GameObject>>();

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        tierList.Add(tier0Plants);
        tierList.Add(tier1Plants);
        tierList.Add(tier2Plants);
    }

    public GameObject GetPlantToSpawn(GameObject parentPlant)
    {
        int plantToSpawnTier = getPlantTier(parentPlant);
        int randomInt = Random.Range(0, 100);
        
        //spawn lower tier plant
        if(randomInt < lowerTierChance)
        {
            if(plantToSpawnTier > 0)
            plantToSpawnTier --;
        }

        //Spawn higher tier plant
        else if(randomInt > lowerTierChance + sameTierChance)
        {
            if(plantToSpawnTier < tierList.Count - 1)
            plantToSpawnTier ++;
        }

        return tierList[plantToSpawnTier][Random.Range(0, tierList[plantToSpawnTier].Count - 1)];
    }

    int getPlantTier(GameObject plant)
    {
        //implementieren: Gameobject einer Pflanze übergeben, die Listen in dieser Klasse durchsuchen und herausfinden in welcher Tier die übergebene Üflanze ist
        return 0;
    }
}
