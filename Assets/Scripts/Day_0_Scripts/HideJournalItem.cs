using System.Collections;
using System.Collections.Generic;
using prison161.EventBus;
using PuppetOfShadows.EventBinding;
using UnityEngine;

public class HideJournalItem : MonoBehaviour
{
    private EventBinding<GetJournal> journalEventBinding;

    
    private void OnEnable()
    {
        journalEventBinding = new EventBinding<GetJournal>(OnDestroyJournalItem);
        EventBus<GetJournal>.Register(journalEventBinding);
    }

    
    private void OnDestroyJournalItem(GetJournal getJournal)
    {
        if(getJournal.journalEnable)
        {
            
            Destroy(gameObject);
        }
    }
}
