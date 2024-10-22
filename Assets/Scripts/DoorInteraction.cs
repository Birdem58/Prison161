using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using StarterAssets;

public class DoorInteraction : MonoBehaviour
{
    
    public GameObject pressButUI; 
    public Transform doorViewPos; 
    public CinemachineVirtualCamera virtualCamera; 
    public Transform playerCameraPos;
    public FirstPersonController playerFpsController;
    public Canvas roomCanvas;
    public VIDEUIManager4 dialogueManager;
    private float moveSpeed;
    private float sprintSpeed;
    private bool isNearDoor = false; 
    private bool isLookingThroughDoor = false; 

    void Start()
    {
        moveSpeed = playerFpsController.GetComponent<FirstPersonController>().MoveSpeed  ;
        sprintSpeed = playerFpsController.GetComponent<FirstPersonController>().SprintSpeed  ;
        


        pressButUI.SetActive(false);
        roomCanvas.gameObject.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isNearDoor = true;
            pressButUI.SetActive(true); 
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isNearDoor = false;
            pressButUI.SetActive(false); 
        }
    }

    void Update()
    {
        if (isNearDoor && Input.GetKeyDown(KeyCode.E))
        {
            if (isLookingThroughDoor)
            {
                ExitKeyholeView();
                dialogueManager.EndCurrentDialogue();
            }
            else
            {
                LookThroughKeyhole(); 
            }
        }


        if(isLookingThroughDoor)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void LookThroughKeyhole()
    {
        pressButUI.SetActive(false);
        roomCanvas.gameObject.SetActive(true);
        

       playerFpsController.GetComponent<FirstPersonController>().MoveSpeed = 0; 
        playerFpsController.GetComponent<FirstPersonController>().SprintSpeed= 0; 

       
        virtualCamera.Follow = doorViewPos;
        virtualCamera.LookAt = doorViewPos;

        isLookingThroughDoor = true;
       
    }

    void ExitKeyholeView()
    {
       
        virtualCamera.Follow = playerCameraPos;
        virtualCamera.LookAt = playerCameraPos;
        roomCanvas.gameObject.SetActive(false);

        playerFpsController.GetComponent<FirstPersonController>().MoveSpeed = moveSpeed;
        playerFpsController.GetComponent<FirstPersonController>().SprintSpeed = sprintSpeed;
        isLookingThroughDoor = false;
        

        if (isNearDoor)
        {
            pressButUI.SetActive(true); 
        }
    }

}


