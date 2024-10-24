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
    //public GameObject canvasWin;
    //public GameObject canvasLose;

    public static event Action<GameState> OnGameStateChange;
    private void Awake()
    {
        Instance = this;
        //canvasLose.SetActive(false);
        //canvasWin.SetActive(false);
    }
    private void Start()
    {
        UpdateGameState(GameState.FirstDay);


    }


    public void UpdateGameState(GameState newState)
    {
        State = newState;


        switch (newState)
        {
            case GameState.FirstDay:
                SceneManager.LoadScene("FirstDay", LoadSceneMode.Additive);
                break;
            case GameState.Firstnight:
                HandleSecondDaySecim();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);


        }
        OnGameStateChange?.Invoke(newState);
    }

    private void HandleSecondDaySecim()
    {
        
    }


    public enum GameState
    {
        FirstDay,
        Firstnight,
        SecondDay,
        ThirdDay,
        GriKovaliyor,
        GriKaciyor,
        GriKazandi,
        GriKaybetti,


    }

}