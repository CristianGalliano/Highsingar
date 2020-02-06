using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmPurchasePanel : MonoBehaviour
{
    [HideInInspector] public string ItemToAdd;
    [HideInInspector] public int AmountToAdd, Cost;

    public void OnConfirmButtonClicked()
    {
        ApplyPurchasedItem();
        HubUIController.HUIC.updateUserInterfaceValues();
        SaveSystem.SavePlayerData();
        StartCoroutine(CloseConfirmPanel());       
    }

    public void OnCancelButtonClicked()
    {
        StartCoroutine(CloseConfirmPanel());
    }

    IEnumerator CloseConfirmPanel()
    {
        GetComponent<Animator>().SetBool("isOpen", false);
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
    }

    private void ApplyPurchasedItem()
    {
        switch (ItemToAdd)
        {
            case "REWIND TOKEN":
                SaveSystemPlayer.tempPlayer.rewindAmount += AmountToAdd;
                break;
            case "RESTART TOKEN":
                SaveSystemPlayer.tempPlayer.restartAmount += AmountToAdd;
                break;
            case "DYNAMITE":
                SaveSystemPlayer.tempPlayer.dynamiteAmount += AmountToAdd;
                break;
            case "":
                Debug.LogWarning("Not passed an item name through!");
                break;
                //Add more cases here when implementing more items!
        }
        SaveSystemPlayer.tempPlayer.goldAmount -= Cost;
    }
}
