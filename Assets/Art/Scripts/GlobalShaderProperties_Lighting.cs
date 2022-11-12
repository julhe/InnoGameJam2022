using UnityEngine;

[ExecuteInEditMode]
public class GlobalShaderProperties_Lighting : MonoBehaviour
{
    [Header("Ambient Light")]
    public Color shadowColor = Color.gray;

    [Space]
    [Header("Light Cookie Settings")]
    //public float cloudSpeed = 1f;
    [SerializeField, Range(0, 1f)]
    public float lightCookieOpacity = 1f;
    [Tooltip("0 = From above; 1 = From Sun Direction")]
    public int lightCookieProjectionMethod = 0;


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
        // LIGHT SETTINGS
        Shader.SetGlobalVector("_shadowColor", shadowColor);

        // LIGHT COOKIE SETTINGS
        //Shader.SetGlobalFloat("_cloudSpeed", cloudSpeed);
        Shader.SetGlobalFloat("_lightCookieOpacity", lightCookieOpacity);
        Shader.SetGlobalInt("_lightCookieProjectionMethod", lightCookieProjectionMethod);
    }
}
