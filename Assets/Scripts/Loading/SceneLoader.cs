using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;

    private CurrentScene _currentSceneScript;

    public LoadingBehavior loadingBehavior;

    public string previousSceneName;

    public List<AudioClip> levelTracks;
    public float[] levelTracksLoopTimes;

    private int _trackIndex = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        _currentSceneScript = GetComponent<CurrentScene>();
    }

    // Non-async functions
    
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void RestartCurrentScene()
    {
        SceneManager.LoadScene(_currentSceneScript.GetCurrentSceneIndex());
    }

    // Async functions
    // To load a scene asynchronously the order is
    // LoadSceneAsync > AddLoadingScene > AddScene > RemovePreviousScene
    
    public void LoadSceneAsync(string sceneName, int trackIndex)
    {
        previousSceneName = SceneManager.GetActiveScene().name;
        _trackIndex = trackIndex;
        
        print("Previous scene name - " + previousSceneName);
        
        StartCoroutine(AddLoadingScene(sceneName));
    }

    IEnumerator AddLoadingScene(string sceneName)
    {
        yield return SceneManager.LoadSceneAsync("LoadingScreen", LoadSceneMode.Additive);
        
        loadingBehavior = FindObjectOfType<LoadingBehavior>();
        
        yield return new WaitForSeconds(1f);
        
        StartCoroutine(AddScene(sceneName));
    }

    IEnumerator AddScene(string sceneName)
    {
        yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        
        // artificial load time
        yield return new WaitForSeconds(2f);
        
        StartCoroutine(RemovePreviousScene());
    }

    IEnumerator RemovePreviousScene()
    {
        yield return SceneManager.UnloadSceneAsync(previousSceneName);
        
        loadingBehavior.LevelOpened();
        
        AudioManager.instance.musicController.ChangeTrackBlend(levelTracks[_trackIndex], levelTracksLoopTimes[_trackIndex]);
        
        yield return new WaitForSeconds(1f);
        
        yield return SceneManager.UnloadSceneAsync("LoadingScreen");

        AudioManager.instance.musicController.ForceAddListener();
        AudioManager.instance.effectsController.ForceAddListener();
        //trigger level start
    }

}
