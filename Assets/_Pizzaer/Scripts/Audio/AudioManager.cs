using UnityEngine;

// STUB (A5): fields and public methods only. Viset writes the real code.
// Plays short sound effects. Music is a separate AudioSource with Loop + Play On Awake.
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioSource sfxSource;

    [Header("Clips")]
    public AudioClip pickupClip;
    public AudioClip deliverClip;
    public AudioClip wrongClip;
    public AudioClip winClip;

    // Play one sound effect.
    public void Play(AudioClip clip)
    {
    }
}
