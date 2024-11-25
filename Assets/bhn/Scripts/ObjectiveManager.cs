using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ObjectiveManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI objectiveText; // UI'daki metin alanı
    private int currentObjectiveIndex = 0;

    // Objective listesi
    private string[] objectives = new string[]
    {
        "Objective 1: Check the fax machine.",
        "Objective 2: Talk to Person 1.",
        "Objective 3: Talk to Person 2.",
        "Objective 4: Talk to Person 3.",
        "Objective 5: Go to Operation Room.",
        "Objective 6: Go to bed."
    };

    private void Start()
    {
        // İlk objective'i ayarla
        UpdateObjective();
    }

    public void CompleteObjective()
    {
        if (currentObjectiveIndex < objectives.Length - 1)
        {
            currentObjectiveIndex++; // Sonraki objective'e geç
            UpdateObjective();
        }
        else
        {
            // Tüm objective'ler tamamlandı
            objectiveText.text = "All objectives completed! Well done!";
        }
    }

    private void UpdateObjective()
    {
        objectiveText.text = objectives[currentObjectiveIndex];
    }
}
