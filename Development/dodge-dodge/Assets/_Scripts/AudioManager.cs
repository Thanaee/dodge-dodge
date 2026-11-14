using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
   public static AudioManager Instance { get; private set; }

   [SerializeField] private AudioSource musicSource;
   [SerializeField] private AudioClip backgroundMusic;
   [SerializeField] private AudioClip crashSound;

   void Awake()
   {
    if (Instance == null)
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    else
    {
        Destroy(gameObject);
    }
   }

    void Start()
    {
        if (musicSource != null && backgroundMusic != null && !musicSource.isPlaying)
        {
            musicSource.clip = backgroundMusic;
            musicSource.loop = true;
            musicSource.Play();
        }
    }

    public void PlayCrash()
    {
        if (musicSource != null && crashSound != null)
        {
            musicSource.PlayOneShot(crashSound);
        }
    }

    
}
