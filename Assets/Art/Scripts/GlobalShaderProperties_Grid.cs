using UnityEngine;

[ExecuteInEditMode]
public class GlobalShaderProperties_Grid : MonoBehaviour
{
    [Space]
    [Header("Grid Settings")]
    [SerializeField, Range(0, 1f)]
    public float gridOpacity = 1f;


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

        // GRID SETTINGS
        Shader.SetGlobalFloat("_gridOpacity", gridOpacity);

    }
}
