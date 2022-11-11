using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TerrainPlane : MonoBehaviour
{
    public ScatterObjectDefinition[] ObjectDefinitions = new ScatterObjectDefinition[0];

    public Vector2 AreaMin, AreaMax;
    // Update is called once per frame
    void Update()
    {

        // clean up!
        for (int i = 0; i < transform.childCount; i++)
        {
            //DestroyImmediate(transform.GetChild(i).gameObject);
        }
        
        foreach (ScatterObjectDefinition scatterObject in ObjectDefinitions)
        {
            scatterObject.Execute(AreaMin, AreaMax, transform);
        }
    }
}
