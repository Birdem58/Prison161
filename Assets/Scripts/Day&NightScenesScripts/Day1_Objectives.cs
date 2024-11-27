using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Day1_Objectives : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            DaynNightManager.Instance.CompleteObjective("day1_obj1");
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            DaynNightManager.Instance.CompleteObjective("day1_obj2");
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            GameManager.Instance.UpdateGameState(GameManager.GameState.FirstNight);
        }
    }
    }
    
