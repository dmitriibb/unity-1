using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance {get; private set; }
    private AudioSource audioSource;

    private void Awake() {
        if (instance != null && instance != this) {
            Destroy(gameObject);
            return;
        }
        if (instance == null) {
            instance = this;
            audioSource = GetComponent<AudioSource>();
            DontDestroyOnLoad(gameObject);
        }
    }

    public void PlaySound(AudioClip sound)
    {
        audioSource.PlayOneShot(sound);
    }
}
