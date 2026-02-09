using UnityEngine;

public class sound : MonoBehaviour
{
    public static sound instance;
    private AudioSource audioSource;
    void Start()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            audioSource = GetComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ToggleSound()
    {
       if(audioSource != null)
        {
            audioSource.mute = !audioSource.mute;
        }
    }

     public bool IsMuted()
    {
        return audioSource != null && audioSource.mute;
    }


   
}
