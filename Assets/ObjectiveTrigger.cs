using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        DaynNightManager.Instance.CompleteObjective("trigger_enter");
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        DaynNightManager.Instance.CompleteObjective("trigger_exit");
    }
}
