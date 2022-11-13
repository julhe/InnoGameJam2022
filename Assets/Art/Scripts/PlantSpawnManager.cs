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

    void Awake()
    {
        Instance = this;
    }

    private GameObject GetPlant(int tierIndex, int plantIndex)
    {
        switch(tierIndex) 
        {
            case 0:
                return tier0Plants[plantIndex];
            case 1:
                return tier1Plants[plantIndex];
            case 2:
                return tier2Plants[plantIndex];
            default:
                throw new ArgumentException("Invalid Plant Tier");
        }
    }

    public GameObject GetPlantToSpawn(GameObject parentPlant)
    {
        int plantToSpawnTier = GetPlantTier(parentPlant);
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
            if(plantToSpawnTier < 2) //tierList.Count - 1
                plantToSpawnTier ++;
        }
        return GetPlant(plantToSpawnTier, Random.Range(0, 2));
    }

    int GetPlantTier(GameObject plant)
    {
        //implementieren: Gameobject einer Pflanze übergeben, die Listen in dieser Klasse durchsuchen und herausfinden in welcher Tier die übergebene Üflanze ist
        return 0;
    }
}
