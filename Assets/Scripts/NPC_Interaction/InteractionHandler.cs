using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InteractionHandler : MonoBehaviour
{

    //on fixed update raycast 10 units in front of the player if there is an object of type IInteraction call the interact method if pressed F

    IInteraction interaction;
    
    [SerializeField] GameObject PlayerInteractionCanvas;
    [SerializeField] private Image crossHair;
    [SerializeField]private TextMeshProUGUI interactionText;
    [SerializeField]private TextMeshProUGUI reactionMessageText;
    
    private bool CanInteract=false;
    //[SerializeField] private Camera _cam;
    
    [SerializeField] private float rayLength = 10f;

    private RaycastHit hit;//hitInfo
    private bool raycastHit;
    private void Start()
    {
        crossHair.enabled = true;
        interactionText.gameObject.SetActive(false);
        interactionText.text = "Interact[F]";
        reactionMessageText.gameObject.SetActive(false);
        reactionMessageText.text = "";
    }

    void FixedUpdate()
    {
        if(PlayerState.Instance.GetState() != PlayerState.State.DEFAULT)
        {
            crossHair.enabled = false;
        }
        else if (PlayerState.Instance.GetState() != PlayerState.State.DIALOGUE)
        {
            crossHair.enabled = true;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        raycastHit = Physics.Raycast(ray, out hit);

        if (Physics.Raycast(ray, out hit, rayLength))
        {
            if (hit.collider.GetComponent<IInteraction>() != null) //IInteraction'a erişebilirse interact edilebilir
            {
                
                interaction = hit.collider.GetComponent<IInteraction>();
                CanInteract = true; 
            }
            else
            {
                CanInteract = false; //else durumu zaten
                interaction = null;
            }
        }
        else
        {
            CanInteract = false; //yine else durumu
            interaction = null;
        }


    }
    void Update()
    {
        PlayerInteractionCanvas.SetActive(CanInteract && PlayerState.Instance.GetState() == PlayerState.State.DEFAULT);
        
        if ( CanInteract && Input.GetKeyDown(KeyCode.F))
        {
            interaction.Interact();
            ReactionMessageTextDisplayer();
        }
        
        if (!CanInteract &&Input.GetKeyDown(KeyCode.Escape))
        {
            CrossHairInteraction();
        }
        
        if (CanInteract)
        {
            CrossHairInteraction();
        }
    }
    
    
    void HighlightCrosshair(bool on)
    {
        if (on)
        {
            crossHair.color = Color.red;
        }
        else { crossHair.color = Color.white; }
    }
    
    public void CrossHairInteraction()
    {
        //CanInteract = true;
        reactionMessageText.gameObject.SetActive(false);
        interactionText.gameObject.SetActive(true);
        
        if (PlayerState.Instance.GetState() != PlayerState.State.DIALOGUE)
        {
            var interactableItem = hit.collider.GetComponent<IInteraction>();
            string tag = hit.collider.tag;
            
            if (interactableItem != null)
            {
                switch (tag)
                {
                    case"Abnormal":
                        InteractionTextDisplayer(true, "Talk[F]");
                        break;
                    case"Normal":
                        InteractionTextDisplayer(true, "Talk[F]");
                        break;
                    case"Paper":
                        InteractionTextDisplayer(true, "Take[F]");
                        break;
                    case"Door":
                        InteractionTextDisplayer(true, "Open[F]");
                        break;
                    default:
                        InteractionTextDisplayer(true,"Interact[F]");
                        break;
                }
                Debug.Log("etkileşim etkin");
            }

            else
            {
                //HighlightCrosshair(false);
                InteractionTextDisplayer(false,"");
                Debug.Log("etkileşim iptal");
            }

        }
        else
        {
            InteractionTextDisplayer(false,"");
        }
    }
    
    void InteractionTextDisplayer(bool on, string text)
    {
        if (on)
        {
            crossHair.enabled = false;
            interactionText.text = text;
            interactionText.gameObject.SetActive(true);
        }
        else
        {
            crossHair.enabled = true;
            interactionText.gameObject.SetActive(false);
        }
    }
    
    void ReactionMessageTextDisplayer()
    {

            var reactionMessageValue = hit.collider.GetComponent<InteractMe>();
            string reactionMessage = reactionMessageValue.objectMessage;

            if (reactionMessageValue != null)
            {
                CanInteract = false;
                reactionMessageText.text = reactionMessage;
                interactionText.gameObject.SetActive(false);
                reactionMessageText.gameObject.SetActive(true);
            }

            else return;


    }
    
}
