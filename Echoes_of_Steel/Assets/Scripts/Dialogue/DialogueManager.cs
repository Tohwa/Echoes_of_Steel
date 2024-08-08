using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.AI;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [Header("Singleton")]
    public static DialogueManager instance;

    [Header("GameObjects")]
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private GameObject choices;
    [SerializeField] private GameObject panelObject;

    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI oneLinerText;
    [SerializeField] private TextMeshProUGUI choiceOneText;
    [SerializeField] private TextMeshProUGUI choiceTwoText;

    [Header("Images")]
    [SerializeField] private Image roboImage;
    [SerializeField] private Image childImage;
    [SerializeField] private Image memoryImage;

    [Header("Animators")]
    [SerializeField] private Animator boxAnimator;
    [SerializeField] private Animator imageAnimator;
    [SerializeField] private Animator journalAnimator;
    [SerializeField] private Animator oneLinerAnimator;

    [Header("Scripts")]
    [SerializeField] private GameUIHandler gameUIHandler;

    // Dialogue variables
    private DialogueAsset[] dialogueAssets;
    private DialogueAsset currentDialogueAsset;
    private int dialogueIndex;
    private int dialogueAssetIndex;
    public static bool isActive;
    private float dialogueSpeed = 50f;
    private bool autoContinue;
    private bool skipLineTriggered;
    private bool lineFinished;
    private bool showMemory;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
            return;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && isActive && !autoContinue)
        {
            if (!lineFinished)
            {

                SkipLine();
            }
            else if (lineFinished)
            {
                DisplayNextSentence();
            }
        }

    }

    public void StartDialogue(DialogueAsset[] _dialogues, bool _autoContinue, bool _pauseGame)
    {
        // Check if dialogue should pause game
        if (_pauseGame)
        {

            isActive = true;
            panelObject.SetActive(true);
        }

        boxAnimator.SetBool("IsOpen", true);

        choices.SetActive(false);

        showMemory = _pauseGame;
        dialogueAssets = _dialogues;
        autoContinue = _autoContinue;
        currentDialogueAsset = dialogueAssets[dialogueAssetIndex];

        DisplayNextSentence();
    }

    public void PlayOneLiner(string _oneLiner)
    {
        oneLinerAnimator.SetBool("IsOpen", true);

        StopAllCoroutines();
        StartCoroutine(TypeSentece(_oneLiner, oneLinerText, autoContinue));
    }


    public void DisplayNextSentence()
    {
        if (dialogueIndex > currentDialogueAsset.sentences.Length - 1)
        {
            EndDialogue();
            return;
        }

        // Show current memory image
        if (dialogueAssetIndex == 0 && dialogueIndex == 4 && showMemory)
        {
            memoryImage.sprite = currentDialogueAsset.memoryImage;
            imageAnimator.SetBool("IsOpen", true);
        }
        Dialogue currentDialogue = currentDialogueAsset.sentences[dialogueIndex];

        // Get visual feedback for current active speaker
        Color tempColor;
        if (currentDialogue.characterId == 0)
        {
            tempColor = roboImage.color;
            tempColor.a = 1f;
            roboImage.color = tempColor;

            tempColor = childImage.color;
            tempColor.a = 0.2f;
            childImage.color = tempColor;
        }
        else
        {
            tempColor = roboImage.color;
            tempColor.a = 0.2f;
            roboImage.color = tempColor;

            tempColor = childImage.color;
            tempColor.a = 1f;
            childImage.color = tempColor;
        }



        StopAllCoroutines();
        StartCoroutine(TypeSentece(currentDialogue.dialogue, dialogueText, autoContinue));

        dialogueIndex++;
    }

    /// <summary>
    /// Type sentence letter by letter
    /// </summary>
    /// <param name="_sentence">Sentence to be typed</param>
    /// <param name="_text">Target text component</param>
    /// <param name="_autoContinue">Automatically continue dialogue</param>
    /// <returns></returns>
    IEnumerator TypeSentece(string _sentence, TextMeshProUGUI _text, bool _autoContinue)
    {
        skipLineTriggered = false;
        lineFinished = false;
        _text.text = "";

        foreach (char letter in _sentence.ToCharArray())
        {
            if (skipLineTriggered)
            {
                _text.text = _sentence;
                break;
            }
            _text.text += letter;

            yield return new WaitForSeconds(1 / dialogueSpeed);

        }
        lineFinished = true;
        yield return new WaitForSeconds(3);
        oneLinerAnimator.SetBool("IsOpen", false);
        if (_autoContinue)
        {
            DisplayNextSentence();
        }

    }

    private void SkipLine()
    {
        skipLineTriggered = true;
    }

    private void EndDialogue()
    {
        // Displays two choices
        if (currentDialogueAsset.isChoiceDialogue)
        {
            choices.SetActive(true);

            choiceOneText.text = currentDialogueAsset.choiceOne;
            choiceTwoText.text = currentDialogueAsset.choiceTwo;

        }
        // Disables dialogue objects
        if (currentDialogueAsset.isEndDialogue)
        {

            isActive = false;
            imageAnimator.SetBool("IsOpen", false);
            boxAnimator.SetBool("IsOpen", false);
            panelObject.SetActive(false);

            // Checks if shielding & shooting have been unlocked
            if (currentDialogueAsset.enableShielding && currentDialogueAsset.enableShooting)
            {
                isActive = true;
                gameUIHandler.OpenPopUp(false);
            }
            // Enables shielding
            else if (currentDialogueAsset.enableShielding)
            {
                isActive = true;
                gameUIHandler.OpenPopUp(true);
            }
            dialogueAssetIndex = 0;
            currentDialogueAsset = dialogueAssets[dialogueAssetIndex];
        }
        dialogueIndex = 0;
        lineFinished = false;
        autoContinue = false;
        showMemory = false;
    }

    public void ChoiceOne()
    {
        if (dialogueAssetIndex == 1)
        {
            dialogueAssetIndex += 2;
        }
        else
        {
            dialogueAssetIndex++;
        }

        GameManager.Instance.corruptionMeter += 25;

        currentDialogueAsset = dialogueAssets[dialogueAssetIndex];
        choices.SetActive(false);
        DisplayNextSentence();
    }

    public void ChoiceTwo()
    {
        if (dialogueAssetIndex == 1)
        {
            dialogueAssetIndex += 3;
        }
        else
        {

            dialogueAssetIndex += 2;
        }

        GameManager.Instance.consciousMeter += 10;
        currentDialogueAsset = dialogueAssets[dialogueAssetIndex];
        choices.SetActive(false);
        DisplayNextSentence();
    }
}
