using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TerrainPlane : MonoBehaviour
{
    public ScatterObjectDefinition[] ObjectDefinitions = new ScatterObjectDefinition[0];

    public Vector2 AreaMin, AreaMax;

    public bool clear;
    // Update is called once per frame
    void Update()
    {
        // clean up!
        if (clear)
        {

            clear = false;
            for (int i = 0; i < transform.childCount; i++)
            {
                DestroyImmediate(transform.GetChild(i).gameObject);
            }
                 
        }

        for (int index = 0; index < ObjectDefinitions.Length; index++)
        {
            ScatterObjectDefinition scatterObject = ObjectDefinitions[index];

            GameObject go = index < transform.childCount
                ? transform.GetChild(index).gameObject
                : new GameObject( $"Slot {index}");
            go.transform.SetParent(transform);
            scatterObject.Execute(AreaMin, AreaMax, go.transform, (uint) index);
        }
    }


}
