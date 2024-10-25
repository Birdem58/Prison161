using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class Objective
{
    public string id;
    public string description;
    public bool isCompleted;

    public Objective(string id, string description)
    {
        this.id = id;
        this.description = description;
        isCompleted = false;
    }
}

public class DaynNightManager : MonoBehaviour
{
    public static DaynNightManager Instance { get; private set; }

    [Header("Objectives Configuration")]
    [SerializeField] private List<Objective> objectives = new List<Objective>();

    [Header("UI Reference")]
    [SerializeField] private ObjectiveUIManager uiManager;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        
        if (objectives.Count == 0)
        {
            objectives.Add(new Objective("trigger_enter", "Enter the trigger area"));
            objectives.Add(new Objective("trigger_exit", "Exit the trigger area"));
        }

        UpdateUIManager();
    }

    public void CompleteObjective(string objectiveId)
    {
        Objective objective = objectives.Find(obj => obj.id == objectiveId);
        if (objective != null && !objective.isCompleted)
        {
            objective.isCompleted = true;
            UpdateUIManager();
            CheckAllObjectivesCompletion();
        }
    }

    private void CheckAllObjectivesCompletion()
    {
        if (AreAllObjectivesComplete())
        {
            Debug.Log("All objectives completed!");
            
        }
    }

    private void UpdateUIManager()
    {
        if (uiManager != null)
        {
            uiManager.UpdateObjectives(objectives);
        }
    }

    public List<Objective> GetCurrentObjectives() => objectives;

    public bool AreAllObjectivesComplete() => objectives.All(obj => obj.isCompleted);

    public bool IsObjectiveCompleted(string objectiveId)
    {
        var objective = objectives.Find(obj => obj.id == objectiveId);
        return objective != null && objective.isCompleted;
    }
}