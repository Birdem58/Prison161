using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using prison161.EventBus;

public class InteractionHandler : MonoBehaviour
{
    
    IInteraction interaction;
   
    InteractMe interactMe;

    [SerializeField] private GameObject PlayerInteractionCanvas;
    [SerializeField] private Image crossHair;
    [SerializeField] private TextMeshProUGUI interactTxt;
    [SerializeField] private TextMeshProUGUI reactionMessageText;

    private bool CanInteract = false;
    public float rayLength = 3f;
    private RaycastHit hit;

    private IInteraction currentDialogueInteraction = null;

    void Start()
    {
        if (PlayerInteractionCanvas != null)
            PlayerInteractionCanvas.SetActive(true);

        if (crossHair != null)
        {
            crossHair.enabled = true;
            crossHair.color = Color.white;
        }

        if (interactTxt != null)
        {
            interactTxt.gameObject.SetActive(false);
        }

        if (reactionMessageText != null)
        {
            reactionMessageText.gameObject.SetActive(false);
        }
    }

    void FixedUpdate()
    {
        
        if (PlayerState.Instance.GetState() == PlayerState.State.DIALOGUE)
            return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, rayLength))
        {
            
            interaction = hit.collider.GetComponent<IInteraction>();
            interactMe = hit.collider.GetComponent<InteractMe>();
            CanInteract = (interaction != null);
        }
        else
        {
            CanInteract = false;
            interaction = null;
            interactMe = null;
        }
    }

    void Update()
    {
       
        if (PlayerState.Instance.GetState() != PlayerState.State.DIALOGUE)
            currentDialogueInteraction = null;

        
        if (PlayerState.Instance.GetState() == PlayerState.State.DIALOGUE)
        {
            HideInteractionUI();
            if (Input.GetKeyDown(KeyCode.F) && currentDialogueInteraction != null)
            {
                currentDialogueInteraction.Interact();
            }
            return;
        }

      
        if (CanInteract)
        {
            
            if (interactMe != null)
            {
                reactionMessageText.text = interactMe.objectMessage;
                reactionMessageText.gameObject.SetActive(true);
                interactTxt.gameObject.SetActive(false);
            }
           
            else if (interaction != null)
            {
                interactTxt.text = interaction.InteractionPrompt;
                interactTxt.gameObject.SetActive(true);
                reactionMessageText.gameObject.SetActive(false);
            }
        }
        else
        {
            HideInteractionUI();
        }

       
        if (CanInteract && Input.GetKeyDown(KeyCode.F))
        {
            interaction.Interact();
            currentDialogueInteraction = interaction;
            
        }

       
        if (!CanInteract && Input.GetKeyDown(KeyCode.Escape))
        {
            HideInteractionUI();
        }
    }

   
    public void HideInteractionUI()
    {
        interactTxt.gameObject.SetActive(false);
        reactionMessageText.gameObject.SetActive(false);
    }
}
