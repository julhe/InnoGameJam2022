using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveByMovingMouseOnEdge : MonoBehaviour
{
    [SerializeField, Range(0.0f, 1.0f)] float DistanceToEdge = 0.1f;

    [SerializeField] float Speed, MaxRadius;
    // Update is called once per frame
    void Update()
    {
        Camera mainCamera = Camera.main;
        Vector2 mouseViewport = mainCamera.ScreenToViewportPoint(Input.mousePosition);
        
        mouseViewport.x = mouseViewport.x * 2.0f - 1.0f;
        mouseViewport.y = mouseViewport.y * 2.0f - 1.0f;
        
        

        Vector2 mouseViewPortAbs = new Vector2(Mathf.Abs(mouseViewport.x), Mathf.Abs(mouseViewport.y));

        float mouseDistToEdge = Mathf.Max(mouseViewPortAbs.x, mouseViewPortAbs.y);
        if (mouseDistToEdge > 1.0f)
        {
            // dont do anything if we do something in the editor.
            return;
        }
        
        Vector2 mouseScrollDir = mouseViewport.normalized;
        if (mouseDistToEdge > DistanceToEdge)
        {
            float speedWeight = Mathf.InverseLerp(DistanceToEdge, 1.0f, mouseDistToEdge);
            Vector3 scrollVecWorldSpace = new Vector3(mouseScrollDir.x, 0.0f, mouseScrollDir.y) * Speed * speedWeight * Time.deltaTime;
            transform.position += scrollVecWorldSpace;
        }
        
        //clamp max range from origin
        transform.position = Vector3.ClampMagnitude(transform.position, MaxRadius);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(Vector3.zero, MaxRadius);
    }
}
