using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetOrderInLayer : MonoBehaviour
{
    public bool isMovingObject;
    private Vector3Int previousPosition;
    // Start is called before the first frame update
    void Start()
    {
        if (!isMovingObject)
        {
            ChangeSortOrder();
        }
        else
        {
            previousPosition = Vector3Int.RoundToInt(gameObject.transform.position);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isMovingObject && Vector3Int.RoundToInt(gameObject.transform.position) != previousPosition)
        {
            previousPosition = Vector3Int.RoundToInt(gameObject.transform.position);
            ChangeSortOrder();
        }
    }

    private void ChangeSortOrder()
    {
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = (100 - (int)transform.position.y);
    }
}
