using UnityEngine;

public class InspectUIManager : MonoBehaviour
{
    public static InspectUIManager Instance { get; private set; }

    [SerializeField] private GameObject inspectBackground; // Gölgeli arka plan 

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void ShowInspectUI()
    {
        if (inspectBackground != null)
            inspectBackground.SetActive(true);
    }

    public void HideInspectUI()
    {
        if (inspectBackground != null)
            inspectBackground.SetActive(false);
    }
}
