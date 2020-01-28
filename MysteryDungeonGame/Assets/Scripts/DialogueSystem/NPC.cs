using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [Header("NPC Dialogue")]
    public Dialogue dialogue;

    [SerializeField] private Animator interactionAnimator;
    [SerializeField] private GameObject interactionPromptObject;
    private bool playerInRange = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && playerInRange && PlayerHubMovement.HubPlayer.canInteract && PlayerHubMovement.HubPlayer.canMove)
        {
            TriggerDialogue();
            PlayerHubMovement.HubPlayer.canInteract = false;
        }

        if (playerInRange && PlayerHubMovement.HubPlayer.canInteract)
        {
            interactionPromptObject.SetActive(true);
            interactionAnimator.SetBool("InRange", true);
        }
        else if (playerInRange && !PlayerHubMovement.HubPlayer.canInteract)
        {
            StartCoroutine(closeInteractionPrompt());
        }
    }

    public void TriggerDialogue()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("someone entered the trigger");
        if (collision.gameObject.CompareTag("Player"))
        {
            interactionPromptObject.SetActive(true);
            interactionAnimator.SetBool("InRange", true);
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("someone left the trigger");
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(closeInteractionPrompt());
            playerInRange = false;
            PlayerHubMovement.HubPlayer.canInteract = true;
        }
    }

    public void TriggerShopEndDialogue()
    {
        FindObjectOfType<DialogueManager>().ShopClosedDialogue();
    }

    IEnumerator closeInteractionPrompt()
    {
        interactionAnimator.SetBool("InRange", false);
        yield return new WaitForSeconds(0.5f);
        interactionPromptObject.SetActive(false);
    }
    private void OnMouseDown()
    {
#if UNITY_ANDROID || UNITY_IOS
        if (playerInRange && PlayerHubMovement.HubPlayer.canInteract && PlayerHubMovement.HubPlayer.canMove)
        {
            TriggerDialogue();
            PlayerHubMovement.HubPlayer.canInteract = false;
        }
#endif
    }
}
