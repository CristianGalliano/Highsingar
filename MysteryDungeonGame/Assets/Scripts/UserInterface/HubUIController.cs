using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HubUIController : MonoBehaviour
{
    public static HubUIController HUIC;

    [SerializeField] private GameObject M_ControlsPanel;
    [SerializeField] private Text goldText, rewindText, restartText;

    private void Awake()
    {
        if (HUIC == null)
        {
            HUIC = this;
        }
        else if (HUIC != this)
        {
            Destroy(this);
        }        
    }

    // Start is called before the first frame update
    void Start()
    {
#if  UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_STANDALONE_LINUX
        M_ControlsPanel.gameObject.SetActive(false);
#endif
        updateUserInterfaceValues();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ClearSaveData()
    {
        SaveSystem.ClearSavedData();
        SaveSystem.LoadPlayerData();
        updateUserInterfaceValues();
    }

    public void updateUserInterfaceValues()
    {
        goldText.text = SaveSystemPlayer.tempPlayer.goldAmount.ToString();
        rewindText.text = SaveSystemPlayer.tempPlayer.rewindAmount.ToString();
        restartText.text = SaveSystemPlayer.tempPlayer.restartAmount.ToString();
    }

    public void ResetScroll(Scrollbar scrollBar)
    {
        scrollBar.value = 1;
    }

    public void ResetShopItemValues(GameObject parent)
    {
        foreach (ItemEntry item in GetComponentsInChildren<ItemEntry>())
        {
            item.ResetValues();
        }
    }
}
