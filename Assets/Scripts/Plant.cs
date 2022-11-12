using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Plant : MonoBehaviour, IInteractable, IPlant
{    
    public enum PlantType
    {
        tier1A,
        tier1B,
        tier2A,
        tier2B,
        tier3A,
        tier3B
    }

    public PlantType ThisPlantType;
    
    [SerializeField] int minAmountLikedPlants = 5;

    [SerializeField] GameObject ChildrenPrefab;

    [SerializeField] float SpawnRadius = 1f;
    [SerializeField] int SeedCount = 8;

    PlantType LikedPlant;
    PlantType DislikedPlant;

    void OnEnable()
    {
        var ownBounds = GetComponent<Collider>().bounds;
        int hits = Physics.OverlapBoxNonAlloc(ownBounds.center, ownBounds.extents, Shared.otherColliderBuffer);
        for (int i = 0; i < hits; i++)
        {
            Collider otherHit = Shared.otherColliderBuffer[i];
            if (otherHit.gameObject == gameObject)
            {
                continue;
            }
            if (otherHit.gameObject.TryGetComponent(out IPlant plant))
            {
                //remove other plants, so they won't intersect!
                plant.OnTryKillByOtherPlant();
            }
        }
    }

    void OnCollisionStay(Collision other)
    {
        if(other.gameObject.TryGetComponent(out IPlant plant))
        {
            plant.OnTryKillByOtherPlant();
        }
    }

    bool CheckForGrowConditions(float neighbourhoodRadius)
    {
        int numLikedPlants = 0;
        int numDislikedPlants = 0;

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, neighbourhoodRadius);
        foreach (var hitCollider in hitColliders)
        {
            Plant plant = hitCollider.GetComponent<Plant>();
            if(plant != null){
                if(plant.ThisPlantType == LikedPlant)
                {
                    numLikedPlants ++;
                }
                else if(plant.ThisPlantType == DislikedPlant)
                {
                    numDislikedPlants ++;
                }
            }
        }

        if(numLikedPlants > numDislikedPlants && numLikedPlants > minAmountLikedPlants)
        {
            return(true);
        }
        else
        {
            return(false);
        }
    }

    static void GenerateSpawnPointsCircle(List<Vector3> spawnPoints, Vector3 origin, float radius, int count)
    {
        float angleStep = (Mathf.PI * 2.0f) / count ;
        for (int i = 0; i < count; i++)
        {
            float angle = i * angleStep;
            Vector3 rawPosition = new Vector3(Mathf.Sin(angle), 0.0f, Mathf.Cos(angle)) * radius;
            spawnPoints.Add(rawPosition + origin);
        }
    }

    static readonly List<Vector3> spawnpointCache = new List<Vector3>();
    bool isInteracting;
    public void OnInteractByUser()
    {
        if (isInteracting)
        {
            return;
        }
        
        isInteracting = true;
        var seq = DOTween.Sequence();
        seq.Append(transform.DOPunchScale(Vector3.one * 0.5f, 0.3f, 4, 0.5f));
        seq.AppendCallback(() => isInteracting = false);
        seq.Play();
        // spawn the new plants!
        spawnpointCache.Clear();
        GenerateSpawnPointsCircle(spawnpointCache, transform.position, SpawnRadius, SeedCount);
        foreach (var destination in spawnpointCache)
        {
            var go = Instantiate(ChildrenPrefab, destination, Quaternion.Euler(0,Random.Range(180f, -180f), 0f));
            go.transform.localScale = Vector3.zero;
            
            //TODO: make growing more cool by making it erratic -> quantize the scale?
            go.transform.DOScale(Vector3.one, Random.Range(0.1f, 0.5f));
            
        }
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position,SpawnRadius );
    }

    public void OnTryKillByOtherPlant()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(transform.DOScale(Vector3.zero, 0.2f));
        sequence.AppendCallback(() => { Destroy(gameObject); });
        sequence.Play();
    }
}
