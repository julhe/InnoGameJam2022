using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class RotateParticleSystem : MonoBehaviour
{
   
    public float rotation = 1f;

    [SerializeField]
    public Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //transform.LookAt(cam.transform);
        transform.Rotate(0, 0, rotation);
    }
}
