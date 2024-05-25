using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private Animator animator;

    private Queue<string> sentences;
    private Dialogue dialogue;

    private void Start()
    {
        sentences = new Queue<string>();
    }

    public void StartDialogue(Dialogue _dialogue)
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
        dialogueText.text = "";

        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }

    private void EndDialogue()
    {
        animator.SetBool("IsOpen", false);
    }
}
