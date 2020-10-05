using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject player;

    public float startLoopTimeInSeconds = 60f; 
    public float remainingTime;

    public float timeToWait = 100f;
    private float currentTime = 0;

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

    [Header("Balloons")] public List<GameObject> balloons;
    
    [Space(20)]
    [SerializeField] private Toilet toilet = null;
    private bool isCritical = false;

    bool secondWind = true;

    public GameState gameState = GameState.Waiting;
    public float remainingTimePercent => remainingTime / startLoopTimeInSeconds;

    public Transform startPos;
    public Transform endPos;
    private float maxDistance;

    public Slider levelProgression;

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

    private void Start()
    {
        SpawnBalloons();

        maxDistance = Vector2.Distance(startPos.position, endPos.position);
    }

    private void SpawnBalloons()
    {
        foreach (var t in balloons)
        {
            t.SetActive(Random.value < 0.3f);
        }
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
                UpdateProgressionToToilet();
                break;
            case GameState.Store:
                OpenStore();
                break;
            case GameState.End:
                break;
            
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    void WaitToInitiateRound()
    {
        if (!waitTime.activeSelf)
        {
            waitTime.SetActive(true);
            currentTime = timeToWait;
        }

        currentTime -= Time.deltaTime;
        timeToWaitText.text = Mathf.CeilToInt(currentTime).ToString();
        if (currentTime <= 0)
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
        print("Initiating round");
        
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

            if (UpgradesManager.Instance.angelPancake && secondWind)
            {
                if (remainingTimePercent <= 0.05f)
                {
                    secondWind = false;
                    SecondWind();
                }
            }

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
            StoreBehavior.Instance.UpdateStore();

            isStoreOpen = true;
            Time.timeScale = 0;
            
            print("Starting end round delay");

            Animator playerAnimator = player.GetComponent<Animator>();
            playerAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
            
            playerAnimator.Play("Dying");
            
            StartCoroutine(EndRoundDelay());
        }
    }

    IEnumerator EndRoundDelay()
    {
        yield return new WaitForSecondsRealtime(3f);

        player.GetComponent<Animator>().updateMode = AnimatorUpdateMode.Normal;
        
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
        ChangeGameStateTo(GameState.End);
        blackScreenCanvas.SetActive(true);
        StartCoroutine(FadeToBlackOnResetGame());
    }

    IEnumerator FadeToBlackOnResetGame()
    {
        yield return new WaitForSecondsRealtime(1f);
        
        //RESET GAME STUFF HERE
        foreach (var canvas in gameCanvases)
        {
            canvas.SetActive(true);
        }
        storeCanvas.SetActive(false);

        isStoreOpen = false;

        remainingTime = startLoopTimeInSeconds;
        timerBarImg.fillAmount = 1;
        
        player.GetComponent<Character2DController>().ResetVelocity();
        player.GetComponent<Animator>().Play("Idle");
        
        SpawnBalloons();
        
        //delete enemies
        //reset spawner
        
        //

        blackScreenCanvas.GetComponent<Animator>().SetTrigger("FadeOut");
        StartCoroutine(FadeBackOnResetGame());
        
    }

    IEnumerator FadeBackOnResetGame()
    {
        yield return new WaitForSecondsRealtime(1f);
        
        blackScreenCanvas.SetActive(false);
        
        Time.timeScale = 1;
        
        StartCountdown();
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

    public void IncreaseRemainingTime(float amount)
    {
        remainingTime += amount;
        if (remainingTime > 60f)
            remainingTime = 60;
    }

    void SecondWind()
    {
        Debug.Log("Second Wind");
        remainingTime = startLoopTimeInSeconds * 0.3f;
    }

    void UpdateProgressionToToilet()
    {
        if (player.transform.position.x <= maxDistance && player.transform.position.x <= endPos.position.x)
        {
            float distance = 1 - (Vector2.Distance(player.transform.position, endPos.position) / maxDistance);
            levelProgression.value = distance;
        }
    }
}
