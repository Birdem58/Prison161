using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VIDE_Data;

public class NpcInterract : MonoBehaviour, IInteraction
{
    [SerializeField]VIDE_Assign videAssign;
    private VIDEUIManager4 diagUI;
    public string InteractionPrompt => "Talk";
    public void Interact()
    {
       PlayerState.Instance.SetState(PlayerState.State.DIALOGUE);
       diagUI.Interact(videAssign);
    }

    public void StopInteract()
    {
        PlayerState.Instance.SetState(PlayerState.State.DEFAULT);
    }

    // Start is called before the first frame update
    void Start()
    {
        diagUI = FindObjectOfType<VIDEUIManager4>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
