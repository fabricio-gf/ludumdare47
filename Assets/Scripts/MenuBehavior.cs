using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuBehavior : MonoBehaviour
{

    public Animator menuAnimator;

    public GameObject mainCanvas;
    public GameObject creditsCanvas;
    
    
    private static readonly int OpenCredits = Animator.StringToHash("OpenCredits");

    public void PressPlay()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void PressCredits()
    {
        mainCanvas.SetActive(false);
        creditsCanvas.SetActive(true);
    }

    public void PressBack()
    {
        mainCanvas.SetActive(true);
        creditsCanvas.SetActive(false);
    }
}
