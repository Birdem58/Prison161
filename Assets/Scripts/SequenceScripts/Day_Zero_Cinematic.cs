using System.Collections;
using System.Runtime.InteropServices;
using Cinemachine;
using prison161.EventBus;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using VIDE_Data;

public class OpeningCinematic : MonoBehaviour, IInteraction
{
    public PlayableDirector director;
    private GameObject playerCamera;
    private Camera mainCamera;
    private VIDEUIManager4 diagUiManager;
    private NpcInterract npcInterrac;
    public CinemachineVirtualCameraBase cinematicCam;
    public GameObject targetCharacter;
    public string InteractionPrompt => "Start Cinematic"; // Bo� b�rakmak yerine anlaml� bir metin

    public void Interact()
    {
        // IInteraction gere�i bu metodu implemente etmek zorunday�z,
        // ama bu cinematic ba�latma i�in kullan�lmayacak.
        // Timeline zaten Start()'ta ba�lat�l�yor.
    }

    public void StopInteract()
    {
        // Gerekirse cinematic'i durdurmak i�in kullan�labilir
    }

    void Start()
    {
        mainCamera = GameManager.Instance.mainCamera;
        playerCamera = GameManager.Instance.playerCamera;
        diagUiManager = GameManager.Instance.diagUiManager;

        
        if (playerCamera == null || diagUiManager == null || targetCharacter == null)
        {
            Debug.LogError("Critical references are missing!");
            return;
        }

        PlayerState.Instance.SetState(PlayerState.State.NONE);
        playerCamera.SetActive(false);
        director.Play();
        director.stopped += OnTimelineFinished;
    }

    void OnTimelineFinished(PlayableDirector director)
    {
        float targetDistance = 3f;
        Vector3 cinematicTarget = cinematicCam.transform.position + cinematicCam.transform.forward * targetDistance;
        mainCamera.transform.rotation = Quaternion.LookRotation(cinematicTarget - mainCamera.transform.position);

        playerCamera.SetActive(true);
        cinematicCam.Priority = 0;


        EventBus<SetCanInteract>.Raise(new SetCanInteract(true));

        npcInterrac = targetCharacter.GetComponent<NpcInterract>();
        EventBus<SetCanInteract>.Raise(new SetCanInteract(true));
        npcInterrac.Interact();
        EventBus<SetCanInteract>.Raise(new SetCanInteract(true));



        //// VIDE_Assign'� do�ru �ekilde al
        //VIDE_Assign npcDialogue = targetCharacter.GetComponent<VIDE_Assign>();
        //if (npcDialogue != null)
        //{

        //    Cursor.lockState = CursorLockMode.None;
        //    Cursor.visible = true;

        //    diagUiManager.Interact(npcDialogue);
        //    Debug.Log("VD Aktif Mi? " + VD.isActive);
        //}
        //else
        //{
        //    Debug.LogError($"VIDE_Assign component not found on {targetCharacter.name}!");
        //}
    }
}