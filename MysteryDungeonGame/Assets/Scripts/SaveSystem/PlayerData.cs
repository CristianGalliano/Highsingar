[System.Serializable]
public class PlayerData
{
    public int goldAmount, rewindAmount, restartAmount, bombAmount, gameState, num8x8Completed, num16x16Completed, num20x20Completed;
    public float timePlayed;

    public PlayerData()
    {
        goldAmount = 100;
        rewindAmount = 20;
        restartAmount = 5;
        bombAmount = 0;
        gameState = 0;
        num8x8Completed = 0;
        num16x16Completed = 0;
        num20x20Completed = 0;
        timePlayed = 0.0f;
    }
}
