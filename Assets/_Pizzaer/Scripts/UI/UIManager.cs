using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

// Shows everything on screen in the Town scene. Lives on UI_Canvas.
public class UIManager : MonoBehaviour
{
    [Header("HUD")]
    public TMP_Text moneyText;
    public TMP_Text orderText;
    public GameObject promptPanel;      // "Press E"

    [Header("Dialogue")]
    public GameObject dialoguePanel;
    public TMP_Text speakerText;
    public TMP_Text lineText;
    public float dialogueSeconds = 3f;

    [Header("Win")]
    public GameObject winPanel;
    public TMP_Text winMoneyText;

    Coroutine hideRoutine;

    void Awake()
    {
        SetActiveSafe(promptPanel, false);
        SetActiveSafe(dialoguePanel, false);
        SetActiveSafe(winPanel, false);
    }

    // Until the real UI is built, missing fields only log instead of crashing.
    static void SetActiveSafe(GameObject go, bool active)
    {
        if (go != null) go.SetActive(active);
    }

    static void SetTextSafe(TMP_Text label, string text)
    {
        if (label != null) label.text = text;
        else Debug.Log("[UI] " + text);
    }

    public void SetMoney(int amount)
    {
        SetTextSafe(moneyText, "$" + amount);
    }

    public void SetOrder(string text)
    {
        SetTextSafe(orderText, text);
    }

    public void ShowPrompt(bool show)
    {
        SetActiveSafe(promptPanel, show);
    }

    // Show a line of dialogue, then hide it after a few seconds.
    public void ShowDialogue(string speaker, string line)
    {
        if (speakerText != null && lineText != null)
        {
            speakerText.text = speaker;
            lineText.text = line;
        }
        else Debug.Log("[UI] " + speaker + ": " + line);
        SetActiveSafe(dialoguePanel, true);

        if (hideRoutine != null) StopCoroutine(hideRoutine);
        hideRoutine = StartCoroutine(HideDialogueLater());
    }

    IEnumerator HideDialogueLater()
    {
        yield return new WaitForSecondsRealtime(dialogueSeconds);
        SetActiveSafe(dialoguePanel, false);
    }

    // Button: Hide Dialogue
    public void HideDialogue()
    {
        if (hideRoutine != null)
        {
            StopCoroutine(hideRoutine);
            hideRoutine = null;
        }

        SetActiveSafe(dialoguePanel, false);
    }

    public void ShowWin(int totalMoney)
    {
        SetTextSafe(winMoneyText, "Shop saved! You earned $" + totalMoney + "!");
        SetActiveSafe(promptPanel, false);
        SetActiveSafe(winPanel, true);

        Cursor.lockState = CursorLockMode.None; // let the player click buttons
        Cursor.visible = true;
        Time.timeScale = 0f;                     // freeze the game
    }

    // Button: Play Again
    public void PlayAgain()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Town");
    }

    // Button: Main Menu
    public void BackToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
