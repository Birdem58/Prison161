using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VIDE_Data;

public class ButtonInteract : MonoBehaviour
{
    public VIDE_Assign dialogueAssign;
    public Template_UIManager uiManager;
    private bool dialogueStarted = false;

    public void OnButtonClick()
    {
        if (!dialogueStarted)  
        {
            VD.BeginDialogue(dialogueAssign);
            uiManager.Interact(dialogueAssign);
            dialogueStarted = true;  
        }
    }
}
