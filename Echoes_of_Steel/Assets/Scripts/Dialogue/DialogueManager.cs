using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private GameObject choices;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI choiceOneText;
    [SerializeField] private TextMeshProUGUI choiceTwoText;
    [SerializeField] private Image charImage;
    [SerializeField] private Animator animator;
    [SerializeField] private float dialogueSpeed;

    private DialogueAsset[] dialogueAssets;
    private DialogueAsset currentDialogueAsset;
    private bool skipLineTriggered;
    private bool lineFinished;
    public static bool isActive;
    private int dialogueIndex;
    private int dialogueAssetIndex;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && isActive)
        {
            if(!lineFinished)
            {

            SkipLine();
            }
            else if(lineFinished)
            {
                DisplayNextSentence();
            }
        }
    }

    public void StartDialogue(DialogueAsset[] _dialogues)
    {
        isActive = true;
        //dialogueBox.SetActive(true);
        animator.SetBool("IsOpen", true);

        choices.SetActive(false);

        dialogueAssets = _dialogues;
        currentDialogueAsset = dialogueAssets[dialogueAssetIndex];

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if(dialogueIndex > currentDialogueAsset.sentences.Length - 1)
        {
            EndDialogue();
            return;
        }

        Dialogue currentDialogue = currentDialogueAsset.sentences[dialogueIndex];

        Character currentCharacter = currentDialogueAsset.characters[currentDialogue.characterId];
        nameText.text = currentCharacter.name;
        charImage.sprite = currentCharacter.sprite;

        StopAllCoroutines();
        StartCoroutine(TypeSentece(currentDialogue.dialogue));

        dialogueIndex++;
    }

    IEnumerator TypeSentece(string sentence)
    {
        skipLineTriggered = false;
        lineFinished = false;
        dialogueText.text = "";

        foreach (char letter in sentence.ToCharArray())
        {
            if(skipLineTriggered)
            {
                dialogueText.text = sentence;
                break;
            }
            dialogueText.text += letter;
            
            yield return new WaitForSeconds(1 / dialogueSpeed);
            
        }
        lineFinished = true;

    }

    private void SkipLine()
    {
        skipLineTriggered = true;
    }

    private void EndDialogue()
    {
        if(currentDialogueAsset.isChoiceDialogue)
        {
            choices.SetActive(true);

            choiceOneText.text = DisplayChoice(0);
            choiceTwoText.text = DisplayChoice(1);
            
                
                
            
        }
        if (currentDialogueAsset.isEndDialogue)
        {

        //test prupose only
        dialogueAssetIndex = 0;
        currentDialogueAsset = dialogueAssets[dialogueAssetIndex];
        //
        isActive = false;
        animator.SetBool("IsOpen", false);
        }
        dialogueIndex = 0;
        lineFinished = false;
        //dialogueBox.SetActive(false);
    }

    public void ChoiceOne()
    {
        if(dialogueAssetIndex == 1)
        {
            dialogueAssetIndex += 2;
        }
        else
        {

            dialogueAssetIndex++;
        }

        GameManager.Instance.corruptionMeter += 20;

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

        GameManager.Instance.corruptionMeter -= 20;

        currentDialogueAsset = dialogueAssets[dialogueAssetIndex];
        choices.SetActive(false);
        DisplayNextSentence();
    }

    private string DisplayChoice(int _index)
    {
        string chosenAnswer;
        string[] choice1 = {"Obey order", "Execute protocol", "Carry out program"};
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
}
