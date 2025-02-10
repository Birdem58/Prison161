using UnityEngine;
using System.Collections.Generic;
using System.Linq;

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

    [SerializeField] private List<List<Objective>> dayNightObjectives = new List<List<Objective>>();
    [SerializeField] private ObjectiveUIManager uiManager;

    private int dayNightCounter = 0;

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
        }
    }

    private void OnEnable()
    {
        GameManager.OnGameStateChange += HandleGameStateChange;
    }

    private void OnDisable()
    {
        GameManager.OnGameStateChange -= HandleGameStateChange;
    }

    private void Start()
    {
        if (dayNightObjectives.Count == 0)
        {
            AddObjectivesForDayNight(new List<Objective>
            {
                new Objective("day1_obj1","I should probably visit the Warden"),
                new Objective("day1_obj2","Whats Happening outside"),
                new Objective("day1_obj3","I should get to know them"),
                new Objective("day1_obj4","Whats with the fax machine")
            });

            AddObjectivesForDayNight(new List<Objective>
            {
                new Objective("night1_obj1", "read Fax"),
                new Objective("night1_obj2", "domal2")
            });
        }

        UpdateUIManager();
    }

    private void HandleGameStateChange(GameManager.GameState newState)
    {
        if (newState != GameManager.GameState.FirstDay)
            SwitchToNextDayNight();
    }

    private void AddObjectivesForDayNight(List<Objective> objectives)
    {
        dayNightObjectives.Add(objectives);
    }

    public void SwitchToNextDayNight()
    {
        if (dayNightCounter < dayNightObjectives.Count - 1)
        {
            dayNightCounter++;
            UpdateUIManager();
        }
        else
        {
            Debug.Log("No more days or nights left!");
            uiManager?.UpdateObjectives(new List<Objective>());
        }

     
    }

    private void UpdateUIManager()
    {
        if (uiManager != null)
        {
            uiManager.UpdateObjectives(dayNightObjectives[dayNightCounter]);
        }
    }

    public List<Objective> GetCurrentObjectives() => dayNightObjectives[dayNightCounter];

    public bool AreAllObjectivesComplete() => dayNightObjectives[dayNightCounter].All(obj => obj.isCompleted);

    public void CompleteObjective(string objectiveId)
    {
        var currentObjectives = dayNightObjectives[dayNightCounter];
        Objective objective = currentObjectives.Find(obj => obj.id == objectiveId);

        if (objective != null && !objective.isCompleted)
        {
            objective.isCompleted = true;
            UpdateUIManager();

            if (AreAllObjectivesComplete())
            {
                Debug.Log($"All objectives completed for Day/Night {dayNightCounter + 1}!");
              
            }
        }
    }
}
