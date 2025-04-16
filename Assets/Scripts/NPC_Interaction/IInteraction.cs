public interface IInteraction 
{
    string InteractionPrompt { get; }
    void Interact();
    void StopInteract();
}

