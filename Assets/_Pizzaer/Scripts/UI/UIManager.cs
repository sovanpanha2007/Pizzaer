using TMPro;
using UnityEngine;

// STUB (A5): fields and public methods only. Panha writes the real code in D2.
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

    // Show the money number in the HUD.
    public void SetMoney(int amount)
    {
    }

    // Show the current order text in the HUD.
    public void SetOrder(string text)
    {
    }

    // Show or hide the "Press E" prompt.
    public void ShowPrompt(bool show)
    {
    }

    // Show a line of dialogue, then hide it after a few seconds.
    public void ShowDialogue(string speaker, string line)
    {
    }

    // Show the win panel with the total money.
    public void ShowWin(int totalMoney)
    {
    }

    // Button: Play Again
    public void PlayAgain()
    {
    }

    // Button: Main Menu
    public void BackToMenu()
    {
    }
}
