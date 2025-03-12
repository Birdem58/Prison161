using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEditorInternal.ReorderableList;

public class JournalDayCounter : MonoBehaviour
{
    private TextMeshProUGUI textMeshProUGUI;
    private string today = "-Default -";
    private void OnEnable()
    {
        GameManager.OnGameStateChange += WhenGameStateChange;
    }

    private void OnDisable()
    {
        GameManager.OnGameStateChange -= WhenGameStateChange;
    }

    // Start is called before the first frame update
    void Start()
    {
        textMeshProUGUI = GetComponent<TextMeshProUGUI>();
        if (GameManager.Instance != null)
        {
            WhenGameStateChange(GameManager.Instance.State); 
        }
    }

    private void WhenGameStateChange(GameManager.GameState newState)
    {
        today = newState switch
        {
            GameManager.GameState.FirstDay => "-Day1-",
            GameManager.GameState.FirstNight => "-Night1-",
            GameManager.GameState.SecondDay => "-Day2-",
            GameManager.GameState.SecondNight => "-Night2-",
            GameManager.GameState.ThirdDay => "-Day3-",
            GameManager.GameState.ThirdNight => "-Night3-",
            GameManager.GameState.FourthDay => "-Day4-",
            GameManager.GameState.FourthNight => "-Night4-",
            GameManager.GameState.FifthDay => "-Day5-",
            GameManager.GameState.FifthNight => "-Night5-",
            _ => "YOK"
        };
        Debug.Log($"New State: {newState}");
        textMeshProUGUI.text = today;   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
