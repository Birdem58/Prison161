using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScene : MonoBehaviour
{

    public Animator animator;
    public GameObject canvasU�;

    public GameObject creditsCanvas;
    public GameObject controlsCanvas;


    private void Start()
    {
        canvasU�.SetActive(false);
        
        creditsCanvas.SetActive(false);
        controlsCanvas.SetActive(false);

        StartCoroutine(PlayAnimationThenShowUI());
    }

    private IEnumerator PlayAnimationThenShowUI()
    {
        animator.Play("bookOpening");

        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        canvasU�.SetActive(true);
    }

    public void ShowStartMenu()
    {
        //startCanvas.SetActive(true);
        //creditsCanvas.SetActive(false);
        //controlsCanvas.SetActive(false);
        //canvasU�.SetActive(false);
        SceneManager.LoadScene(1);
    }

    public void ShowCreditsMenu()
    {
        
        creditsCanvas.SetActive(true);
        controlsCanvas.SetActive(false);
        canvasU�.SetActive(false);
    }

    public void ShowControlsMenu()
    {
        
        creditsCanvas.SetActive(false);
        controlsCanvas.SetActive(true);
        canvasU�.SetActive(false);
    }
    public void BackToMainUI()
    {
      
        creditsCanvas.SetActive(false);
        controlsCanvas.SetActive(false);
        canvasU�.SetActive(true);
    }
    public void ExitGame()
    {
        Debug.Log("Oyun Kapan�yor...");
        Application.Quit();
    }


}
