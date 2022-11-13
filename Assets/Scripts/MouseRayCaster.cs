using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class MouseRayCaster : MonoBehaviour
{
    [SerializeField] GameObject LeftClickEffect, RightClickEffect;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            Camera main = Camera.main;
            Debug.Assert(main);
            
            Ray mouseRay = main.ScreenPointToRay(Input.mousePosition);

            int hitCount = Physics.RaycastNonAlloc(mouseRay, Shared.rayCastHitBuffer, 999.0f);

            bool hasClickedInteractible = false;
            for (int hitIdx = 0; hitIdx < hitCount; hitIdx++)
            {
                RaycastHit hit = Shared.rayCastHitBuffer[hitIdx];
                if (!hasClickedInteractible)
                {
                    IInteractable interactable = hit.transform.GetComponent<IInteractable>();
                    interactable ??=
                        hit.transform
                            .GetComponentInParent<
                                IInteractable>(); // try find iinteractible in parent if not present in current.
                    if (interactable != null)
                    {
                        if (Input.GetMouseButtonDown(1))
                        {

                            interactable.OnInteractByUserRight();
                        }
                        else
                        {

                            interactable.OnInteractByUserLeft();
                        }

                        hasClickedInteractible = true;
                    }
                }

                if (hit.transform.gameObject.CompareTag("GroundPlane"))
                {
                    GameObject fx;
                    if (Input.GetMouseButtonDown(1))
                    {
                        fx = Instantiate(RightClickEffect, hit.point, Quaternion.identity);
                        
                    }
                    else
                    {
                        fx = Instantiate(LeftClickEffect, hit.point, Quaternion.identity);
                    }
                    
                    Destroy(fx, 5.0f);
                }
            }
        }
    }
}
