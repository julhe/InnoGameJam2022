#ifndef GetGlobalTerrainParameters_INCLUDED
#define GetGlobalTerrainParameters_INCLUDED
 
//Texture _fogGradient;
TEXTURE2D(_terrainSplatmap);
SAMPLER(sampler_terrainSplatmap);
float4 _terrainSplatmap_TexelSize;

float _terrainSize;

TEXTURE2D(_terrainLayer00);
SAMPLER(sampler_terrainLayer00);
float4 _terrainLayer00Color;
float2 _terrainLayer00Tiling;

TEXTURE2D(_terrainLayer01);
SAMPLER(sampler_terrainLayer01);
float4 _terrainLayer01Color;
float2 _terrainLayer01Tiling;

 
void GetTerrainSplatmap_float(in float2 terrainUV, out float4 terrainSplatmap)
{
	terrainSplatmap = SAMPLE_TEXTURE2D(_terrainSplatmap, sampler_terrainSplatmap, terrainUV).rgba;
}

void GetTerrainSize_float(out float terrainSize)
{
	terrainSize = _terrainSize;
}

void GetTerrainLayer00_float(in float2 terrainUV, out float4 terrainLayer)
{
	terrainLayer = SAMPLE_TEXTURE2D(_terrainLayer00, sampler_terrainLayer00, terrainUV * _terrainLayer00Tiling).rgba * _terrainLayer00Color;
}

void GetTerrainLayer01_float(in float2 terrainUV, out float4 terrainLayer)
{
	terrainLayer = SAMPLE_TEXTURE2D(_terrainLayer01, sampler_terrainLayer01, terrainUV * _terrainLayer01Tiling).rgba * _terrainLayer01Color;
}

#endif