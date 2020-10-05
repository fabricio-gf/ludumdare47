using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
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

    public GameObject defeatImage;
    public GameObject victoryImage;
    public TextMeshProUGUI scoreText;

    [Space(20)]
    public Image timerBarImg;
    public GameObject waitTime;
    public TMP_Text timeToWaitText;

    [Header("Balloons")] public List<GameObject> balloons;
    
    [Space(20)]
    [SerializeField] private ToiletIcon toiletIcon = null;
    private bool isCritical = false;  

    bool secondWind = true;

    public GameState gameState = GameState.Waiting;

    public float remainingTimePercent => remainingTime / startLoopTimeInSeconds;

    public Transform startPos;
    public Transform endPos;
    private float maxDistance;

    public Slider levelProgression;

    public int score = 0;

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
        
        if(SceneLoader.Instance != null && SceneLoader.Instance._gameFinished) scoreText.gameObject.SetActive(true);
    }

    private void Start()
    {
        //EnemyBlackboard.Instance.Initialize();
        UpgradesManager.Instance.UpdateMoneyText();
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
                HandleStore();
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

        EnemySpawner.Instance.canSpawn = true;
        
        player.GetComponent<Character2DController>().ResumePlayerMovement();
        
        ChangeGameStateTo(GameState.Playing);
        
        //play gameplay song
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
                toiletIcon.StartShaking();
            }
            else if (remainingTimePercent > 0.3f && isCritical)
            {
                isCritical = false;
                toiletIcon.StopShaking();
            }
        }
        else
        {
            ChangeGameStateTo(GameState.Store);
        }
    }

    void HandleStore()
    {
        if (!isStoreOpen)
        {
            isStoreOpen = true;
            Time.timeScale = 0;
            
            print("Starting end round delay");

            Animator playerAnimator = player.GetComponent<Animator>();
            playerAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
            
            playerAnimator.SetTrigger("Died");
            
            player.GetComponent<Character2DController>().HideHand();
            
            defeatImage.SetActive(true);
            //play defeat sound
            
            StartCoroutine(EndRoundDelay());
        }
    }

    IEnumerator EndRoundDelay()
    {
        yield return new WaitForSecondsRealtime(3f);

        player.GetComponent<Animator>().updateMode = AnimatorUpdateMode.Normal;
        OpenStore();
    }

    public void OpenStore()
    {
        foreach (var canvas in gameCanvases)
        {
            canvas.SetActive(false);
        }
        defeatImage.SetActive(false);
        victoryImage.SetActive(false);
        
        StoreBehavior.Instance.UpdateStore();
        storeCanvas.SetActive(true);
        
        //play store song
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

        
        player.transform.position = startPos.position;
        player.GetComponent<Animator>().SetTrigger("Reset");

        Character2DController character = player.GetComponent<Character2DController>();
        character.ResetVelocity();
        character.ResumePlayerMovement();
        character.ShowHand();

        UpgradesManager.Instance.UpdateMoneyText();
        SpawnBalloons();
        toiletIcon.StopShaking();

        var enemies = FindObjectsOfType<EnemyController>();
        foreach (var enemy in enemies)
        {
            Destroy(enemy.gameObject);
        }
        EnemySpawner.Instance.canSpawn = true;

        var coins = FindObjectsOfType<Pickuppable>();
        foreach (var coin in coins)
        {
            Destroy(coin.gameObject);
        }

        score = 0;
        scoreText.text = score.ToString();
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
        yield return new WaitForSecondsRealtime(1f);
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

    public void TriggerPlayerVictory()
    {
        ChangeGameStateTo(GameState.End);
        player.GetComponent<Character2DController>().StopPlayerMovement();
        victoryImage.SetActive(true);

        scoreText.text = score.ToString();
        scoreText.gameObject.SetActive(true);
        SceneLoader.Instance._gameFinished = true;
        //play victory song
    }

    public void AddScore(int value)
    {
        score += value;
        scoreText.text = score.ToString();
    }
}
