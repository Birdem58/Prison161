using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using prison161.EventBus;
using StarterAssets;

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
        if (isSitting)
        {
            GetUpFromTable();
            SelectionMechanizm.Instance.ToggleUI(false);
            return;
        };
        SelectionMechanizm.Instance.ToggleUI(true);
        EventBus<AnchorChracter>.Raise(new AnchorChracter(true));
        Sit();
    }

    public void GetUpFromTable()
    {
        if (!isSitting) return;

        EventBus<AnchorChracter>.Raise(new AnchorChracter(false));
        StandUp();
    }

    public virtual void StopInteract()
    {
    }
}