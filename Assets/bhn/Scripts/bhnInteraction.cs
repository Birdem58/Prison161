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
    
    [SerializeField] private float rayLength = 10f;
    [SerializeField]private Camera _cam;
    
    [SerializeField]private TextMeshProUGUI interactionText;

    private void Start()
    {
        crossHair.enabled = true;
        interactionText.gameObject.SetActive(false);
        interactionText.text = "Interact[F]";
    }

    // Update is called once per frame
    void Update()
    {
        CrossHairInteraction();
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
}
