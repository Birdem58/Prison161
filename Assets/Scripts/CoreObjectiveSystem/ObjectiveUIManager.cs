using TMPro;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class ObjectiveUIManager : MonoBehaviour
{
    [SerializeField] private Canvas objectiveCanvas;
    [SerializeField] private Image ObjectiveContainer;
    [SerializeField] private TextMeshProUGUI objectivesText;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip completionSound;

    private string currentObjectiveId;

    public void UpdateObjectives(List<Objective> objectives)
    {
        if (objectivesText == null || objectives == null || objectives.Count == 0)
        {
            DisableUI();
            return;
        }

        Objective currentObjective = objectives.Find(obj => !obj.isCompleted);

        if (currentObjective == null)
        {
            objectivesText.text = "All Objectives Completed!";
            objectivesText.color = Color.green;
            PlayCompletionSound();
            DisableUI();
            return;
        }

        objectivesText.text = currentObjective.description;
        objectivesText.color = Color.white;
        EnableUI();
    }

    public void EnableUI()
    {
        if (objectiveCanvas != null) objectiveCanvas.enabled = true;
        if (ObjectiveContainer != null) ObjectiveContainer.enabled = true;
    }

    public void DisableUI()
    {
        if (objectiveCanvas != null) objectiveCanvas.enabled = false;
        if (ObjectiveContainer != null) ObjectiveContainer.enabled = false;
    }

    private void PlayCompletionSound()
    {
        if (audioSource != null && completionSound != null)
        {
            audioSource.PlayOneShot(completionSound);
        }
    }
}