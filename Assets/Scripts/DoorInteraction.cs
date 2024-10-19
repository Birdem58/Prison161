using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;


public class DoorInteraction : MonoBehaviour
{
    
    public GameObject pressButUI; 
    public Transform doorViewPos; 
    public CinemachineVirtualCamera virtualCamera; 
    public Transform playerCameraPos;
    public GameObject player;
    public Canvas roomCanvas;

    private bool isNearDoor = false; 
    private bool isLookingThroughDoor = false; 

    void Start()
    {
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
            }
            else
            {
                LookThroughKeyhole(); 
            }
        }
    }

    void LookThroughKeyhole()
    {
        pressButUI.SetActive(false);
        roomCanvas.gameObject.SetActive(true);
        player.GetComponent<CharacterController>().enabled = false; 

       
        virtualCamera.Follow = doorViewPos;
        virtualCamera.LookAt = doorViewPos;

        isLookingThroughDoor = true;
        Cursor.lockState = CursorLockMode.None; 
        Cursor.visible = true;
    }

    void ExitKeyholeView()
    {
       
        virtualCamera.Follow = playerCameraPos;
        virtualCamera.LookAt = playerCameraPos;
        roomCanvas.gameObject.SetActive(false);
        player.GetComponent<CharacterController>().enabled = true;
        isLookingThroughDoor = false;
        Cursor.lockState = CursorLockMode.Locked; 
        Cursor.visible = false; 

        if (isNearDoor)
        {
            pressButUI.SetActive(true); 
        }
    }

}


