﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    public string name;
    [TextArea(3, 10)]
    public string[] sentences;

    [TextArea(3, 10)]
    public string ShopKeeperSentence;
    [TextArea(3, 10)]
    public string QuestGiverSentence;

    [Header("NPC Type")]
    public bool IsShopKeeper;
    public bool IsQuestGiver;
}