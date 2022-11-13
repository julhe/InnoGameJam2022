using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseRayCaster : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            Camera main = Camera.main;
            Debug.Assert(main);
            
            Ray mouseRay = main.ScreenPointToRay(Input.mousePosition);

            int hitCount = Physics.RaycastNonAlloc(mouseRay, Shared.rayCastHitBuffer, 999.0f);
            for (int hitIdx = 0; hitIdx < hitCount; hitIdx++)
            {
                RaycastHit hit = Shared.rayCastHitBuffer[hitIdx];
                if (hit.transform.gameObject.TryGetComponent(out IInteractable interactable))
                {
                    if (Input.GetMouseButtonDown(1))
                    {
                        
                        interactable.OnInteractByUserRight();
                    }
                    else
                    {
                        
                        interactable.OnInteractByUserLeft();
                    }
                    break;
                }

                var interactableParent = hit.transform.gameObject.GetComponentInParent<IInteractable>();
                if (interactableParent != null)
                {
                    if (Input.GetMouseButtonDown(1))
                    {
                        
                        interactableParent.OnInteractByUserRight();
                    }
                    else
                    {
                        
                        interactableParent.OnInteractByUserLeft();
                    }
                    
                    break;
                }





            }
        }
    }
}
