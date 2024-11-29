using UnityEngine;

public class SetFrameRate : MonoBehaviour
{
    public int targetFrameRate = 60; // Desired frame rate

    void Start()
    {
        // Set the target frame rate
        Application.targetFrameRate = targetFrameRate;
    }
}
