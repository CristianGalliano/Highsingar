using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int goldAmount, rewindAmount, restartAmount;

    public PlayerData(PlayerMovement player)
    {
        goldAmount = player.Gold;
        rewindAmount = player.RewindAmount;
        restartAmount = player.RestartAmount;
    }
}
