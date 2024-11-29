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
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(_cam.transform.position, _cam.transform.forward, out RaycastHit hit, rayLength))
        {
            var interactableItem = hit.collider.GetComponent<IInteraction>();
    
            if (interactableItem != null)
            {
                //HighlightCrosshair(true);
                InteractionTextDisplayer(true);
                Debug.Log("etkileşim etkin");
            }
            else
            {
                //HighlightCrosshair(false);
                InteractionTextDisplayer(false);
                Debug.Log("etkileşim iptal");
            }

        }
        else
        {
            InteractionTextDisplayer(false);
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

    void InteractionTextDisplayer(bool on)
    {
        if (on)
        {
            crossHair.enabled = false;
            interactionText.gameObject.SetActive(true);
        }
        else
        {
            crossHair.enabled = true;
            interactionText.gameObject.SetActive(false);
        }
    }
}
