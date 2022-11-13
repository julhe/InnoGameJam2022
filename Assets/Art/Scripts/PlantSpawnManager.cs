using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    public GameObject GetPlantToSpawn(PlantType parentPlant)
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

    int GetPlantTier(PlantType plant)
    {
        foreach (var tier0Plant in tier0Plants)
        {
            if (plant == tier0Plant.GetComponent<Plant>().SelfPlantType)
                return 0;
        }
        foreach (var tier1Plant in tier1Plants)
        {
            if (plant == tier1Plant.GetComponent<Plant>().SelfPlantType)
                return 1;
        }
        foreach (var tier2Plant in tier2Plants)
        {
            if (plant == tier2Plant.GetComponent<Plant>().SelfPlantType)
                return 2;
        }
        throw new ArgumentException("Plant missing in the PlantSpawnManagers plants-list");
    }
}
