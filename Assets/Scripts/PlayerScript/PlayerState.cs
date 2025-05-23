using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public enum State
    {
        DEFAULT,
        DIALOGUE,
        INSPECT,
        NONE
        
    }

    //Singleton
    public static PlayerState Instance;
    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        } 
    }

    public State state = State.DEFAULT;

    public void SetState(State newState)
    {
        state = newState;
        //turn off character controller in NONE and DIALOGUE state
        switch (state)
        {
            case State.DEFAULT:
                SetCharacterController(true);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                break;
            case State.DIALOGUE:
                SetCharacterController(false);
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;
                break;
            case State.INSPECT:
                SetCharacterController(false);
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = false;
                break;
            case State.NONE:
                SetCharacterController(false);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                break;
        }
    }

    public State GetState()
    {
        return state;
    }

    public void GetStateInfo()
    {
        Debug.Log(state);
    }
    public void SetDefaultState()
    {
        state = State.DEFAULT;
    }

    public void SetDialogueState()
    {
        state = State.DIALOGUE;
    }
    public void SetInspectState()
    {
        state = State.INSPECT;
    }


    public void SetNoneState()
    {
        state = State.NONE;
    }



    public void SetCharacterController(bool value)
    {
      //  Debug.Log("SetCharacterController: " + value);
        GetComponent<FirstPersonController>().enabled = value;
        GetComponent<CharacterController>().enabled = value;
    }



}
