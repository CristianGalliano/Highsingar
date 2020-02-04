using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerHubMovement : MonoBehaviour
{
    public static PlayerHubMovement HubPlayer;

    [HideInInspector] public bool canMove = true;
    [HideInInspector] public bool canInteract = true;

    private Vector3Int previousPosition;
    private Vector3Int playerStartPosition;// = Vector3Int.zero;
    private Vector3Int nextPosition;

    [SerializeField] private Animator playerAnimator;
    [SerializeField] private Animator emoteAnimator;

    public Tilemap HubTilemap;
    private List<Vector3Int> availableTiles;

    private int GoldAmount, RewindAmount, RestartAmount, BombAmount;

    private void Awake()
    {
        if (HubPlayer == null)
        {
            HubPlayer = this;
        }
        else if (HubPlayer != this)
        {
            Destroy(this);
        }
    }

    public void save()
    {
        SaveSystem.SavePlayerData();
        Debug.Log("Playtime: " + SaveSystemPlayer.tempPlayer.timePlayed);
    }

    // Start is called before the first frame update
    void Start()
    {
        GoldAmount = SaveSystemPlayer.tempPlayer.goldAmount;
        RewindAmount = SaveSystemPlayer.tempPlayer.restartAmount;
        RestartAmount = SaveSystemPlayer.tempPlayer.restartAmount;
        BombAmount = SaveSystemPlayer.tempPlayer.bombAmount;

        playerStartPosition = new Vector3Int((int)gameObject.transform.position.x, (int)gameObject.transform.position.y, (int)gameObject.transform.position.z);
        gameObject.transform.position = playerStartPosition;
        availableTiles = new List<Vector3Int>();
        foreach (Vector3Int position in HubTilemap.cellBounds.allPositionsWithin)
        {
            if (HubTilemap.HasTile(position))
            {
                availableTiles.Add(position);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_EDITOR_LINUX
        if (canMove)
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                MovePlayer(0, 1);
            }
            else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                MovePlayer(0, -1);
            }
            else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                MovePlayer(-1, 0);
            }
            else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                MovePlayer(1, 0);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha1) && emoteAnimator.GetCurrentAnimatorStateInfo(0).IsName("Empty State"))
        {
            emoteAnimator.SetTrigger("HeartTrigger");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && emoteAnimator.GetCurrentAnimatorStateInfo(0).IsName("Empty State"))
        {
            emoteAnimator.SetTrigger("AngryTrigger");
        }
#endif
    }

    public void MovePlayer(int X, int Y)
    {
        if (canMove)
        {
            StartCoroutine(MoveOverSeconds(gameObject, new Vector3Int((int)transform.localPosition.x + X, (int)transform.localPosition.y + Y, (int)transform.localPosition.z), false));
        }
    }

    public IEnumerator MoveOverSeconds(GameObject objectToMove, Vector3Int end, bool isRewind)
    {
        nextPosition = end;
        Vector3 startingPos = objectToMove.transform.position;
        if (startingPos.x < end.x)
        {
            GetComponentInChildren<SpriteRenderer>().flipX = false;
        }
        else if (startingPos.x > end.x)
        {
            GetComponentInChildren<SpriteRenderer>().flipX = true;
        }
        if (availableTiles.Contains(nextPosition))
        {
            playerAnimator.SetTrigger("Moved");
            canMove = false;
            float elapsedTime = 0;            
            while (elapsedTime < (1f / 2f))
            {
                objectToMove.transform.position = Vector3.Lerp(startingPos, end, (elapsedTime / (1f / 2f)));
                elapsedTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            previousPosition = nextPosition;
            objectToMove.transform.position = end;
            canMove = true;          
        }
        else
        {
            yield return null;
        }
    }
}
