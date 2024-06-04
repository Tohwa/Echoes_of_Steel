using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private Animator animator;
    [SerializeField] private float dialogueSpeed;

    private Queue<string> sentences;
    private DialogueAsset dialogue;
    private bool skipLineTriggered;
    private bool lineFinished;

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

    private void Start()
    {
        sentences = new Queue<string>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
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
        animator.SetBool("IsOpen", true);

        dialogue = _dialogue;

        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if(sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();

        if (sentence.StartsWith('m'))
        {
            sentence = sentence.Substring(1);
            nameText.text = dialogue.mainCharName;
        }
        else if (sentence.StartsWith('s'))
        {
            sentence = sentence.Substring(1);
            nameText.text = dialogue.sideCharName;
        }

        //dialogueText.text = sentence;
        StopAllCoroutines();
        StartCoroutine(TypeSentece(sentence));

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
        animator.SetBool("IsOpen", false);
    }
}
