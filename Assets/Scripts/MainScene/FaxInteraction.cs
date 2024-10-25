using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FaxInteraction : MonoBehaviour
{
    public float Distance = 5f;
    public TextMeshProUGUI interactText;
    public GameObject wallCollider;
    public List<GameObject> paperList;
    public int paperIndex = 0;
    private bool isTriggered = false;
    public Animator animator;

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (isTriggered && paperList[paperIndex].activeSelf)
        {
            interactText.text = "Press E to close";
            if (Input.GetKeyDown(KeyCode.E))
            {
                DropPaper();
            }
            return;
        }

        if (Physics.Raycast(ray, out hit, Distance))
        {
            if (hit.collider.CompareTag("Paper") && isTriggered)
            {
                interactText.enabled = true;

                if (Input.GetKeyDown(KeyCode.E))
                {
                    PickUpPaper();
                }
            }
            else
            {
                interactText.enabled = false;
            }
        }
        else
        {
            interactText.enabled = false;
        }
    }

    void PickUpPaper()
    {
        paperList[paperIndex].SetActive(true);
        interactText.text = "Press E to close";
        wallCollider.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        animator.SetInteger("doorPos",1);

    }

    public void DropPaper()
    {
        paperList[paperIndex].SetActive(false);
        interactText.enabled = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            isTriggered = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            isTriggered = false;
            DropPaper(); 
        }
    }
}
