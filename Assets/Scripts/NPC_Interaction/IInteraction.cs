using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteraction 
{
    string InteractionPrompt { get; }
    void Interact();
    void StopInteract();
}

