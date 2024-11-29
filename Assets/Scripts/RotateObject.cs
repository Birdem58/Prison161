using UnityEngine;

public class RotateObject : MonoBehaviour
{
    public float rotationSpeed = 50f; // Speed of rotation in degrees per second

    void Update()
    {
        // Rotate the object around the y-axis
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
}
