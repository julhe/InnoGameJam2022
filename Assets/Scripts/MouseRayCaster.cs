using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseRayCaster : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Camera main = Camera.main;
            Debug.Assert(main);
            
            Ray mouseRay = main.ScreenPointToRay(Input.mousePosition);

            int hitCount = Physics.RaycastNonAlloc(mouseRay, Shared.rayCastHitBuffer, 999.0f);
            for (int hitIdx = 0; hitIdx < hitCount; hitIdx++)
            {
                RaycastHit hit = Shared.rayCastHitBuffer[hitIdx];
                if (!hit.transform.gameObject.TryGetComponent(out IInteractable interactable))
                {
                    continue;
                }
                
                interactable.OnInteractByUser();
                break;
            }
        }
    }
}
