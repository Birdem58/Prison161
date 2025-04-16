using UnityEngine;

public class Safe : MonoBehaviour, IInteraction
{
    public string objectMessage;
    public string InteractionPrompt => objectMessage;
    public void Interact()
    { 
        //şifre ui panelini aç 
    }

    public void StopInteract()
    { 
        //şifre ui panelini kapat
    }
}
