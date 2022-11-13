using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using Sequence = DG.Tweening.Sequence;

public class Plant : MonoBehaviour, IInteractable, IPlant
{

    public enum PlantState
    {
        Seed,
        Tree,
    }


    public PlantType SelfPlantType;
    
    [SerializeField] int minAmountLikedPlants = 5;

    [SerializeField] GameObject ChildrenPrefab; //Delete this line when it becomes obsolete

    [SerializeField] float NeighbourhoodRadius = 1f;

    [SerializeField] Transform SpawnPointParrent;
    [SerializeField] GameObject TreeVisual, SeedVisual;
    
    [SerializeField] PlantType LikedPlant;
    [SerializeField] PlantType DislikedPlant;


    public PlantState currentPlantState = PlantState.Seed;
    
    void OnEnable()
    {
        
        TreeVisual.transform.localScale = Vector3.zero;
        SeedVisual.transform.localScale = Vector3.zero;
        
        SeedVisual.transform.DOScale(Vector3.one, 0.5f);


    }

    bool CanGrowAtSpot(Vector3 position)
    {
        var treeRenderer = TreeVisual.GetComponentInChildren<MeshRenderer>();
        if (treeRenderer)
        {
            Bounds bound = treeRenderer.bounds;
            Debug.Assert(bound.extents.sqrMagnitude > 0.0);
            bool hasOverlap = CheckOverlapWithPlants(bound.center + position, bound.extents);
            if (hasOverlap)
            {
                return false;
            }
        }
        else
        {
            Debug.LogError($"{name} has missing TreeCollider");
        }
        
        var seedRenderer = SeedVisual.GetComponentInChildren<MeshRenderer>();
        if (seedRenderer)
        {
            Bounds bound = seedRenderer.bounds;
            Debug.Assert(bound.extents.sqrMagnitude > 0.0);
            bool hasOverlap = CheckOverlapWithPlants(bound.center + position, bound.extents);
            if (hasOverlap)
            {
                return false;
            }
        }
        else
        {
            Debug.LogError($"{name} has missing seedCollider");
        }

        return true;
    }

    bool CheckOverlapWithPlants(Vector3 position, Vector3 extends)
    {
        int hits = Physics.OverlapBoxNonAlloc(position, extends, Shared.otherColliderBuffer);
        
        for (int i = 0; i < hits; i++)
        {
            Collider otherHit = Shared.otherColliderBuffer[i];
            Plant otherPlantComponent = otherHit.GetComponentInParent<Plant>();
            if (otherPlantComponent == null)
            {
                continue;
            }
            
            if (otherPlantComponent != null && otherPlantComponent.gameObject == gameObject)
            {
                // don't check on self
                continue;
            }

            return true;
        }
        return false;
    }
    
    bool CheckForGrowConditions()
    {
        int numLikedPlants = 0;
        int numDislikedPlants = 0;

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, NeighbourhoodRadius);
        HashSet<Plant> visitedPlants = new HashSet<Plant>();
        foreach (var hitCollider in hitColliders)
        {
            Plant otherPlant = hitCollider.GetComponentInParent<Plant>();
            if (otherPlant == null) continue;
            if (visitedPlants.Contains(otherPlant))
            {
                continue;
            }

            visitedPlants.Add(otherPlant);
            
            if(otherPlant.SelfPlantType == LikedPlant && otherPlant.currentPlantState != PlantState.Seed)
            {
                numLikedPlants ++;
            }
            else if(otherPlant.SelfPlantType == DislikedPlant && otherPlant.currentPlantState != PlantState.Seed)
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
            Vector3 position = parent.GetChild(i).position;
            position.y = 0.0f; //force to spawn on ground
            childPositions.Add(position);
        }
    }

    static readonly List<Vector3> spawnpointCache = new List<Vector3>();
    bool isInteracting; // prevents interaction while f.e. an animation from a previous interaction is running.
    public void OnInteractByUserLeft()
    {
        if (isInteracting || !CheckForGrowConditions())
        {
            return;
        }
        
        isInteracting = true;
        Sequence seq = DOTween.Sequence();
        switch (currentPlantState)
        {
            case PlantState.Seed:
                seq.Append(SeedVisual.transform.DOScale(Vector3.zero, 0.1f));
                seq.Append(TreeVisual.transform.DOScale(Vector3.one, 0.1f));
                
                currentPlantState = PlantState.Tree;
                break;
            case PlantState.Tree:
                seq.Append(transform.DOPunchScale(Vector3.one * 0.5f, 0.3f, 4, 0.5f));
                // spawn the new plants!
                spawnpointCache.Clear();
                CollectChildPositions(spawnpointCache, SpawnPointParrent);
                foreach (Vector3 destination in spawnpointCache)
                {
                    //PlantSpawnManager.GetPlantToSpawn(ChildrenPrefab)
                    GameObject plantToSpawn = PlantSpawnManager.Instance.GetPlantToSpawn(SelfPlantType);
                    if (plantToSpawn.GetComponent<Plant>().CanGrowAtSpot(destination))
                    {
                        var go = Instantiate(plantToSpawn, destination, Quaternion.Euler(0,Random.Range(180f, -180f), 0f));
                        go.transform.localScale = Vector3.zero;
            
                        //TODO: make growing more cool by making it erratic -> quantize the scale?
                        var spawnSeq = DOTween.Sequence();

                        float growthDelay = Random.Range(0.05f, 0.3f);
                        go.transform.DOScale(Vector3.one, growthDelay);

                        spawnSeq.AppendInterval(growthDelay * 0.5f);
                        spawnSeq.AppendCallback(() =>
                        {
                            var audioSource = go.GetComponent<AudioSource>();
                            audioSource.pitch += Random.Range(-0.2f, 0.2f);
                            audioSource.Play();
                        });

                    }

            
                }
                
                seq.Append(TreeVisual.transform.DOPunchScale(Vector3.one * 0.5f, 0.3f, 4, 0.5f));
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        seq.AppendCallback(() => isInteracting = false);
        seq.Play();

    }

    public void OnInteractByUserRight()
    {
        OnTryKillByOtherPlant();
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
public enum PlantType
{
    tier0A,
    tier0B,
    tier1A,
    tier1B,
    tier2A,
    tier2B
}