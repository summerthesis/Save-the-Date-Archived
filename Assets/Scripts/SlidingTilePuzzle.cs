using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingTilePuzzle : MonoBehaviour
{
    public GameObject horizontalButton;
    public GameObject verticalButton;
    public GameObject[] tiles;
    public GameObject leftPiece;
    public GameObject rightPiece;
    public GameObject middlePiece;

    public int[] currentOrder;
    public int[] desiredOrder;//0 = left 1 = right 2 = middle
    public int[] piecePosition;//3 values  left piece 0-8, right, left. ie. where is [0,1,2,] on the board 
    public bool isAnimating;
    public bool puzzleComplete;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(!isAnimating)
        {
            checkButtons();
        }

    }

    void checkOrder()
    { 
    
    }

    void checkButtons()
    {

    }
    void Animate()
    {

    }

    
}
