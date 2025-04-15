using System.Collections;
using Cinemachine;
using UnityEditor.Rendering.Universal.ShaderGUI;
using UnityEngine;
using UnityEngine.Playables;
using VIDE_Data;

public class OpeningCinematic : MonoBehaviour
{
    public PlayableDirector director;
    private GameObject playerCamera;
    private Camera mainCamera;
    public CinemachineVirtualCameraBase cinematicCam;
    public GameObject targetCharacter;

    void Start()
    {
        mainCamera = GameManager.Instance.mainCamera;
        playerCamera = GameManager.Instance.playerCamera;
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
        PlayerState.Instance.SetState(PlayerState.State.DEFAULT);
        Debug.Log(PlayerState.Instance.GetState());
        StartCoroutine(StartInteractionWithDelay());
    }

    IEnumerator StartInteractionWithDelay()
    {
        yield return new WaitForSeconds(1f); ;

        var npc = targetCharacter.GetComponent<NpcInterract>();
        npc.Interact();


        yield return null;

        if (VD.isActive)
        {
            VD.Next(); 
        }
    }

}










