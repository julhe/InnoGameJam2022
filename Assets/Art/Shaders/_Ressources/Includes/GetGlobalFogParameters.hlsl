#ifndef GetGlobalFogParameters_INCLUDED
#define GetGlobalFogParameters_INCLUDED


TEXTURE2D(_fogGradient);
SAMPLER(sampler_fogGradient);
float4 _fogGradient_TexelSize;

float _fogOpacity;
float _fogDivide;
float _fogPower;

 
void GetFogParameters_float(out float fogDivide, out float fogPower, out float fogOpacity)
{
    fogOpacity = _fogOpacity;
    fogDivide = _fogDivide;
    fogPower = _fogPower;
}

void GetFogGradient_float(in float2 fogGradientUV, out float4 fogGradientColor)
{
    fogGradientColor = SAMPLE_TEXTURE2D(_fogGradient, sampler_fogGradient, fogGradientUV).rgba;
}

#endif