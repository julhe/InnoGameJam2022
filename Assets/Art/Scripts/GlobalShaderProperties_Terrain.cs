using UnityEngine;

[ExecuteInEditMode]
public class GlobalShaderProperties_Terrain : MonoBehaviour
{
    
    //[SerializeField] Terrain terrain;
    //[SerializeField] Texture2D terrainHeightmap;
    //[SerializeField] Texture2D terrainNormalmap;
    
    //[SerializeField] float terrainSize = 200f;
    
    [Space]
    [SerializeField] Texture2D terrainLayer00;
    [SerializeField] Color terrainLayer00Color = Color.white;
    [SerializeField] float terrainLayer00Tiling = 1f;
    /*
    [Space]
    [SerializeField] Texture2D terrainLayer01;
    [SerializeField] Color terrainLayer01Color = Color.white;
    [SerializeField] float terrainLayer01Tiling = 1f;
    */


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

        // TERRAIN SETTINGS
        /*
        Shader.SetGlobalTexture("_terrainSplatmap", terrain.terrainData.alphamapTextures[0]);
        Shader.SetGlobalTexture("_terrainHeightmap", terrainHeightmap);
        Shader.SetGlobalTexture("_terrainNormalmap", terrainNormalmap);
        */
        //Shader.SetGlobalFloat("_terrainSize", terrainSize);

        
        Shader.SetGlobalTexture("_terrainLayer00", terrainLayer00);
        Shader.SetGlobalVector("_terrainLayer00Color", terrainLayer00Color);
        
        Shader.SetGlobalVector("_terrainLayer00Tiling", new Vector2(terrainLayer00Tiling, terrainLayer00Tiling));
        /*
        Shader.SetGlobalTexture("_terrainLayer01", terrainLayer01);
        Shader.SetGlobalVector("_terrainLayer01Color", terrainLayer01Color);
        Shader.SetGlobalVector("_terrainLayer01Tiling", new Vector2(terrainLayer01Tiling, terrainLayer01Tiling));
        */


    }
}
