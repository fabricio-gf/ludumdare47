using UnityEngine;
using UnityEngine.SceneManagement;

public class CurrentScene : MonoBehaviour
{
    [SerializeField] private string currentSceneName;
    [SerializeField] private int currentSceneIndex;

    private void Awake()
    {
        currentSceneName = SceneManager.GetActiveScene().name;
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        SceneManager.sceneLoaded += ChangeSceneInfo;
    }

    public string GetCurrentSceneName()
    {
        return currentSceneName;
    }

    public int GetCurrentSceneIndex()
    {
        return currentSceneIndex;
    }

    private void ChangeSceneInfo(Scene scene, LoadSceneMode mode)
    {
        currentSceneName = scene.name;
        currentSceneIndex = scene.buildIndex;
    }
}
