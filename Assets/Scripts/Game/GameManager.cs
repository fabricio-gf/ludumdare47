using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public float startLoopTimeInSeconds = 60f; 
    public float remainingTime;

    public float timeToWait = 3f;

    public bool isGamePaused = false;
    public bool isWaiting = true;

    public GameObject[] gameCanvases;
    public GameObject pauseCanvas;
    public GameObject storeCanvas;

    public Slider timerSlider;
    public GameObject waitTime;
    public TMP_Text timeToWaitText;

    public GameState gameState = GameState.Waiting;
    public enum GameState
    {
        Initiating,
        Waiting,
        Playing,
        Store,
        End
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

        HandleGameState();
    }

    void HandleGameState()
    {
        switch (gameState)
        {
            case GameState.Initiating:
                InitiateRound();
                break;
            case GameState.Playing:
                Playing();
                break;
            case GameState.Store:

                break;
            case GameState.Waiting:
                WaitToInitiateRound();
                break;
            case GameState.End:
                RestartLoop();
                break;
        }
    }

    void WaitToInitiateRound()
    {
        timeToWait -= Time.deltaTime;
        int time = (int)timeToWait;
        timeToWaitText.text = time.ToString();
        if (timeToWait <= 0)
        {
            isWaiting = false;
            waitTime.SetActive(false);
            ChangeGameStateTo(GameState.Initiating);
        }
    }

    public void InitiateRound()
    {
        remainingTime = startLoopTimeInSeconds;
        timerSlider.maxValue = startLoopTimeInSeconds;
        timerSlider.value = startLoopTimeInSeconds;
        ChangeGameStateTo(GameState.Playing);
    }

    void Playing()
    {
        if (remainingTime >= 0)
        {
            remainingTime -= Time.deltaTime;
            timerSlider.value = remainingTime;
        }
        else
        {
            ChangeGameStateTo(GameState.End);
        }
    }

    void RestartLoop()
    {
        SceneLoader.Instance.RestartCurrentScene();
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
    
    void ChangeGameStateTo(GameState gameState)
    {
        this.gameState = gameState;
    }
}
