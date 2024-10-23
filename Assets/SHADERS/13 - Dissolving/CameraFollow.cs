using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private GameObject player;
    // Follow target
    private Transform target = null;
    // Reference to local transform
    private Transform thisTransform = null;

    // Final player positions
    [SerializeField] private Vector3 offset;
    float smoothing = 5f;

    private Vector3 previousTargetPosition;
    // --------------------------------------------------------
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        target = player.GetComponent<Transform>();
        // Get transform for camera
        thisTransform = GetComponent<Transform>();
    }
    private void Start()
    {
        //offset = new Vector3(0.0f, 4.0f, -3.6f);
    }
    // -------------------------------------------------------- 
    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 targetPosition = player.transform.position;
        if (targetPosition != previousTargetPosition)
        {
            FollowingPlayer();
            previousTargetPosition = targetPosition;
        }
        
    }
    // -------------------------------------------------------- 
    // Let's follow the player
    void FollowingPlayer()
    {
        Vector3 targetCamPos = new Vector3(target.position.x + offset.x, offset.y, target.position.z + offset.z);
        thisTransform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);
        thisTransform.LookAt(target);
    }
}
