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

    private bool isNearDoor = false; 
    private bool isLookingThroughDoor = false; 

    void Start()
    {
        pressButUI.SetActive(false); 
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
                ExitKeyholeView(); // Kapý deliðinden çýk
            }
            else
            {
                LookThroughKeyhole(); // Kapý deliðinden bak
            }
        }
    }

    void LookThroughKeyhole()
    {
        pressButUI.SetActive(false); // UI'ý kapat
        player.GetComponent<CharacterController>().enabled = false; // Oyuncu hareketini devre dýþý býrak

       
        virtualCamera.Follow = doorViewPos;
        virtualCamera.LookAt = doorViewPos;

        isLookingThroughDoor = true; // Kapý deliðine baktýðýný iþaretle
    }

    void ExitKeyholeView()
    {
        // Cinemachine kameranýn takip edeceði pozisyonu tekrar oyuncunun pozisyonuna döndür
        virtualCamera.Follow = playerCameraPos;
        virtualCamera.LookAt = playerCameraPos;

        player.GetComponent<CharacterController>().enabled = true; // Oyuncu hareketini tekrar aktif et
        isLookingThroughDoor = false; // Kapý deliðinden çýktýðýný iþaretle

        if (isNearDoor)
        {
            pressButUI.SetActive(true); // Hala kapýya yakýnsa UI'ý göster
        }
    }
}


