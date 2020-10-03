using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance = null;

    public MusicController musicController = null;
    public EffectsController effectsController = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
}
