using UnityEngine;

public class GlobalVars : MonoBehaviour
{
    [SerializeField] private AudioClip soundHurt;
    public AudioClip SoundHurt => soundHurt;

    [SerializeField] private AudioClip soundDied;
    public AudioClip SoundDied => soundDied;

    public static GlobalVars Instance { get; private set; }

    // TODO use static methods for access static instance
    private void Awake() {
        Instance = this;
    }
}
