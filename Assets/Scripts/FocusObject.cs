using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Camera))]
[ExecuteAlways]
public class FocusObject : MonoBehaviour
{
    [SerializeField] float Distance = 50.0f;
    [SerializeField, Range(0.0f, 180.0f)] float Angle = 35.0f;

    [SerializeField] Transform Target;

    [SerializeField] float Damping = 1.0f;

    Vector3 velocity;
    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(Angle, 0.0f, 0.0f);
        Vector3 offset = transform.forward * -Distance;
        Vector3 targetPosition = Target ? Target.position : Vector3.zero;
        if (Application.isPlaying)
        {
            
            transform.position = Vector3.SmoothDamp(transform.position, offset + targetPosition, ref velocity, Damping);
           
        }
        else
        {
            transform.position = offset + targetPosition;
        }
    }
}
