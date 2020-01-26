using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [Header("User Interface Hooks")]
    public Text nameText;
    public Text dialogueText;
    public GameObject dialoguePanel;
    public GameObject shopKeeperPanel;

    private Animator dialogueBoxAnimator;
    private Animator shopKeeperPanelAnimator;
    private Queue<string> sentences;
    private Dialogue currentDialogue;

    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
        dialogueBoxAnimator = dialoguePanel.GetComponent<Animator>();
        shopKeeperPanelAnimator = shopKeeperPanel.GetComponent<Animator>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        currentDialogue = dialogue;
        dialoguePanel.SetActive(true);
        dialogueBoxAnimator.SetBool("IsOpen", true);
        nameText.text = dialogue.name;
        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.0125f);
        }
    }

    void EndDialogue()
    {           
        StartCoroutine(CloseDialogueUI());
        Debug.Log("Finished Conversation.");
    }

    IEnumerator CloseDialogueUI()
    {
        dialogueBoxAnimator.SetBool("IsOpen", false);
        yield return new WaitForSeconds(0.5f);
        dialoguePanel.SetActive(false);
        if (currentDialogue.IsShopKeeper)
        {
            shopKeeperPanel.SetActive(true);
            shopKeeperPanelAnimator.SetBool("IsOpen", true); 
        }
    }
}
