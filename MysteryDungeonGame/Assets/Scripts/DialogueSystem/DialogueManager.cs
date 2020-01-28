using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [Header("User Interface Hooks")]
    [SerializeField] private Text nameText;
    [SerializeField] private Text dialogueText;
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private GameObject shopKeeperPanel;
    [SerializeField] private GameObject M_ControlsPanel;

    private Animator dialogueBoxAnimator;
    private Animator shopKeeperPanelAnimator;
    private Animator m_ControlsPanelAnimator;
    private Queue<string> sentences;
    private Dialogue currentDialogue;

    private bool closingShop = false;

    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
        dialogueBoxAnimator = dialoguePanel.GetComponent<Animator>();
        shopKeeperPanelAnimator = shopKeeperPanel.GetComponent<Animator>();
        m_ControlsPanelAnimator = M_ControlsPanel.GetComponent<Animator>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        PlayerHubMovement.HubPlayer.canMove = false;
        StartCoroutine(StartDialogueUI(dialogue));
    }

    IEnumerator StartDialogueUI(Dialogue dialogue)
    {
        m_ControlsPanelAnimator.SetBool("IsOpen", false);
        yield return new WaitForSeconds(0.5f);
        closingShop = false;
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

    public void EndDialogue()
    {           
        StartCoroutine(CloseDialogueUI());
        Debug.Log("Finished Conversation.");
    }

    public void QuickEndDialogue()
    {
        StartCoroutine(QuickCloseDialogueUI());
        Debug.Log("Finished Conversation early.");
    }

    IEnumerator CloseDialogueUI()
    {
        dialogueBoxAnimator.SetBool("IsOpen", false);
        yield return new WaitForSeconds(1 / 3f);
        dialoguePanel.SetActive(false);
        if (currentDialogue.IsShopKeeper && !closingShop)
        {
            shopKeeperPanel.SetActive(true);
            shopKeeperPanelAnimator.SetBool("IsOpen", true);
        }
        else
        {
            m_ControlsPanelAnimator.SetBool("IsOpen", true);
            yield return new WaitForSeconds(0.5f);
            PlayerHubMovement.HubPlayer.canMove = true;
        }
    }

    IEnumerator QuickCloseDialogueUI()
    {
        dialogueBoxAnimator.SetBool("IsOpen", false);
        yield return new WaitForSeconds(1 / 3f);
        dialoguePanel.SetActive(false);
        m_ControlsPanelAnimator.SetBool("IsOpen", true);
        yield return new WaitForSeconds(0.5f);
        PlayerHubMovement.HubPlayer.canMove = true;
    }

    public void ShopClosedDialogue()
    {
        StartCoroutine(CloseShopWindow());
    }

    IEnumerator CloseShopWindow()
    {
        closingShop = true;
        shopKeeperPanelAnimator.SetBool("IsOpen", false);
        yield return new WaitForSeconds(0.5f);
        shopKeeperPanel.SetActive(false);
        dialoguePanel.SetActive(true);
        dialogueBoxAnimator.SetBool("IsOpen", true);
        nameText.text = currentDialogue.name;
        sentences.Clear();
        sentences.Enqueue(currentDialogue.ShopKeeperSentence);

        DisplayNextSentence();
    }
}
