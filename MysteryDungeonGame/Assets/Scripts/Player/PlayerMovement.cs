﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Player;

    [HideInInspector]
    public int moveCount = 0;

    [HideInInspector]
    public bool moving = false;

    public bool Rewind;

    [SerializeField]
    private Text MovesText;
    public Button NextLevelButton;
    
    [HideInInspector]
    public Vector3Int previousPosition;
    [HideInInspector]
    public Vector3Int playerStartPosition;
    private Vector3Int nextPosition;
    public Stack<Vector3Int> PreviousPositions = new Stack<Vector3Int>();

    [SerializeField]
    private Animator emoteAnimator;

    [SerializeField]
    private GameObject destroyedTile;
    [SerializeField]
    private GameObject spawnedTile;

    public int Gold, RewindAmount, RestartAmount;

    private void Awake()
    {
        if (Player == null)
        {
            Player = this;
        }
        else if (Player != this)
        {
            Destroy(this);
        }
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_EDITOR_LINUX
        if (!moving)
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
            else if (Input.GetKey(KeyCode.Q) && gameObject.transform.position != playerStartPosition)
            {
                MoveToPrevious();
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

    private void MovePlayer(int X, int Y)
    {
        StartCoroutine(MoveOverSeconds(gameObject, new Vector3Int((int)transform.localPosition.x + X, (int)transform.localPosition.y + Y, (int)transform.localPosition.z), false));
    }

    public void M_MovePlayerVertical(int Y)
    {
        if (!moving)
        {
            StartCoroutine(MoveOverSeconds(gameObject, new Vector3Int((int)transform.localPosition.x, (int)transform.localPosition.y + Y, (int)transform.localPosition.z), false));
        }
    }

    public void M_MovePlayerHorizontal(int X)
    {
        if (!moving)
        {
            StartCoroutine(MoveOverSeconds(gameObject, new Vector3Int((int)transform.localPosition.x + X, (int)transform.localPosition.y, (int)transform.localPosition.z), false));
        }
    }

    public IEnumerator MoveOverSeconds(GameObject objectToMove, Vector3Int end, bool isRewind)
    {
        nextPosition = end;
        if (TestPathGeneration.PathGenerator.tilesInMap.Contains(nextPosition))
        {          
            if (!isRewind)
            {
                TestPathGeneration.PathGenerator.tilemap.SetTile(previousPosition, null);
                Instantiate(destroyedTile, new Vector3(previousPosition.x + 0.5f, previousPosition.y + 0.5f, previousPosition.z), Quaternion.identity);
                TestPathGeneration.PathGenerator.tilesInMap.Remove(previousPosition);

                PreviousPositions.Push(previousPosition);
                moveCount++;
                MovesText.text = "Moves: " + moveCount;
            }

            GetComponentInChildren<Animator>().SetTrigger("Moved");
            moving = true;
            float elapsedTime = 0;
            Vector3 startingPos = objectToMove.transform.position;
            if (startingPos.x < end.x)
            {
                GetComponentInChildren<SpriteRenderer>().flipX = false;
            }
            else if (startingPos.x > end.x)
            {
                GetComponentInChildren<SpriteRenderer>().flipX = true;
            }
            while (elapsedTime < (1f / 2f))
            {
                objectToMove.transform.position = Vector3.Lerp(startingPos, end, (elapsedTime / (1f / 2f)));
                elapsedTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            previousPosition = nextPosition;
            objectToMove.transform.position = end;           
            moving = false;
            if (TestPathGeneration.PathGenerator.tilesInMap.Count == 1)
            {
                Debug.Log("Reached the last tile of the level! This means you've finished the level!");
                NextLevelButton.gameObject.SetActive(true);
                //this should be where the player can exit or go to another level of the same type.
                int j = 5;
                for (int i = 1; i <= j; i++)
                {
                    Instantiate(spawnedTile, new Vector3(end.x + 0.5f, end.y + i + 0.5f, end.z), Quaternion.identity);
                    yield return new WaitForSeconds(0.5f);
                    if (i < j)
                    {
                        TestPathGeneration.PathGenerator.tilemap.SetTile(new Vector3Int(end.x, end.y + i, end.z), TestPathGeneration.PathGenerator.exitTiles);
                    }
                    else
                    {
                        TestPathGeneration.PathGenerator.tilemap.SetTile(new Vector3Int(end.x, end.y + i, end.z), TestPathGeneration.PathGenerator.startTile);
                    }
                }
                for (int i = 1; i <= j; i++)
                {
                    TestPathGeneration.PathGenerator.tilesInMap.Add(new Vector3Int(end.x, end.y + i, end.z));
                }
            }
        }
        else
        {
            yield return null;
        }
    }

    public void MoveToPrevious()
    {
        if (Rewind)
        {
            if (PreviousPositions.Peek() == playerStartPosition)
            {
                TestPathGeneration.PathGenerator.tilemap.SetTile(PreviousPositions.Peek(), TestPathGeneration.PathGenerator.startTile);
            }
            else
            {
                TestPathGeneration.PathGenerator.tilemap.SetTile(PreviousPositions.Peek(), TestPathGeneration.PathGenerator.tiles[Random.Range(0, TestPathGeneration.PathGenerator.tiles.Length)]);
            }
            TestPathGeneration.PathGenerator.tilesInMap.Add(PreviousPositions.Peek());
            StartCoroutine(MoveOverSeconds(gameObject, PreviousPositions.Pop(),true));
        }
    }
}