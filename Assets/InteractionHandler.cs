using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionHandler : MonoBehaviour
{


    //on fixed update raycast 10 units in front of the player if there is an object of type IInteraction call the interact method if pressed F

    IInteraction interaction;
    [SerializeField] GameObject PlayerInteractionCanvas;
    private bool CanInteract = false;
    void FixedUpdate()
    {
        if(PlayerState.Instance.GetState() != PlayerState.State.DEFAULT)
        {
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 10))
        {
            if (hit.collider.GetComponent<IInteraction>() != null)
            {
                interaction = hit.collider.GetComponent<IInteraction>();
                CanInteract = true;
            }else{
                CanInteract = false;
                interaction = null;
            }
        }else{
            CanInteract = false;
            interaction = null;
        }
    }
    void Update()
    {
        if ( CanInteract && Input.GetKeyDown(KeyCode.F))
        {
            interaction.Interact();
        }
        
        PlayerInteractionCanvas.SetActive(CanInteract && PlayerState.Instance.GetState() == PlayerState.State.DEFAULT);
    }
}
