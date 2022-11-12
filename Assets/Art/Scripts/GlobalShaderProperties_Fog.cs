using UnityEngine;

[ExecuteInEditMode]
public class GlobalShaderProperties_Fog : MonoBehaviour
{
    public Texture2D fogGradient;
    public float fogDivide = 58f;
    public float fogPower = 10f;
    [SerializeField, Range(0, 1f)]
    public float fogOpacity = 1f;


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
        Shader.SetGlobalTexture("_fogGradient", fogGradient);
        Shader.SetGlobalFloat("_fogDivide", fogDivide);
        Shader.SetGlobalFloat("_fogPower", fogPower);
        Shader.SetGlobalFloat("_fogOpacity", Mathf.Clamp(fogOpacity, 0f, 1f));


    }
}
