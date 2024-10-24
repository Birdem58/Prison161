using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraEffectsManager : MonoBehaviour
{
    //singleton
    public static CameraEffectsManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }
    }
    
}
