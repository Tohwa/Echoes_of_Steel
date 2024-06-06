using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private Image charImage;
    [SerializeField] private Animator animator;
    [SerializeField] private float dialogueSpeed;

    private DialogueAsset dialogue;
    private bool skipLineTriggered;
    private bool lineFinished;
    private static bool isActive;
    private int dialogueIndex;

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

    public void StartDialogue(DialogueAsset _dialogue)
    {
        isActive = true;
        //dialogueBox.SetActive(true);
        animator.SetBool("IsOpen", true);

        dialogue = _dialogue;

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if(dialogueIndex > dialogue.sentences.Length - 1)
        {
            EndDialogue();
            return;
        }

        Dialogue currentDialogue = dialogue.sentences[dialogueIndex];

        Character currentCharacter = dialogue.characters[currentDialogue.characterId];
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
        isActive = false;
        dialogueIndex = 0;
        animator.SetBool("IsOpen", false);
        //dialogueBox.SetActive(false);
    }
}
