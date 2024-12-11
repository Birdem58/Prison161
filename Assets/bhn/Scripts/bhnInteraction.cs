using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class bhnInteraction : MonoBehaviour
{
    [Header("Crosshair")]
    [SerializeField] private Image crossHair;
    
    [SerializeField] private float rayLength = 5f;
    [SerializeField]private Camera _cam;
    
    [SerializeField]private TextMeshProUGUI interactionText;
    [SerializeField]private TextMeshProUGUI reactionMessageText;

    private bool _isInteracting;

    private void Start()
    {
        _isInteracting = false;
        crossHair.enabled = true;
        interactionText.gameObject.SetActive(false);
        interactionText.text = "Interact[F]";
        reactionMessageText.gameObject.SetActive(false);
        reactionMessageText.text = "";
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.F))
        {
            ReactionMessageTextDisplayer();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CrossHairInteraction();
        }

        if (!_isInteracting)
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
        _isInteracting = false;
        reactionMessageText.gameObject.SetActive(false);
        interactionText.gameObject.SetActive(true);
        
        if (Physics.Raycast(_cam.transform.position, _cam.transform.forward, out RaycastHit hit, rayLength))
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
                        InteractionTextDisplayer(false,"Interact[F]");
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
        if (Physics.Raycast(_cam.transform.position, _cam.transform.forward, out RaycastHit hit, rayLength))
        {
            var reactionMessageValue = hit.collider.GetComponent<InteractMe>();
            string reactionMessage = reactionMessageValue.objectMessage;

            if (reactionMessageValue != null)
            {
                _isInteracting = true;
                reactionMessageText.text = reactionMessage;
                interactionText.gameObject.SetActive(false);
                reactionMessageText.gameObject.SetActive(true);
            }

            else return;

        }
        else
        {
            return;
        }
    }
}
