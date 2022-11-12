#ifndef GetGlobalWindParameters_INCLUDED
#define GetGlobalWindParameters_INCLUDED
 
vector _windOrigin;
vector _windDirection;

vector _windNoiseOffset;

float _windSpeed;
float _windSize;
float _windStrength;

void GetWindParameters_float(out vector windOrigin, out vector windDirection, out vector windNoiseOffset, out float windSpeed, out float windSize, out float windStrength)
{
    windOrigin = _windOrigin;
    windDirection = _windDirection;

    windNoiseOffset = _windNoiseOffset;

    windSpeed = _windSpeed;
    windSize = _windSize;
    windStrength = _windStrength;
}


#endif