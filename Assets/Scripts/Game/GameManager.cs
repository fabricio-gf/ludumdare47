using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public float startLoopTimeInSeconds = 60f; 
    public float remainingTime;

    public float timeToWait = 100f;

    public bool isGamePaused = false;
    public bool isWaiting = true;
    public bool isStoreOpen = false;

    [Header("Canvases")]
    public GameObject[] gameCanvases;
    public GameObject pauseCanvas;
    public GameObject storeCanvas;
    public GameObject blackScreenCanvas;

    [Space(20)]
    public Image timerBarImg;
    public GameObject waitTime;
    public TMP_Text timeToWaitText;
    
    [SerializeField] private Toilet toilet = null;
    private bool isCritical = false;  

    public GameState gameState = GameState.Waiting;
    public float remainingTimePercent => remainingTime / startLoopTimeInSeconds;
    public enum GameState
    {
        PreWaiting,
        Waiting,
        Initiating,
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

        if (SceneLoader.Instance != null) SceneLoader.Instance._onSceneReady += StartCountdown;
        else StartCountdown();
    }

    private void OnDisable()
    {
        if (SceneLoader.Instance != null) SceneLoader.Instance._onSceneReady -= StartCountdown;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }

        HandleGameState();
    }

    void TogglePause()
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

    void HandleGameState()
    {
        switch (gameState)
        {
            case GameState.PreWaiting:
                break;
            case GameState.Waiting:
                WaitToInitiateRound();
                break;
            case GameState.Initiating:
                InitiateRound();
                break;
            case GameState.Playing:
                Playing();
                break;
            case GameState.Store:
                OpenStore();
                break;
            case GameState.End:
                RestartLoop();
                break;
            
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    void WaitToInitiateRound()
    {
        if (!waitTime.activeSelf)
            waitTime.SetActive(true);

        timeToWait -= Time.deltaTime;
        int time = Mathf.CeilToInt(timeToWait);
        timeToWaitText.text = time.ToString();
        if (timeToWait <= 0)
        {
            isWaiting = false;
            waitTime.SetActive(false);
            ChangeGameStateTo(GameState.Initiating);
        }
    }
    
    public void StartCountdown()
    {
        ChangeGameStateTo(GameState.Waiting);
    }

    public void InitiateRound()
    {
        remainingTime = startLoopTimeInSeconds;
        timerBarImg.fillAmount = 1;
        ChangeGameStateTo(GameState.Playing);
    }

    void Playing()
    {
        if (remainingTime >= 0)
        {
            remainingTime -= Time.deltaTime;
            timerBarImg.fillAmount = remainingTimePercent;

            if (remainingTimePercent <= 0.3f && !isCritical)
            {
                isCritical = true;
                toilet.StartShaking();
            }
            else if (remainingTimePercent > 0.3f && isCritical)
            {
                isCritical = false;
                toilet.StopShaking();
            }
        }
        else
        {
            ChangeGameStateTo(GameState.Store);
        }
    }

    void OpenStore()
    {
        if (!isStoreOpen)
        {
            isStoreOpen = true;
            Time.timeScale = 0;

            print("Starting end round delay");
            StartCoroutine(EndRoundDelay());
        }
    }

    IEnumerator EndRoundDelay()
    {
        yield return new WaitForSecondsRealtime(2f);
        
        foreach (var canvas in gameCanvases)
        {
            canvas.SetActive(false);
        }
        storeCanvas.SetActive(true);
    }

    void RestartLoop()
    {
        SceneLoader.Instance.RestartCurrentScene();
    }

    public void PressResume()
    {
        TogglePause();
    }

    public void CloseStore()
    {
        
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
    
    void ChangeGameStateTo(GameState gameState)
    {
        this.gameState = gameState;
    }
}
