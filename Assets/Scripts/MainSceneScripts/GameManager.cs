using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    
    public GameState State;

    public static event Action<GameState> OnGameStateChange;

    private int dayNightCounter = -1; 

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        UpdateGameState(GameState.FirstDay);
    }

    public void UpdateGameState(GameState newState)
    {
        CameraEffectsManager.instance.darknessEvents.ondarknessBegin += () =>
        {
            OnUpdateGameState(newState);
        };

        CameraEffectsManager.instance.ToggleDarkScreen(true);
    }

    public void OnUpdateGameState(GameState newState)
    {
        
        if (SceneManager.GetSceneByName(State.ToString()).isLoaded)
        {
            SceneManager.UnloadSceneAsync(State.ToString()).completed += (AsyncOperation unloadOp) =>
            {
                LoadNextScene(newState);
            };
        }
        else
        {
            
            LoadNextScene(newState);
        }
    }


    private void LoadNextScene(GameState newState)
    {
        
        State = newState;
        SceneManager.LoadSceneAsync(State.ToString(), LoadSceneMode.Additive).completed += (AsyncOperation loadOp) =>
        {
            
            dayNightCounter++;
            Debug.Log($"Day&Night counter updated: {dayNightCounter}");

           
            CameraEffectsManager.instance.ToggleDarkScreen(false);

           
            OnGameStateChange?.Invoke(State);

            GameStateFunctions(State);
        };
    }

    public int GetDayNightCounter()
    {
        return dayNightCounter;
    }

    private void GameStateFunctions(GameState state)
    {
        switch (state)
        {
            case GameState.FirstDay:
                OnFirstDay();
                break;


            default:
                Debug.Log($"No specific logic for state: {state}");
                break;
        }
    }

    void OnFirstDay()
    {

    }


    public enum GameState
    {
        FirstDay,
        FirstNight,
        SecondDay,
        SecondNight,
        ThirdDay,
        ThirdNight,
        FourthDay,
        FourthNight,
        FifthDay,
        FifthNight,
    }
    //GameManager.Instance.UpdateGameState(GameManager.GameState.HangiG�n&gece);
}
