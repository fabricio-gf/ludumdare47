using UnityEngine;
using UnityEngine.UI;

//This class has a custom GUI draw call. It can be found at the class ConditionalHide. 
public class AddLoadBehaviourToButton : MonoBehaviour
{
    private Button ThisButton = null;

    [SerializeField] private bool AddRestartScene = false;
    [SerializeField] private bool AddLoadScene = false;
    [SerializeField] private string SceneName = null;
    [SerializeField] private int TrackIndexToPlay = 0;

    // Start is called before the first frame update
    void Awake()
    {
        ThisButton = GetComponent<Button>();

        if (AddRestartScene)
        {
            ThisButton.onClick.AddListener(() => SceneLoader.Instance.RestartCurrentScene());
        }

        else if (AddLoadScene)
        {
            //ThisButton.onClick.AddListener(() => SceneLoader.Instance.LoadScene(SceneName));
            ThisButton.onClick.AddListener(() => SceneLoader.Instance.LoadSceneAsync(SceneName, TrackIndexToPlay));
        }
    }
}
