#ifndef GetGlobalLightParameter_INCLUDED
#define GetGlobalLightParameter_INCLUDED
 
float4 _shadowColor;

void GetLightParameters_float(out float4 shadowColor)
{
    shadowColor = _shadowColor;
}

#endif