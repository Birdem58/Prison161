using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionHandler : MonoBehaviour
{


    //on fixed update raycast 10 units in front of the player if there is an object of type IInteraction call the interact method if pressed F

    IInteraction interaction;
    void FixedUpdate()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 10))
        {
            if (hit.collider.GetComponent<IInteraction>() != null)
            {
                interaction = hit.collider.GetComponent<IInteraction>();
            }
        }
    }
    void Update()
    {
        if (interaction != null && Input.GetKeyDown(KeyCode.F))
        {
            interaction.Interact();
        }
    }
}
