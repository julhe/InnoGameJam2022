using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Shared
{
    public static RaycastHit[] rayCastHitBuffer = new RaycastHit[255];
    public static Collider[] otherColliderBuffer = new Collider[255];
}
