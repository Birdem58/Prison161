using UnityEngine;

public interface IInspectable
{
    void Inspect();
}

public class InspectableObject : MonoBehaviour, IInspectable, IInteraction
{
    // IInteraction implementation: provide the prompt text.
    public string InteractionPrompt => "Inspect[F]";

    [Header("Inspect Settings")]
    [SerializeField] private float distanceFromCamera = 1f;   // How far from the camera the object will be placed
    [SerializeField] private float rotationSpeed = 100f;        // Speed of rotation with the mouse
    [SerializeField] private float scaleMultiplier = 1f;        // Optional scale adjustment

    private GameObject inspectClone;
    private bool isInspecting = false;
    private InteractionHandler interactionHandler; // Reference to disable player interactions

    // IInteraction implementation: when the player interacts, start inspection.
    public void Interact()
    {
        Inspect();
    }

    // IInteraction implementation: when stopping interaction, end inspection if active.
    public void StopInteract()
    {
        if (isInspecting)
            EndInspect();
    }

    // IInspectable implementation: start the inspection process.
    public void Inspect()
    {
        if (isInspecting) return;
        StartInspect();
    }

    private void StartInspect()
    {
        isInspecting = true;

        // Get and disable the player's interaction handler to stop further raycast UI updates.
        interactionHandler = FindObjectOfType<InteractionHandler>();
        if (interactionHandler != null)
        {
            // Hide any reaction/interact messages before disabling.
            interactionHandler.HideInteractionUI();
            interactionHandler.enabled = false;
        }

        // Set player state to INSPECT so that raycast-based messages won't appear.
        PlayerState.Instance.SetState(PlayerState.State.INSPECT);

        // Create a clone for inspection.
        inspectClone = Instantiate(gameObject);
        // Remove this script from the clone to avoid recursion.
        Destroy(inspectClone.GetComponent<InspectableObject>());

        // Disable colliders and physics on the clone.
        Collider[] cloneColliders = inspectClone.GetComponentsInChildren<Collider>();
        foreach (var col in cloneColliders)
            col.enabled = false;

        Rigidbody[] cloneRBs = inspectClone.GetComponentsInChildren<Rigidbody>();
        foreach (var rb in cloneRBs)
            rb.isKinematic = true;

        // --- Calculate the visual center of the clone ---
        Renderer[] renderers = inspectClone.GetComponentsInChildren<Renderer>();
        Bounds bounds;
        if (renderers.Length > 0)
        {
            bounds = renderers[0].bounds;
            for (int i = 1; i < renderers.Length; i++)
            {
                bounds.Encapsulate(renderers[i].bounds);
            }
        }
        else
        {
            bounds = new Bounds(inspectClone.transform.position, Vector3.zero);
        }
        // Compute the offset from the pivot to the visual center.
        Vector3 offset = bounds.center - inspectClone.transform.position;

        // --- Determine the desired world position for the clone's visual center ---
        // Get the center of the viewport at the specified distance.
        Vector3 desiredWorldCenter = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, distanceFromCamera));
        // Compute the new world position so that the clone's visual center aligns with the desired center.
        Vector3 newWorldPos = desiredWorldCenter - offset;
        inspectClone.transform.position = newWorldPos;

        // --- Preserve the clone's scale ---
        Vector3 originalScale = inspectClone.transform.localScale;

        // Parent the clone to the camera while preserving its world transform.
        inspectClone.transform.SetParent(Camera.main.transform, true);

        // Reset the clone's rotation and reapply its scale.
        inspectClone.transform.localRotation = Quaternion.identity;
        inspectClone.transform.localScale = originalScale * scaleMultiplier;

        // Show the inspection UI (for example, a tinted/blurred background).
        if (InspectUIManager.Instance != null)
            InspectUIManager.Instance.ShowInspectUI();
    }

    private void Update()
    {
        if (!isInspecting) return;

        // Rotate the inspected clone using mouse input.
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        if (inspectClone != null)
        {
            inspectClone.transform.Rotate(Camera.main.transform.up, -mouseX * rotationSpeed * Time.deltaTime, Space.World);
            inspectClone.transform.Rotate(Camera.main.transform.right, mouseY * rotationSpeed * Time.deltaTime, Space.World);
        }

        // Press Escape to exit inspection.
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            EndInspect();
        }
    }

    private void EndInspect()
    {
        if (inspectClone != null)
            Destroy(inspectClone);
        isInspecting = false;

        // Re-enable the player's interaction handler.
        if (interactionHandler != null)
            interactionHandler.enabled = true;

        // Reset the player state to DEFAULT so that raycast-based messages resume.
        PlayerState.Instance.SetState(PlayerState.State.DEFAULT);

        // Hide the inspection UI.
        if (InspectUIManager.Instance != null)
            InspectUIManager.Instance.HideInspectUI();
    }
}
