using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TestPathGeneration : MonoBehaviour
{
    private int numberExamples = 0;
    private int GridSize = 16;
    private int MinMoves = 128;
    private int MaxMoves = 256;
    private bool hit = false;
    private List<Node> ListOfNodes = new List<Node>();
    private List<int> nodesVisited = new List<int> { };
    private bool minNodesReached = false;

    private int offset;

    [SerializeField]
    private Tile tile;
    [SerializeField]
    private Tilemap tilemap;

    void Start()
    {
        offset = GridSize + 2;
        //for (int i = 0; i < 10; i++)
        //{
        //GridSize = Random.Range(2, 33);
        while (!minNodesReached)
        {
            createList();
        }
        //}
    }

    //generate gridsize squared nodes.
    //for each node the directions/connected nodes are node number +1, node number -1, node number +x, node number -x.
    //until minimum moves are reached it cant go out of range/hit a wall.
    //remove nodes that have already been used from other nodes.

    void Update()
    {
        
    }

    private void createList()
    {
        ListOfNodes.Clear();
        for (int i = 0; i < sqr(GridSize); i++)
        {

            Node Temp = new Node(i+1, GridSize);
            calculateBorders(Temp);
            ListOfNodes.Add(Temp);
        }
        generatePath();
        hit = false;
    }

    private void generatePath()
    {
        nodesVisited.Clear();
        string path = "";
        Node Temp = ListOfNodes[Random.Range(0, sqr(GridSize))];
        int i = 0;
        while (Temp.down != 1000 && Temp.up != 1000 && Temp.left != 1000 && Temp.right != 1000)
        {
            Temp = ListOfNodes[Random.Range(0, sqr(GridSize))];
        }
        nodesVisited.Add(Temp.value);
        //path += Temp.value + ", ";
        
        while (!hit)
        {
            if (i >= MaxMoves)
            {
                hit = true;
            }
            else
            {
                int next = 0;
                if (!noDirection(Temp))
                {

                    next = calculateDirection(Temp);
                    while (next == 1000)
                    {
                        next = calculateDirection(Temp);
                    }
                }
                else
                {
                    hit = true;
                }
                if (!hit)
                {
                    Node Prev = Temp;
                    removeUsedNode(Prev);                       

                    Temp = ListOfNodes[next - 1];
                    //path += Temp.value + ", ";
                    nodesVisited.Add(Temp.value);
                }
            }
            i++;
        }
        if (nodesVisited.Count > MinMoves)
        {
            foreach (int value in nodesVisited)
            {
                path += value + ", ";
            }
            Debug.Log(path);
            numberExamples++;
            if (numberExamples == 10)
            {
                minNodesReached = true;
            }
            generateTilemap();
        }      
    }

    public void generateTilemap()
    {
        foreach (int value in nodesVisited)
        {
            int x, y;
            y = ((value - 1) / GridSize);
            x = (value -1)-(y * GridSize);
            Debug.Log("Coordinate for Node " + value + ":" + x + ", " + y);
            tilemap.SetTile(new Vector3Int(x + (offset * numberExamples), y, 0), tile);
            if (nodesVisited.IndexOf(value) == 0)
            {
                tile.color = Color.cyan;
            }
        }
        tile.color = Color.white;
    }

    #region MathsFunctions
    private int sqr(int value)
    {
        return value * value;
    }

    private int calculateDirection(Node Temp)
    {
        int rand = Random.Range(0,4);
        switch (rand)
        {
            case 0:
                return Temp.left;
            case 1:
                return Temp.right;
            case 2:
                return Temp.up;
            case 3:
                return Temp.down;
        }
        return 0;

    }

    private bool noDirection(Node Temp)
    {
        if (Temp.up == 1000 && Temp.down == 1000 && Temp.left == 1000 && Temp.right == 1000)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void removeUsedNode(Node Prev)
    {
        foreach (Node node in ListOfNodes)
        {
            if (node.up == Prev.value)
            {
                node.up = 1000;
            }
            if (node.down == Prev.value)
            {
                node.down = 1000;
            }
            if (node.left == Prev.value)
            {
                node.left = 1000;
            }
            if (node.right == Prev.value)
            {
                node.right = 1000;
            }         
        }
        Prev.value = 1000;
    }

    private void calculateBorders(Node Temp)
    {
        if (Temp.value == 1)
        {
            Temp.right = 1000;
            Temp.down = 1000;
        }
        if (Temp.value == sqr(GridSize))
        {
            Temp.left = 1000;
            Temp.up = 1000;
        }
        if (Temp.value == GridSize)
        {
            Temp.left = 1000;
            Temp.down = 1000;
        }
        if (Temp.value == (GridSize * (GridSize - 1)) + 1)
        {
            Temp.right = 1000;
            Temp.up = 1000;
        }
        if (Temp.value < GridSize)
        {
            Temp.down = 1000;
        }
        if (Temp.value % (GridSize) == 0)
        {
            Temp.left = 1000;
        }
        if (Temp.value > GridSize * (GridSize - 1))
        {
            Temp.up = 1000;
        }
        if ((Temp.value - 1) % GridSize == 0)
        {
            Temp.right = 1000;
        }
    }
    #endregion
}
