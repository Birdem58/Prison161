using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using prison161.EventBus;
using StarterAssets;
using PuppetOfShadows.EventBinding;

public class SelectionInteraction : MonoBehaviour, IInteraction
{
    public string InteractionPrompt => isSitting ? "Stand Up [F]" : "Select People For Operation [F]";
    public Transform sitPosition;
    public CharacterController characterController;
    public float sittingHeight = 1f;
    private bool isSitting = false;
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private float originalHeight;
    public bool isSelectionOk = true;
    [SerializeField] GameObject selectionCharacters;

    private EventBinding<OnConfirmingSelection> eventBingind;
    private void OnEnable()
    {
        eventBingind = new EventBinding<OnConfirmingSelection>(OnSigned);
        EventBus<OnConfirmingSelection>.Register(eventBingind);
    }

    private void OnDisable()
    {
        EventBus<OnConfirmingSelection>.Deregister(eventBingind);
    }

    private void OnSigned()
    {

        //seledction bittikten sonra baska olaylari da yerine getirebiliriz boylece
        GetUpFromTable();
    }
    private void SetCharacterTransform(Vector3 newPosition, Quaternion newRotation)
    {
        characterController.enabled = false;
        characterController.transform.position = newPosition;
        characterController.transform.rotation = newRotation;
        characterController.enabled = true;
    }

    void Sit()
    {
        isSitting = true;
        originalPosition = characterController.transform.position;
        originalRotation = characterController.transform.rotation;
        SetCharacterTransform(sitPosition.position, sitPosition.rotation);
        

    }

    void StandUp()
    {
        isSitting = false;
        SetCharacterTransform(originalPosition, originalRotation);
        
    }

    public virtual void Interact()
    {
        Debug.Log("interact basildi");
        if (isSelectionOk)
        {
            if (isSitting)
            {
                GetUpFromTable();
                SelectionMechanizm.Instance.ToggleUI(false);
                CanShowCharacters(isSelectionOk);
                return;
            };

            SelectionMechanizm.Instance.ToggleUI(true);
            EventBus<AnchorChracter>.Raise(new AnchorChracter(true));
            Sit();
            CanShowCharacters(isSitting);
        }
    }

    public void GetUpFromTable()
    {
        if (!isSitting) return;

        EventBus<AnchorChracter>.Raise(new AnchorChracter(false));
        StandUp();
    }

    public void CanShowCharacters(bool state)
    {
       selectionCharacters.SetActive(state);
    }
    public virtual void StopInteract()
    {
    }
}