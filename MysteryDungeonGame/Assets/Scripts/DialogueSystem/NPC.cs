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
#if UNITY_ANDROID || UNITY_IOS
            StopAllCoroutines();
            StartCoroutine(openMobileInteractionprompt());
#else
            interactionPromptObject.SetActive(true);
            interactionAnimator.SetBool("InRange", true);
#endif
        }
        else if (playerInRange && !PlayerHubMovement.HubPlayer.canInteract)
        {
#if UNITY_ANDROID || UNITY_IOS
            StopAllCoroutines();
            StartCoroutine(closeMobileInteractionprompt());
#else
            StartCoroutine(closeInteractionPrompt());
#endif
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
#if UNITY_ANDROID || UNITY_IOS
            StopAllCoroutines();
            StartCoroutine(openMobileInteractionprompt());
#else
            interactionPromptObject.SetActive(true);
            interactionAnimator.SetBool("InRange", true);           
#endif
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("someone left the trigger");
        if (collision.gameObject.CompareTag("Player"))
        {
#if UNITY_ANDROID || UNITY_IOS
            StopAllCoroutines();
            StartCoroutine(closeMobileInteractionprompt());
#else
            StartCoroutine(closeInteractionPrompt());
#endif
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

    IEnumerator openMobileInteractionprompt()
    {
        float elapsedTime = 0;
        float DesiredValue = 1.35f;

        Material mat =  gameObject.GetComponentInChildren<SpriteRenderer>().material;
        float value = mat.GetFloat("_OutlineThickness");
        while (elapsedTime < (1f / 6f))
        {
            mat.SetFloat("_OutlineThickness", Mathf.Lerp(value, DesiredValue, (elapsedTime / (1f / 10f))));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        mat.SetFloat("_OutlineThickness", DesiredValue);
    }

    IEnumerator closeMobileInteractionprompt()
    {    
        float elapsedTime = 0;
        float DesiredValue = 0;

        Material mat = gameObject.GetComponentInChildren<SpriteRenderer>().material;
        float value = mat.GetFloat("_OutlineThickness");
        while (elapsedTime < (1f / 6f))
        {
            mat.SetFloat("_OutlineThickness", Mathf.Lerp(value, DesiredValue, (elapsedTime / (1f / 10f))));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        mat.SetFloat("_OutlineThickness", DesiredValue);
    }
}
