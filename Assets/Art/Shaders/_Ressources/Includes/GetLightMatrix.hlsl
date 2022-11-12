#ifndef GetLightMatrix_INCLUDED
#define GetLightMatrix_INCLUDED
 
Matrix _lightMatrix;

void GetLightMatrix_float(out Matrix lightMatrix)
{
    lightMatrix = _lightMatrix;
}


#endif