using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraEffectsManager : MonoBehaviour
{
    //singleton
    public static CameraEffectsManager instance;

    public DarkessEvents darkessEvents;
    [SerializeField] Animator DarkPanelAnimator;
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
    public void ToggleDarkScreen(bool isDark)
    {
        DarkPanelAnimator.SetBool("IsDark", isDark);
    }

}