using UnityEngine;
using prison161.EventBus;

public class JournalInteraction : CollectibleItem
{
    public override string InteractionPrompt => "Take[F]";

    public override void Interact()
    {
        base.Interact();
        EventBus<GetJournal>.Raise(new GetJournal(true));
    }
}
