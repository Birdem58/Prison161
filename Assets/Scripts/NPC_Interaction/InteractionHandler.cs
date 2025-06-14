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
    private List<Outline> enabledOutlines = new List<Outline>();

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

        if (canInteract && interactMe != null)
        {
            if (interactMe.isInteractable && Input.GetKeyDown(KeyCode.F))
            {
                
                reactionMessageText.text = interactMe.objectMessage;
                reactionMessageText.gameObject.SetActive(true);
                interactTxt.gameObject.SetActive(false);
            }
            else if (!reactionMessageText.gameObject.activeSelf)
            {
                interactTxt.text = interactMe.interactionMessage;
                interactTxt.gameObject.SetActive(true);
                reactionMessageText.gameObject.SetActive(false); 
            }
            EnableOutline(interactMe);
           
        }
        else if (canInteract && interaction != null)
        {
            interactTxt.text = interaction.InteractionPrompt;
            interactTxt.gameObject.SetActive(true);
            reactionMessageText.gameObject.SetActive(false);
            EnableOutline(interaction as MonoBehaviour);
        }

        else
        {
            HideInteractionUI();
            DisableAllOutlines();
        }
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
        InteractMe interact = target.GetComponent<InteractMe>();
        if (npcInterract != null || (interact!=null &&!interact.enableOutline))
        {
            return;
        }


        if (outline == null)
        {
            outline = target.AddComponent<Outline>();
            outline.OutlineColor = Color.yellow; 
            outline.OutlineWidth = 10.0f; 
        }

       
        
        enabledOutlines.Add(outline);

        outline.enabled = true;
    }
  
    //hic optimize degiol degistirilmesi gerek 
    void DisableAllOutlines()
    {
       
        foreach (Outline outline in enabledOutlines)
        {
            if (outline != null)
            {
                outline.enabled = false;
            }
        }
        enabledOutlines.Clear();
    }


    public void HideInteractionUI()
    {
        interactTxt.gameObject.SetActive(false);
        reactionMessageText.gameObject.SetActive(false);
    }
}
