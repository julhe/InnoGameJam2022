using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SetLightMatrix : MonoBehaviour
{
    void Update()
    {
        // Matrix4x4 lightMatrix = transform.worldToLocalMatrix;
        Matrix4x4 lightMatrix = transform.localToWorldMatrix;
        Shader.SetGlobalMatrix("_lightMatrix", lightMatrix);
    }
}