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
    [SerializeField] int minAmountLikedPlants = 5;

    [SerializeField] GameObject ChildrenPrefab; //Delete this line when it becomes obsolete
    [SerializeField] PlantSpawnManager PlantSpawnManager;
    
    [SerializeField] float NeighbourhoodRadius = 1f;

    [SerializeField] Transform SpawnPointParrent;
    [SerializeField] GameObject TreeVisual, SeedVisual;
    
    [SerializeField] GameObject LikedPlant;
    [SerializeField] GameObject DislikedPlant;


    bool isInSeedState = true;
    
    void OnEnable()
    {
        TreeVisual.transform.localScale = Vector3.zero;
        SeedVisual.transform.localScale = Vector3.zero;
        SeedVisual.transform.DOScale(Vector3.one, 0.5f);

        // remove other trees that block the current tree
        var ownBounds = TreeVisual.GetComponent<Collider>().bounds;
        int hits = Physics.OverlapBoxNonAlloc(ownBounds.center, ownBounds.extents, Shared.otherColliderBuffer);
        for (int i = 0; i < hits; i++)
        {
            Collider otherHit = Shared.otherColliderBuffer[i];
            if (otherHit.transform.parent && otherHit.transform.parent.gameObject == gameObject)
            {
                continue;
            }
            if (otherHit.gameObject.TryGetComponent(out IPlant plant))
            {
                
                //remove other plants, so they won't intersect!
                plant.OnTryKillByOtherPlant();
            }
            else
            {
                plant = otherHit.GetComponentInParent<IPlant>();
                if (plant != null)
                {
                    plant.OnTryKillByOtherPlant();
                }
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

    bool CheckForGrowConditions()
    {
        int numLikedPlants = 0;
        int numDislikedPlants = 0;

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, NeighbourhoodRadius);
        foreach (var hitCollider in hitColliders)
        {
            Plant plant = hitCollider.GetComponent<Plant>();
            if (plant == null) continue;
            if(plant.gameObject == LikedPlant)
            {
                numLikedPlants ++;
            }
            else if(plant.gameObject == DislikedPlant)
            {
                numDislikedPlants ++;
            }
        }

        return numLikedPlants >= numDislikedPlants && numLikedPlants >= minAmountLikedPlants;
    }

    void CollectChildPositions(List<Vector3> childPositions, Transform parent)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            childPositions.Add(parent.GetChild(i).position);
        }
    }

    static readonly List<Vector3> spawnpointCache = new List<Vector3>();
    bool isInteracting;
    public void OnInteractByUser()
    {
        if (isInteracting || !CheckForGrowConditions())
        {
            return;
        }
        
        isInteracting = true;
        var seq = DOTween.Sequence();
        if (isInSeedState)
        {
            isInSeedState = false;
            seq.Append(SeedVisual.transform.DOScale(Vector3.zero, 0.1f));
            seq.Append(TreeVisual.transform.DOScale(Vector3.one, 0.1f));
        }
        else
        // seq.Append(transform.DOPunchScale(Vector3.one * 0.5f, 0.3f, 4, 0.5f));
        // seq.AppendCallback(() => isInteracting = false);
        // seq.Play();
        // // spawn the new plants!
        // spawnpointCache.Clear();
        
        // GenerateSpawnPointsCircle(spawnpointCache, transform.position, SpawnRadius, SeedCount);
        // foreach (var destination in spawnpointCache)
        {
            
            seq.Append(transform.DOPunchScale(Vector3.one * 0.5f, 0.3f, 4, 0.5f));

        
            // spawn the new plants!
            spawnpointCache.Clear();
            CollectChildPositions(spawnpointCache, SpawnPointParrent);
            foreach (var destination in spawnpointCache)
            {
                //PlantSpawnManager.GetPlantToSpawn(ChildrenPrefab)
                var go = Instantiate(PlantSpawnManager.GetPlantToSpawn(ChildrenPrefab), destination, Quaternion.Euler(0,Random.Range(180f, -180f), 0f));
                go.transform.localScale = Vector3.zero;
            
                //TODO: make growing more cool by making it erratic -> quantize the scale?
                go.transform.DOScale(Vector3.one, Random.Range(0.1f, 0.5f));
            
            }
        }

        seq.AppendCallback(() => isInteracting = false);
        seq.Play();

    }
    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, NeighbourhoodRadius);
    }

    public void OnTryKillByOtherPlant()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(transform.DOScale(Vector3.zero, 0.2f));
        sequence.AppendCallback(() => { Destroy(gameObject); });
        sequence.Play();
    }
}
