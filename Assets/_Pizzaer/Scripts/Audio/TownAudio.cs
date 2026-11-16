using UnityEngine;

public class TownAudio : MonoBehaviour
{
    private void Start()
    {
        AudioManager.Instance.PlayMusic(
            AudioManager.Instance.townMusic
        );
    }
}