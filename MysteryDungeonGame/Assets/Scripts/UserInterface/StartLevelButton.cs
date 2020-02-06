using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class StartLevelButton : MonoBehaviour
{

    [SerializeField] private int UnlockedGameState;
    [SerializeField] private TextAsset DesiredLevelText;
    [SerializeField] private int LevelGridSize;

    private void Start()
    {
        CheckUnlocked();
    }

    public void OnLevelButtonClicked()
    {
        SaveSystemPlayer.loadedLevel = DesiredLevelText;
        SaveSystemPlayer.gridSize = LevelGridSize;
        SaveSystem.SavePlayerData();
        SceneManager.LoadScene(1);
    }

    private void CheckUnlocked()
    {
        if (SaveSystemPlayer.tempPlayer.gameState >= UnlockedGameState)
        {
            GetComponent<Button>().interactable = true;
            transform.GetChild(1).gameObject.SetActive(false);
        }
        else
        {
            GetComponent<Button>().interactable = false;
            transform.GetChild(1).gameObject.SetActive(true);
        }
    }
}
