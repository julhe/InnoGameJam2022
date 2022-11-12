using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//[ExecuteAlways]
public class RotateParticleSystemY : MonoBehaviour
{
   
    public float rotation = 1f;


    // Update is called once per frame
    void Update()
    {
        //transform.LookAt(cam.transform);
        transform.Rotate(0, rotation, 0);
    }
}
