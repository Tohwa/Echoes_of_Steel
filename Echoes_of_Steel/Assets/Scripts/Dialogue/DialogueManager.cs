using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.AI;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private GameObject choices;
    [SerializeField] private GameObject panelObject;
    //[SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI oneLinerText;
    [SerializeField] private TextMeshProUGUI choiceOneText;
    [SerializeField] private TextMeshProUGUI choiceTwoText;
    [SerializeField] private Image roboImage;
    [SerializeField] private Image childImage;
    [SerializeField] private Image memoryImage;
    [SerializeField] private Animator boxAnimator;
    [SerializeField] private Animator imageAnimator;
    [SerializeField] private Animator journalAnimator;
    [SerializeField] private Animator oneLinerAnimator;
    [SerializeField] private GameUIHandler gameUIHandler;
    [SerializeField] private float dialogueSpeed;

    private DialogueAsset[] dialogueAssets;
    private DialogueAsset currentDialogueAsset;
    private bool autoContinue;
    private bool showMemory;
    //private DialogSettings dialogSettings;
    private bool skipLineTriggered;
    private bool lineFinished;
    private bool journalIsOpen;
    public static bool isActive;
    private int dialogueIndex;
    private int dialogueAssetIndex;



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
        //DontDestroyOnLoad(this);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && isActive)
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

        //if (isActive)
        //{
        //    Cursor.lockState = CursorLockMode.None;
        //    Cursor.visible = true;
        //}
        //else
        //{
        //    Cursor.lockState = CursorLockMode.Locked;
        //    Cursor.visible = false;
        //}
    }

    public void StartDialogue(DialogueAsset[] _dialogues, bool _autoContinue, bool _pauseGame)
    {
        if (_pauseGame)
        {

            isActive = true;
            panelObject.SetActive(true);
        }
        //dialogueBox.SetActive(true);
        boxAnimator.SetBool("IsOpen", true);

        choices.SetActive(false);

        showMemory = _pauseGame;
        dialogueAssets = _dialogues;
        autoContinue = _autoContinue;
        //dialogSettings = _settings;
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

        if (dialogueAssetIndex == 0 && dialogueIndex == 4 && showMemory)
        {
            memoryImage.sprite = currentDialogueAsset.memoryImage;
            imageAnimator.SetBool("IsOpen", true);
        }
        Dialogue currentDialogue = currentDialogueAsset.sentences[dialogueIndex];

        //Character currentCharacter = dialogSettings.characters[currentDialogue.characterId];
        //nameText.text = currentCharacter.name;
        //charImage.sprite = currentCharacter.sprite;
        Color tempColor;
        if (currentDialogue.characterId == 0)
        {
            tempColor = roboImage.color;
            tempColor.a = 1f;
            roboImage.color = tempColor;

            tempColor = childImage.color;
            tempColor.a = 0.2f;
            childImage.color = tempColor;

            //dialogueText.alignment = TextAlignmentOptions.Right;
        }
        else
        {
            tempColor = roboImage.color;
            tempColor.a = 0.2f;
            roboImage.color = tempColor;

            tempColor = childImage.color;
            tempColor.a = 1f;
            childImage.color = tempColor;

            //dialogueText.alignment = TextAlignmentOptions.Left;

        }



        StopAllCoroutines();
        StartCoroutine(TypeSentece(currentDialogue.dialogue, dialogueText, autoContinue));

        dialogueIndex++;
    }

    IEnumerator TypeSentece(string sentence, TextMeshProUGUI _text, bool _autoContinue)
    {
        skipLineTriggered = false;
        lineFinished = false;
        _text.text = "";

        foreach (char letter in sentence.ToCharArray())
        {
            if (skipLineTriggered)
            {
                _text.text = sentence;
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
        if (currentDialogueAsset.isChoiceDialogue)
        {
            choices.SetActive(true);

            choiceOneText.text = currentDialogueAsset.choiceOne;
            choiceTwoText.text = currentDialogueAsset.choiceTwo;




        }
        if (currentDialogueAsset.isEndDialogue)
        {

            //test prupose only
            //
            isActive = false;
            imageAnimator.SetBool("IsOpen", false);
            boxAnimator.SetBool("IsOpen", false);
            panelObject.SetActive(false);
            if (currentDialogueAsset.enableShielding && currentDialogueAsset.enableShooting)
            {
                isActive = true;
                gameUIHandler.OpenPopUp(false);
            }
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
        //dialogueBox.SetActive(false);
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

    private string DisplayChoice(int _index)
    {
        string chosenAnswer;
        string[] choice1 = { "Obey order", "Execute protocol", "Carry out program" };
        string[] choice2 = { "Question own protocol", "Search for answer", "Investigate own program" };


        if (_index == 0)
        {
            chosenAnswer = choice1[Random.Range(0, 3)];
        }
        else
        {
            chosenAnswer = choice2[Random.Range(0, 3)];
        }

        return chosenAnswer;
    }

    public void OpenJournal()
    {
        if (!journalIsOpen)
        {
            journalAnimator.SetBool("IsOpen", true);
            panelObject.SetActive(true);
            journalIsOpen = true;
            GameManager.Instance.gamePaused = true;

        }
        else
        {
            journalAnimator.SetBool("IsOpen", false);
            panelObject.SetActive(false);
            journalIsOpen = false;
            GameManager.Instance.gamePaused = false;

        }
    }
}
