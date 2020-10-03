using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public float remainingTime;

    public bool isGamePaused = false;

    public GameObject[] gameCanvases;
    public GameObject pauseCanvas;
    public GameObject storeCanvas;

    public enum GameState
    {
        Waiting,
        Playing,
        Store
    }
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isGamePaused = !isGamePaused;
            if (isGamePaused)
            {
                Time.timeScale = 0;
                foreach (var canvas in gameCanvases)
                {
                    canvas.SetActive(false);
                }
                pauseCanvas.SetActive(true);
            }
            else
            {
                Time.timeScale = 1;
                pauseCanvas.SetActive(false);
                foreach (var canvas in gameCanvases)
                {
                    canvas.SetActive(true);
                }
            }
        }
    }

    public void InitiateRound()
    {
        
    }

    public void ResumeGame()
    {
        
    }

    public void GoToMainMenu()
    {
        
    }

    public void QuitGame()
    {
        
    }
    
}
