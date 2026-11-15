using UnityEngine;

public class MainMenuAudio : MonoBehaviour
{
    private void Start()
    {
        AudioManager.Instance.PlayMusic(
            AudioManager.Instance.menuMusic
        );
        AudioManager.Instance.PlayClick();
    }
}