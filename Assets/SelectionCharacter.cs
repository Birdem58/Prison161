using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionCharacter : MonoBehaviour
{
    [SerializeField]string characterName;
    void Start()
    {
        if (PlayerPrefs.GetInt(characterName,0) == -1)
        {
            gameObject.SetActive(false);
        }
    }
    
}
