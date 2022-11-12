using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GlobalWindProperties : MonoBehaviour
{
    public float windSpeed = 1f;
    public static float windSpeedGlobal;
    public float windSize = 1f;
    public float windStrength = 1f;

    public static Vector3 windDirection;

    Vector3 windNoiseOffset;


    private void Start()
    {
        UpdateShader();
    }

    private void Update()
    {
        UpdateShader();
    }

    void UpdateShader()
    {
        // FOG SETTINGS
        //Shader.SetGlobalTexture("_fogGradient", fogGradient);
        //Shader.SetGlobalFloat("_fogDivide", fogDivide);

        Vector3 windDirectionOld = windDirection;

        windDirection = Vector3.Lerp(windDirectionOld, transform.forward, Time.deltaTime / 1);
        //windDirection = transform.forward;
        windSpeedGlobal = windSpeed;

        // Calculates offset vector for world position, to transform the wind noise smoothly
        windNoiseOffset = windNoiseOffset + transform.forward * (windSpeed * Time.deltaTime);

        Shader.SetGlobalVector("_windOrigin", transform.position);
        Shader.SetGlobalVector("_windDirection", windDirection);

        Shader.SetGlobalVector("_windNoiseOffset", windNoiseOffset);

        Shader.SetGlobalFloat("_windSpeed", windSpeed);
        Shader.SetGlobalFloat("_windSize", windSize);
        Shader.SetGlobalFloat("_windStrength", windStrength);


    }
}
