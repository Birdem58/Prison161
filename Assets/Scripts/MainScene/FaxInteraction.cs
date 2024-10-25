using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FaxInteraction : MonoBehaviour, IInteraction
{
    public GameObject wallCollider;
    public List<GameObject> paperList;
    public int paperIndex = 0;
    private bool isTriggered = false;
    public Animator animator;


    void PickUpPaper()
    {
        PlayerState.Instance.SetState(PlayerState.State.DIALOGUE);
        paperList[paperIndex].SetActive(true);   
        wallCollider.SetActive(false);
        animator.SetInteger("doorPos",1);
    }

    public void DropPaper()
    {
        paperList[paperIndex].SetActive(false);
        PlayerState.Instance.SetState(PlayerState.State.DEFAULT);
    }


    public void Interact()
    {
        if(!isTriggered){
            isTriggered = true;
            PickUpPaper();
        }else StopInteract();
    }

    public void StopInteract()
    {
        isTriggered = false;
        DropPaper();
    }
}
