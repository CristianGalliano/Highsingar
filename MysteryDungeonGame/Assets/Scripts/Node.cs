using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public int value;   
    public int up, down, left, right;

    public Node(int Value, int GridSize)
    {
        value = Value;
        up = Value + GridSize;
        down = Value - GridSize;
        left = Value+1;
        right = Value-1;
    }
}
