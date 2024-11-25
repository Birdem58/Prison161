using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class ObjectiveUIManager : MonoBehaviour
{
    [SerializeField] private Canvas objectiveCanvas;
    [SerializeField] private TextMeshProUGUI objectivesText;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip completionSound;

    private string currentObjectiveId;

    public void UpdateObjectives(List<Objective> objectives)
    {
        if (objectivesText == null || objectives == null || objectives.Count == 0) return;
        Objective currentObjective = objectives.Find(obj => !obj.isCompleted);

        if (currentObjective == null)
        {
            currentObjective = objectives[objectives.Count - 1];
        }


        if (currentObjective.isCompleted && currentObjective.id != currentObjectiveId)
        {
            PlayCompletionSound();

            currentObjectiveId = currentObjective.id;


            objectivesText.text = currentObjective.description;
            objectivesText.color = currentObjective.isCompleted ? Color.green : Color.white;


            if (objectiveCanvas != null)
            {
                objectiveCanvas.enabled = true;
            }
        }

       
    }
    private void PlayCompletionSound()
    {
        if (audioSource != null && completionSound != null)
        {
            audioSource.PlayOneShot(completionSound);
        }
    }
}