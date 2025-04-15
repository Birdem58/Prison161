using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using prison161.EventBus;
using PuppetOfShadows.EventBinding;

public class InteractionHandler : MonoBehaviour
{
    
    IInteraction interaction;
   
    InteractMe interactMe;

    [SerializeField] private GameObject PlayerInteractionCanvas;
    [SerializeField] private Image crossHair;
    [SerializeField] private TextMeshProUGUI interactTxt;
    [SerializeField] private TextMeshProUGUI reactionMessageText;
    [SerializeField] private Image keyUi;
    private EventBinding<SetCanInteract> setCanInteract;

    private bool canInteract = false;
    public float rayLength = 5f;
    private RaycastHit hit;

    private IInteraction currentDialogueInteraction = null;

    private void OnEnable()
    {
        setCanInteract = new EventBinding<SetCanInteract>(OnCanInteractSetter);
        EventBus<SetCanInteract>.Register(setCanInteract);
    }
    public void OnDisable()
    {
        EventBus<SetCanInteract>.Deregister(setCanInteract);
    }


 
    private void OnCanInteractSetter(SetCanInteract setCanInteract)
    {
        
        canInteract = setCanInteract.canInteract;  
    
    
    }
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
            Debug.Log("Raycast hit: " + hit.collider.name);
            Debug.Log(interaction);
            interaction = hit.collider.GetComponent<IInteraction>();
            interactMe = hit.collider.GetComponent<InteractMe>();
            canInteract = (interaction != null);
        }
        else
        {
            canInteract = false;
            interaction = null;
            interactMe = null;
        }

    }

   

//public class Interaction : MonoBehaviour
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

        if (canInteract)
        {
            if (interactMe != null)
            {
                reactionMessageText.text = interactMe.objectMessage;
                reactionMessageText.gameObject.SetActive(true);
                interactTxt.gameObject.SetActive(false);
                EnableOutline(interactMe);
            }
            else if (interaction != null)
            {
                interactTxt.text = interaction.InteractionPrompt;
                interactTxt.gameObject.SetActive(true);
                reactionMessageText.gameObject.SetActive(false);
                EnableOutline(interaction as MonoBehaviour);
            }

            
        }
        else
        {
            HideInteractionUI();
            DisableAllOutlines();
        }
        Debug.Log("canInteract: " + canInteract);        
        if (canInteract && Input.GetKeyDown(KeyCode.F))
        {
            interaction.Interact();
            Debug.Log("f ye basildi lo");
            currentDialogueInteraction = interaction;
        }

        if (!canInteract && Input.GetKeyDown(KeyCode.Escape))
        {
            HideInteractionUI();
        }
    }




    void EnableOutline(MonoBehaviour obj)
    {
        if (obj == null ) return;

        GameObject target = obj.gameObject;
        Outline outline = target.GetComponent<Outline>();

        NpcInterract npcInterract = target.GetComponent<NpcInterract>();
        if(npcInterract != null)
        {
            return;
        }


        if (outline == null)
        {
            outline = target.AddComponent<Outline>();
            outline.OutlineColor = Color.yellow; 
            outline.OutlineWidth = 10.0f; 
        }

        outline.enabled = true;
    }
  

    void DisableAllOutlines()
    {
        Outline[] outlines = FindObjectsOfType<Outline>();
        foreach (Outline outline in outlines)
        {
            outline.enabled = false;
        }
    }


    public void HideInteractionUI()
    {
        interactTxt.gameObject.SetActive(false);
        reactionMessageText.gameObject.SetActive(false);
    }
}
