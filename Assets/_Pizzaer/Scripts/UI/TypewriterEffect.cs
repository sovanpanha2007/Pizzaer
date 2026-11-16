using System.Collections;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(AudioSource))]
public class TypewriterEffect : MonoBehaviour
{
    [Header("Settings")]
    public float typingSpeed = 0.04f;
    public float delayBetweenParts = 0.5f; // Dramatic pause before the mission text
    public AudioClip typeSound;

    [Header("Text Sequence")]
    // Drag your HookText and MissionText into this array in the Inspector
    public TMP_Text[] textSequence; 

    private AudioSource audioSource;
    private string[] fullTexts;
    private Coroutine typingCoroutine;
    private bool isSkipping = false;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        
        fullTexts = new string[textSequence.Length];
        for (int i = 0; i < textSequence.Length; i++)
        {
            fullTexts[i] = textSequence[i].text;
            textSequence[i].text = "";
        }
    }

    void OnEnable()
    {
        isSkipping = false;
        
        for (int i = 0; i < textSequence.Length; i++)
        {
            textSequence[i].text = "";
        }

        if (typingCoroutine != null) StopCoroutine(typingCoroutine);
        typingCoroutine = StartCoroutine(TypeSequence());
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isSkipping = true;
        }
    }

    IEnumerator TypeSequence()
    {
        for (int i = 0; i < textSequence.Length; i++)
        {
            TMP_Text currentText = textSequence[i];
            string targetText = fullTexts[i];

            foreach (char letter in targetText)
            {
                if (isSkipping) break; 

                currentText.text += letter;
                
                if (typeSound != null && letter != ' ')
                {
                    audioSource.PlayOneShot(typeSound, 0.5f);
                }
                
                yield return new WaitForSeconds(typingSpeed);
            }

            if (isSkipping)
            {
                for (int j = 0; j < textSequence.Length; j++)
                {
                    textSequence[j].text = fullTexts[j];
                }
                break; 
            }

            yield return new WaitForSeconds(delayBetweenParts);
        }
    }
}