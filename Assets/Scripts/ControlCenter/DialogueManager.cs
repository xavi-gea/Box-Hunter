using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    private bool isDialogueInProgress = false;
    private bool isSentenceInProgress = false;

    private string currentSentence;
    private Queue<string> dialogueLines = new();

    private Coroutine typingCoroutine;
    public float textDelay = 0.05f;

    public GameObject dialogueBoxPrefab;
    private GameObject dialogueBox;

    private Transform[] dialogueChildren;
    private TextMeshProUGUI dialogueSubject;
    private TextMeshProUGUI dialogueContent;
    private GameObject dialogueButton;

    private void Awake()
    {
        Instance = this;

        //submitAction = InputSystem.actions.FindAction("UI/Submit");
    }

    /// <summary>
    /// If not in combat and the game is not paused, toggle the current input map
    /// </summary>
    private void ToggleInputMap()
    {
        if (!CombatEncounterManager.Instance.isInCombat && 
            !ScreenManager.Instance.isGamePaused) {

            InputManager.Instance.ToggleInputMap();
        }
    }

    /// <summary>
    /// Instance a dialogue prefab with the data provided by <paramref name="dialogue"/> and place it in the relevant location
    /// </summary>
    /// <param name="dialogue"></param>
    public void StartDialogue(Dialogue dialogue)
    {
        if (isDialogueInProgress)
        {
            Destroy(dialogueBox);
        }
        else
        {
            isDialogueInProgress = true;
        }

        ToggleInputMap();

        dialogueLines.Clear();
        dialogueLines = new Queue<string>(dialogue.dialogueLines);

        if (CombatEncounterManager.Instance.isInCombat)
        {
            dialogueBox = Instantiate(dialogueBoxPrefab, GameObject.Find("CombatBottomScreen").transform);
        }
        else
        {
            dialogueBox = Instantiate(dialogueBoxPrefab, GameObject.Find("Canvas").transform);
        }

        dialogueChildren = dialogueBox.GetComponentsInChildren<Transform>();

        foreach (Transform child in dialogueChildren)
        {
            if (child.name.Equals("Subject"))
            {
                dialogueSubject = child.GetComponent<TextMeshProUGUI>();
                continue;
            }
            else if (child.name.Equals("Content"))
            {
                dialogueContent = child.GetComponent<TextMeshProUGUI>();
                continue;
            }
            else if (child.name.Equals("ButtonNext"))
            {
                dialogueButton = child.gameObject;
            }
        }

        if (!string.IsNullOrEmpty(dialogue.subject))
        {
            dialogueSubject.text = dialogue.subject;
        }
        else
        {
            dialogueSubject.text = null;
        }

        SetSelectedButton(dialogueButton);

        NextSentence();
    }

    /// <summary>
    /// Coroutine that makes every letter of the dialogue content be typed out with a delay
    /// </summary>
    /// <param name="sentence"></param>
    /// <returns></returns>
    private IEnumerator TypeLetters(char[] sentence)
    {
        dialogueContent.text = "";
        isSentenceInProgress = true;

        foreach (char letter in sentence)
        {
            dialogueContent.text += letter;
            yield return new WaitForSeconds(textDelay);
        }

        isSentenceInProgress = false;
    }


    /// <summary>
    /// If there is one, go to the next sentence
    /// If not, end the dialogue
    /// </summary>
    public void NextSentence()
    {
        if (Instance == this)
        {
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
                typingCoroutine = null;
            }

            if (isSentenceInProgress)
            {
                dialogueContent.text = currentSentence;
                isSentenceInProgress = false;
            }
            else
            {
                if (dialogueLines.Count > 0)
                {
                    currentSentence = dialogueLines.Dequeue();

                    char[] sentenceLetters = currentSentence.ToCharArray();
                    typingCoroutine = StartCoroutine(TypeLetters(sentenceLetters));
                }
                else
                {
                    EndDialogue();
                }
            }
        }
        else
        {
            Instance.NextSentence();
        }
    }

    /// <summary>
    /// Ends the currently opened dialogue
    /// </summary>
    private void EndDialogue()
    {
        ToggleInputMap();

        Destroy(dialogueBox);

        isDialogueInProgress = false;

        DialogueEvents.dialogueDone.Invoke();
    }

    /// <summary>
    /// Returns if there is an opened dialogue box
    /// </summary>
    /// <returns></returns>
    public bool GetIsDialogueInProgress()
    {
        return isDialogueInProgress;
    }

    /// <summary>
    /// Sets the currently selected button in the UI to the one that continues/ends the dialogue
    /// </summary>
    /// <param name="button"></param>
    private void SetSelectedButton(GameObject button)
    {
        EventSystem.current.SetSelectedGameObject(button);

        //var standaloneInputModule = EventSystem.current.currentInputModule as StandaloneInputModule;

        //if (standaloneInputModule != null)
        //{
        //    return;
        //}

        //EventSystem.current.SetSelectedGameObject(null);
    }
}