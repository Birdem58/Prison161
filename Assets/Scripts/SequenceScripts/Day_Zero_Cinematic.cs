using Cinemachine;
using UnityEditor.Rendering.Universal.ShaderGUI;
using UnityEngine;
using UnityEngine.Playables;
using VIDE_Data; 

public class OpeningCinematic : MonoBehaviour
{
    public PlayableDirector director;
    public GameObject playerCamera;
    public Camera mainCamera;
    public CinemachineVirtualCameraBase cinematicCam;
    public GameObject targetCharacter;



    void Start()
    {
        
        // Oyun baþýnda kontrolleri ve kameralarý kapat
        PlayerState.Instance.SetState(PlayerState.State.NONE);
        playerCamera.SetActive(false);

        // Timeline baþlat
        director.Play();

        // Timeline bittiðinde kontrolleri etkinleþtir
        director.stopped += OnTimelineFinished;
    }

    void OnTimelineFinished(PlayableDirector director)
    {

        float targetDistance = 3f; 

        Vector3 cinematicTarget = cinematicCam.transform.position + cinematicCam.transform.forward * targetDistance;


        mainCamera.transform.rotation = Quaternion.LookRotation(cinematicTarget - mainCamera.transform.position);

        playerCamera.SetActive(true);
        cinematicCam.Priority = 0;
        Debug.Log("TimelineisFÝnisgeed");
        NpcInterract npcInteract = targetCharacter.GetComponent<NpcInterract>();
        npcInteract.Interact();
    }


   
   
    }





  

    

    
