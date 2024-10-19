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
                ExitKeyholeView(); // Kap� deli�inden ��k
            }
            else
            {
                LookThroughKeyhole(); // Kap� deli�inden bak
            }
        }
    }

    void LookThroughKeyhole()
    {
        pressButUI.SetActive(false); // UI'� kapat
        player.GetComponent<CharacterController>().enabled = false; // Oyuncu hareketini devre d��� b�rak

       
        virtualCamera.Follow = doorViewPos;
        virtualCamera.LookAt = doorViewPos;

        isLookingThroughDoor = true; // Kap� deli�ine bakt���n� i�aretle
    }

    void ExitKeyholeView()
    {
        // Cinemachine kameran�n takip edece�i pozisyonu tekrar oyuncunun pozisyonuna d�nd�r
        virtualCamera.Follow = playerCameraPos;
        virtualCamera.LookAt = playerCameraPos;

        player.GetComponent<CharacterController>().enabled = true; // Oyuncu hareketini tekrar aktif et
        isLookingThroughDoor = false; // Kap� deli�inden ��kt���n� i�aretle

        if (isNearDoor)
        {
            pressButUI.SetActive(true); // Hala kap�ya yak�nsa UI'� g�ster
        }
    }
}


