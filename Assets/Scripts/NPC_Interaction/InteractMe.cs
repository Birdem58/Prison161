using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractMe : MonoBehaviour, IInteraction
{
    public string objectMessage;
    public string InteractionPrompt => objectMessage;

    public void Interact()
    {
        
        return;
    }

    public void StopInteract()
    {
        return;
    }
}
