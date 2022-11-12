using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using Random = Unity.Mathematics.Random;

[CreateAssetMenu(menuName = "ScatterObjectDefinition")]
public class ScatterObjectDefinition : ScriptableObject
{
    public GameObject Prefab;
    [Range(-5f, 5f)] public float NoiseAmplitude = 1.0f;
    [Range(0.0f, 1.0f)] public float NoiseFrequency = 1.0f;
    [Range(-5f, 5f)] public float NoiseBias = 0.0f;

    public float ScaleMin = 0.5f, ScaleMax = 1.5f;
    [Min(0.0f)] public float Density = 1.0f;

    public float  RelaxRange = 1.0f;
    public int RelaxIterations = 1;
    public void Execute(float2 areaMin, float2 areaMax, Transform parent, uint seed)
    {
        int totalCount = Mathf.RoundToInt(math.lengthsq(areaMax - areaMin) * Density);
        NativeArray<float3> positionAndScale = new NativeArray<float3>(totalCount, Allocator.TempJob);
        NoiseScatterJob scatterJob = new()
        {
            areaMin = areaMin,
            areaMax = areaMax,
            NoiseAmp = NoiseAmplitude,
            NoiseFreq = NoiseFrequency,
            NoiseBias = NoiseBias,
            Positions = positionAndScale,
        };
        
        scatterJob.Schedule(totalCount, 16).Complete();

        for (int relaxIteration = 0; relaxIteration < RelaxIterations; relaxIteration++)
        {
            RelaxPointsJob relaxJob = new()
            {
                Positions = positionAndScale,
                PositionsOut = new NativeArray<float3>(scatterJob.Positions.Length, Allocator.TempJob),
                RelaxRange = RelaxRange,
            };
        
            relaxJob.Schedule(scatterJob.Positions.Length, 16).Complete();

            positionAndScale.CopyFrom(relaxJob.PositionsOut);
            relaxJob.PositionsOut.Dispose();
        }

        for (int i = 0; i < scatterJob.Positions.Length; i++)
        {
            var posAndNoise = scatterJob.Positions[i];
            // apply scale.
            posAndNoise.z = posAndNoise.z > 0.0f ? Mathf.Lerp(ScaleMin, ScaleMax, posAndNoise.z) : 0.0f;
            positionAndScale[i] = posAndNoise;
        }

        // place objects
        int debugInstantiateCount = 0;
        Random rnd = Random.CreateFromIndex(seed);
        for (int index = 0; index < positionAndScale.Length; index++)
        {
            float3 positionAndNoise = positionAndScale[index];
            bool shouldPlace = positionAndNoise.z > 0.0;

            GameObject go;
            if (index < parent.childCount )
            {
                go = parent.GetChild(index).gameObject;
            }
            else
            {
                go = Instantiate(Prefab, parent);
                debugInstantiateCount++;
            }

            if (!shouldPlace)
            {
                go.SetActive(false);
                continue;
            }

            go.SetActive(true);
            go.transform.position = new Vector3(positionAndNoise.x, 0.0f, positionAndNoise.y);
            go.transform.localScale = positionAndNoise.z * Vector3.one;
            go.transform.rotation = Quaternion.Euler(0f, rnd.NextFloat(-180f, 190f), 0f);
            
            go.hideFlags = HideFlags.DontSave;
        }

        if (debugInstantiateCount > 0)
        {
            Debug.Log($"Instantiated {debugInstantiateCount} new instances.");
        }

        positionAndScale.Dispose();
    }
}

[BurstCompile]

public struct NoiseScatterJob : IJobParallelFor
{
    [ReadOnly] public float2 areaMin, areaMax;
    [ReadOnly] public float NoiseAmp, NoiseFreq, NoiseBias;
    [WriteOnly] public NativeArray<float3> Positions;
    public void Execute(int index)
    {
        //TODO: good distribution?
        Random random = Random.CreateFromIndex((uint) index);
        
        float2 position = random.NextFloat2(areaMin, areaMax);

        float noiseValue = noise.snoise(position * NoiseFreq) * NoiseAmp + NoiseBias;

        Positions[index] = new float3(position.x, position.y, noiseValue);
    }
}

[BurstCompile]

public struct RelaxPointsJob : IJobParallelFor
{
    [ReadOnly] public NativeArray<float3> Positions;
    [WriteOnly] public NativeArray<float3> PositionsOut;
    [ReadOnly] public float RelaxRange;
    public void Execute(int selfIdx)
    {
        float3 currentPos = Positions[selfIdx];
        if (currentPos.z <= 0.0)
        {
            return;
        }
        
        float2 offset = float2.zero;
        for (int otherIdx = 0; otherIdx < Positions.Length; otherIdx++)
        {
            if (selfIdx == otherIdx)
            {
                continue;
            }

            float3 otherPos = Positions[otherIdx];
            if (otherPos.z <= 0.0)
            {
                continue;
            }
            float2 vecToOtherPos = otherPos.xy - currentPos.xy;
            float distance = math.length(vecToOtherPos);

            distance = math.max(distance, 0.0f);

            float weight = (RelaxRange ) - distance ;
            weight = math.max(weight, 0.0f); 
            offset.xy += math.normalize(vecToOtherPos) * (weight * currentPos.z);
        }
        
        currentPos.xy -= offset * 0.5f;
        PositionsOut[selfIdx] = currentPos;
    }
}