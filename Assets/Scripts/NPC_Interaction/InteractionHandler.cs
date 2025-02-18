using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using prison161.EventBus;

public class InteractionHandler : MonoBehaviour
{
    // Reference to the interactable component found via raycast.
    IInteraction interaction;
    // Optional component that can provide an alternate object message.
    InteractMe interactMe;

    [SerializeField] private GameObject PlayerInteractionCanvas;
    [SerializeField] private Image crossHair;
    [SerializeField] private TextMeshProUGUI interactionText;
    [SerializeField] private TextMeshProUGUI reactionMessageText;

    private bool CanInteract = false;
    public float rayLength = 3f;
    private RaycastHit hit;

    // This field stores the dialogue interaction once initiated.
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

        if (interactionText != null)
        {
            interactionText.gameObject.SetActive(false);
        }

        if (reactionMessageText != null)
        {
            reactionMessageText.gameObject.SetActive(false);
        }
    }

    void FixedUpdate()
    {
        // When in dialogue mode, we retain the dialogue interaction without updating the raycast.
        if (PlayerState.Instance.GetState() == PlayerState.State.DIALOGUE)
            return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, rayLength))
        {
            // Retrieve the IInteraction and optional InteractMe components.
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
        // Clear any stored dialogue interaction if not in dialogue mode.
        if (PlayerState.Instance.GetState() != PlayerState.State.DIALOGUE)
            currentDialogueInteraction = null;

        // In dialogue mode, process dialogue input and hide the interaction UI.
        if (PlayerState.Instance.GetState() == PlayerState.State.DIALOGUE)
        {
            HideInteractionUI();
            if (Input.GetKeyDown(KeyCode.F) && currentDialogueInteraction != null)
            {
                currentDialogueInteraction.Interact();
            }
            return;
        }

        // Normal mode: display the appropriate prompt.
        if (CanInteract)
        {
            // If an InteractMe component is present, display its object message.
            if (interactMe != null)
            {
                reactionMessageText.text = interactMe.objectMessage;
                reactionMessageText.gameObject.SetActive(true);
                interactionText.gameObject.SetActive(false);
            }
            // Otherwise, display the prompt provided by the interactable object.
            else if (interaction != null)
            {
                interactionText.text = interaction.InteractionPrompt;
                interactionText.gameObject.SetActive(true);
                reactionMessageText.gameObject.SetActive(false);
            }
        }
        else
        {
            HideInteractionUI();
        }

        // When F is pressed, trigger the interaction.
        if (CanInteract && Input.GetKeyDown(KeyCode.F))
        {
            interaction.Interact();
            currentDialogueInteraction = interaction;
            
        }

        // Allow the player to hide the UI with Escape.
        if (!CanInteract && Input.GetKeyDown(KeyCode.Escape))
        {
            HideInteractionUI();
        }
    }

    /// <summary>
    /// Hides both interaction UI elements.
    /// </summary>
    public void HideInteractionUI()
    {
        interactionText.gameObject.SetActive(false);
        reactionMessageText.gameObject.SetActive(false);
    }
}
