#ifndef GetGlobalLightCookieParameters_INCLUDED
#define GetGlobalLightCookieParameters_INCLUDED
 
// float _cloudSpeed;
float _lightCookieOpacity;
float _lightCookieProjectionMethod;

void GetLightCookieParameters_float(out float lightCookieOpacity, out float lightCookieProjectionMethod)
{
    //cloudSpeed = _cloudSpeed;
    lightCookieOpacity = _lightCookieOpacity;
    lightCookieProjectionMethod = _lightCookieProjectionMethod;
}

#endif