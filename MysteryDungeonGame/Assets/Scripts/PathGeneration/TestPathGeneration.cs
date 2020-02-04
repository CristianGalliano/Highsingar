using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TestPathGeneration : MonoBehaviour
{
    public static TestPathGeneration PathGenerator;

    private int numberExamples = 0;

    [System.NonSerialized]
    public int GridSize;
    private int MinMoves;
    private int MaxMoves;
    private bool hit = false; 
    private List<int> nodesVisited = new List<int> { };
    private bool minNodesReached = false;

    private int offset;

    public Tile [] tiles;
    public Tile startTile;
    public Tile exitTiles;
    public Tilemap tilemap;
    [SerializeField]
    private TextAsset Paths;
    [SerializeField]
    private GameObject playerController;

    public List<Vector3Int> tilesInMap;

    private void Awake()
    {
        if (PathGenerator == null)
        {
            PathGenerator = this;
        }
        else if (PathGenerator != this)
        {
            Destroy(this);
        }
        Paths = SaveSystemPlayer.loadedLevel;
        GridSize = SaveSystemPlayer.gridSize;
    }
    void Start()
    {
        MaxMoves = sqr(GridSize);
        MinMoves = sqr(GridSize) / 2;
        offset = -(GridSize / 2);
        loadLevel();
    }

    public void loadLevel()
    {
        PlayerMovement.Player.NextLevelButton.gameObject.SetActive(false);
        PlayerMovement.Player.moveCount = 0;
        tilesInMap.Clear();
        nodesVisited.Clear();
        tilemap.ClearAllTiles();
        splitInputFile();
        PlayerMovement.Player.PreviousPositions.Clear();
        PlayerMovement.Player.previousPosition = PlayerMovement.Player.playerStartPosition;
    }

    public void generateTilemap()
    {
        int count = 0;
        foreach (int value in nodesVisited)
        {
            int x, y;
            y = ((value - 1) / GridSize);
            x = (value -1)-(y * GridSize);
            if (nodesVisited.IndexOf(value) == 0)
            {
                tilemap.SetTile(new Vector3Int(x + offset, y + offset, 0), startTile);
                

                PlayerMovement.Player.transform.position = new Vector3(x + (offset), y + (offset), playerController.transform.position.z);
                PlayerMovement.Player.previousPosition = new Vector3Int((int)PlayerMovement.Player.transform.position.x, (int)PlayerMovement.Player.transform.position.y, (int)PlayerMovement.Player.transform.position.z);
                PlayerMovement.Player.playerStartPosition = PlayerMovement.Player.previousPosition;
            }
            else
            {
                tilemap.SetTile(new Vector3Int(x + offset, y + offset , 0), tiles[Random.Range(0,tiles.Length)]);
            }
            tilesInMap.Add(new Vector3Int(x + offset, y + offset, 0));
            count++;
        }       
    }

    #region Readin
    public void splitInputFile()
    {
        string[] allPaths;
        allPaths = (Paths.text.Split('\n'));
        int random = Random.Range(0, allPaths.Length-1);
        string [] path = allPaths[random].Split(',');
        for (int i = 0; i < path.Length-1; i++)
        {
            nodesVisited.Add(int.Parse(path[i]));
        }
        generateTilemap();
    }
    #endregion

    #region MathsFunctions
    private int sqr(int value)
    {
        return value * value;
    }
    #endregion
}
