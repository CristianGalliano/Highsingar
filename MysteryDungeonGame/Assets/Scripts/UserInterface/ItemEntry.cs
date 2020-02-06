using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemEntry : MonoBehaviour
{
    [Header("Item Values")]
    public string itemName;
    public int itemValue;
    private int minAmount = 1;
    public int maxAmount;
    private int itemAmount;
    public Sprite itemSprite;

    [Header("User Interface Hooks")]
    public Button buyButton;
    public Button addOneButton;
    public Button minusOneButton;
    public Text amountText;
    public Text priceText;
    public Text nameText;
    public Image itemImage;
    public GameObject confirmPanel;
    public Text confirmPanelDetails;
    public Text confirmPanelPrice;

    private void Start()
    {
        itemAmount = 1;
        nameText.text = itemName;
        itemImage.sprite = itemSprite;
        priceText.text = itemValue.ToString();
        CheckPlayerCanAfford();
    }

    public void OnPlusButtonClicked()
    {
        itemAmount += 1;
        if (itemAmount > maxAmount)
        {
            itemAmount = maxAmount;
        }
        UpdateItemValues();
    }

    public void OnMinusButtonClicked()
    {
        itemAmount -= 1;
        if (itemAmount < minAmount)
        {
            itemAmount = minAmount;
        }
        UpdateItemValues();
    }

    public void OnBuyButtonClicked()
    {
        confirmPanel.SetActive(true);
        confirmPanelDetails.text = itemAmount + "X " + itemName;
        confirmPanelPrice.text = (itemAmount * itemValue).ToString();
        confirmPanel.GetComponent<ConfirmPurchasePanel>().ItemToAdd = itemName;
        confirmPanel.GetComponent<ConfirmPurchasePanel>().AmountToAdd = itemAmount;
        confirmPanel.GetComponent<ConfirmPurchasePanel>().Cost = (itemAmount * itemValue);
        confirmPanel.GetComponent<Animator>().SetBool("isOpen", true);
    }

    public void ResetValues()
    {
        itemAmount = 1;
        UpdateItemValues();
    }

    private void UpdateItemValues()
    {
        CheckPlayerCanAfford();
        amountText.text = itemAmount.ToString();
        priceText.text = (itemAmount * itemValue).ToString();
    }

    private void CheckPlayerCanAfford()
    {
        if (SaveSystemPlayer.tempPlayer.goldAmount < (itemAmount * itemValue))
        {
            buyButton.interactable = false;
        }
        else
        {
            buyButton.interactable = true;
        }
    }
}
