using UnityEngine;

public class GameController : MonoBehaviour
{
    private void Awake()
    {
        SaveSystem.LoadPlayerData();
        Destroy(gameObject);
    }
}
