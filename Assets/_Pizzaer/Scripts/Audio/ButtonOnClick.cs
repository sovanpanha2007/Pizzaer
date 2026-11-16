using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonOnClick : MonoBehaviour
{
    public void OnPlayButtonClicked()
    {
        AudioManager.Instance.PlayClick();

    }
}