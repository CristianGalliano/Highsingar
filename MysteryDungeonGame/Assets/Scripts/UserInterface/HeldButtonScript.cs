using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class HeldButtonScript : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool held = false;
    [SerializeField] private Vector2Int direction;

    public void OnPointerDown(PointerEventData eventData)
    {
        held = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        held = false;
    }

    private void Update()
    {
        if (held)
        {
            if (SceneManager.GetActiveScene().name == "PlayerHub")
            {
                PlayerHubMovement.HubPlayer.MovePlayer(direction.x, direction.y);
            }
            else if (SceneManager.GetActiveScene().name == "DungeonScene")
            {
                PlayerMovement.Player.MovePlayer(direction.x, direction.y);
            }
        }
    }
}