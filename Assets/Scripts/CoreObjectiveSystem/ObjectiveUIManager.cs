using TMPro;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class ObjectiveUIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI objectivesText;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip completionSound;

    public void UpdateObjectives(List<Objective> objectives)
    {
        if (objectivesText == null || objectives == null || objectives.Count == 0)
        {
            
            return;
        }

        string objectivesDisplay = "";
        bool hasDisplayedCurrent = false;

        foreach (var objective in objectives)
        {
            if (objective.isCompleted)
            {
                objectivesDisplay += $"- <s>{objective.description}</s>\n";
            }
            else if (!hasDisplayedCurrent)
            {
                objectivesDisplay += $"- {objective.description}\n";
                hasDisplayedCurrent = true;
            }
            else
            {
                break;
            }
        }

        objectivesText.text = objectivesDisplay;
    
    }

 

    private void PlayCompletionSound()
    {
        if (audioSource != null && completionSound != null)
        {
            audioSource.PlayOneShot(completionSound);
        }
    }
}
