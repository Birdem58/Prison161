using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllwaysFacePlayer : MonoBehaviour
{
    private Transform playerTransform;
    // Start is called before the first frame update
    void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            playerTransform = playerObject.transform;
        }
        else
        {
            Debug.LogError("No player found");
        }
    }

    void Update()
    {
        if (playerTransform != null)
        {

            Vector3 directionToPlayer = playerTransform.position - transform.position;

            
            directionToPlayer.y = 0;

            
            if (directionToPlayer != Vector3.zero)
            {

                Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
                transform.rotation = targetRotation;
            }
        }
    }
}

