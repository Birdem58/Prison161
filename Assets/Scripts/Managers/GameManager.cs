using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
 
    public FaxInteraction faxInteraction;
    

    public GameState State;


    public static event Action<GameState> OnGameStateChange;
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
        CameraEffectsManager.instance.darkessEvents.ondarknessBegin += () =>
        {
            OnUpdateGameState(newState);
        };

        CameraEffectsManager.instance.ToggleDarkScreen(true);
    }


    public void OnUpdateGameState(GameState newState)
    {

        //Check if scene is looaded unload it if it is
        if (SceneManager.GetSceneByName(State.ToString()).isLoaded)
        {
            SceneManager.UnloadSceneAsync(State.ToString());
        }

        State = newState;

        SceneManager.LoadSceneAsync(State.ToString(), LoadSceneMode.Additive).completed += (AsyncOperation obj) =>
        {
            CameraEffectsManager.instance.ToggleDarkScreen(false);
        };


        OnGameStateChange?.Invoke(State);
    }

    public enum GameState
    {
        FirstDay,
        FirstNight,
        SecondDay,
        ThirdDay,
        GriKovaliyor,
        GriKaciyor,
        GriKazandi,
        GriKaybetti,


    }

}