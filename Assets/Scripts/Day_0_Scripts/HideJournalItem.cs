using System.Collections;
using System.Collections.Generic;
using prison161.EventBus;
using PuppetOfShadows.EventBinding;
using UnityEngine;

public class HideJournalItem : MonoBehaviour
{
    private void OnEnable()
    {
        EventBus<GetJournal>.Register(new EventBinding<GetJournal>(OnDestroyJournalItem)); 
    }

    private void OnDisable()
    {
        EventBus<GetJournal>.Deregister(new EventBinding<GetJournal>(OnDestroyJournalItem)); 
    }
    private void OnDestroyJournalItem(GetJournal getJournal)
    {
        if(getJournal.journalEnable)
        {
            
            Destroy(gameObject);
        }
    }
}
