using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuBehavior : MonoBehaviour
{

    public Animator menuAnimator;

    [Header("Canvases")]
    public GameObject mainCanvas;
    public GameObject instructionsCanvas;
    public GameObject creditsCanvas;
    public GameObject blackScreenCanvas;
    public GameObject backButton;

    [Header("Button Images")] 
    public Image playButton;
    public Image instructionsButton;
    public Image creditsButton;

    [Header("Sprites")]
    public Sprite pressedButton;
    public Sprite unpressedButton;

    public Sprite eatenCake;

    public void ChangeCakeSprite()
    {
        playButton.sprite = eatenCake;
    }
    
    public void PressInstructions()
    {
        bool state = !instructionsCanvas.activeSelf;
        backButton.SetActive(state);
        instructionsCanvas.SetActive(state);
        
        instructionsButton.sprite = state ? pressedButton : unpressedButton;
    }
    
    public void PressCredits()
    {
        bool state = !creditsCanvas.activeSelf;
        backButton.SetActive(state);
        creditsCanvas.SetActive(state);

        creditsButton.sprite = state ? pressedButton : unpressedButton;
    }

    public void PressBack()
    {
        backButton.SetActive(false);
        creditsCanvas.SetActive(false);
        instructionsCanvas.SetActive(false);

        instructionsButton.sprite = unpressedButton;
        creditsButton.sprite = unpressedButton;
    }

    public void PressQuit()
    {
        blackScreenCanvas.SetActive(true);
        StartCoroutine(QuitDelay());
    }

    IEnumerator QuitDelay()
    {
        yield return new WaitForSeconds(1f);
        Application.Quit();
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#endif
    }
}
