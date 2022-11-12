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
    [SerializeField] GameObject ChildrenPrefab;

    [SerializeField] float SpawnRadius = 1f;
    [SerializeField] int SeedCount = 8;

    void OnEnable()
    {
        //GetComponent<Collider>().
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
    public void OnInteractByUser()
    {
        transform.DOPunchScale(Vector3.one * 0.5f, 0.3f, 4, 0.5f);
        
        // spawn the new plants!
        spawnpointCache.Clear();
        GenerateSpawnPointsCircle(spawnpointCache, transform.position, SpawnRadius, SeedCount);
        foreach (var destination in spawnpointCache)
        {
            var go = Instantiate(ChildrenPrefab, destination, Quaternion.Euler(0,Random.Range(180f, -180f), 0f));
            go.transform.localScale = Vector3.zero;
            go.transform.DOScale(Vector3.one, Random.Range(0.1f, 0.5f));
            
        }
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position,SpawnRadius );
    }
}
