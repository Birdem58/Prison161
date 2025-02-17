using UnityEngine;

public abstract class CollectibleItem : MonoBehaviour, IInteraction
{
    public abstract string InteractionPrompt { get; }

    public virtual void Interact()
    {
       
        Debug.Log("Item collected: " + gameObject.name);
    }

    public virtual void StopInteract() { }
}
