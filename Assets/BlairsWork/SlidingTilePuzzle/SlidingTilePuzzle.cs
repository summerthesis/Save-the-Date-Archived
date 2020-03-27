using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This is a mess...just let me get it working 100% will clean up. Sorry!!

public class SlidingTilePuzzle : MonoBehaviour
{
    public GameObject horizontalButton;
    public GameObject verticalButton;
    public GameObject[] tiles;
    public GameObject[] moveablePieces;
    public GameObject tester;

    //change a lot of this to enums later. (still new to those)
    //any of the [] relevant to the moveable pieces are ordered [leftPiece,midPiece,rightPiece].... so bad.. sorry 
    public int[] myTileNumber;// which tile the piece should move to or is currently on
    public int[] myStartTile;
    private int hDirection;//0 left 1 right
    private int vDirection;//0 up   1 down
    private int delayLock;

    public bool[] pieceLocked;//if all true "turn" is over
    public bool[] tileOccupied;
    public bool moveH, moveV;
    public bool puzzleComplete;
    public bool[] isTileDown;

    public Vector3[] desiredPos;

    public float speed;
    float step;
    int animState = 0;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(puzzleComplete)   { WinAnim();  }
        else {RunPuzzle();} 
    }

    void RunPuzzle()
    {
        step = speed * Time.deltaTime;
        if (!moveH && !moveV)
        {

            horizontalButton.GetComponent<PushButton>().isActive = false;
            verticalButton.GetComponent<PushButton>().isActive = false;
            checkButtons();
        }
        if (moveV)
        {
            if (vDirection == 0)
            {
                MoveUp();
            }
            if (vDirection == 1)
            {
                MoveDown();
            }

        }
        if (moveH)
        {
            if (hDirection == 0)
            {
                MoveLeft();
            }
            if (hDirection == 1)
            {
                MoveRight();
            }
        }
    }
    void checkWin()
    {
        if(myTileNumber[0] < myTileNumber[1] 
        && myTileNumber[1] < myTileNumber[2])
        {
            puzzleComplete = true;
        }
    }
    void WinAnim()
    {
        for (int i = 0; i < 3; i++)
        {
            if (Vector3.Distance(moveablePieces[i].transform.position, tiles[i + 3].transform.position) > 0.001f)
            {
                moveablePieces[i].transform.position = Vector3.MoveTowards(moveablePieces[i].transform.position, tiles[i + 3].transform.position, step / 3);
            }
            else
            {
            
            }
        }
    }
    void checkButtons()
    {
        if (verticalButton.GetComponent<PushButton>().isPressed)
        {
            if (vDirection == 1) { vDirection = 0; } else { vDirection = 1; }
            moveV = true; moveH = false;
        }
        if (horizontalButton.GetComponent<PushButton>().isPressed)
        {
            if (hDirection == 1) { hDirection = 0; } else { hDirection = 1; }
            moveH = true; moveV = false;
        }
    }

   void MoveRight()
    {
        delayLock++;
        
        for(int i = 0; i < 3; i++)
        {
           if(pieceLocked[0] && pieceLocked[1] && pieceLocked[2] && delayLock > 100)
            {
                delayLock = 0;
                horizontalButton.GetComponent<PushButton>().isPressed = false;
                horizontalButton.GetComponent<PushButton>().isActive = false;
                moveH = false;
            }
           
            if(myTileNumber[i] < 9)
            {
                if (!tileOccupied[myTileNumber[i] + 1] && myTileNumber[i] < 8)
                {
                    if (Vector3.Distance(moveablePieces[i].transform.position, tiles[myTileNumber[i] + 1].transform.position) > 0.001f)
                    {
                        moveablePieces[i].transform.position = Vector3.MoveTowards(moveablePieces[i].transform.position, tiles[myTileNumber[i] + 1].transform.position, step);
                    }
                    else
                    {
                        pieceLocked[i] = true;
                        tileOccupied[myTileNumber[i]] = false;
                        tileOccupied[myTileNumber[i] + 1] = true;
                        myTileNumber[i]++;
                        

                    }

                }
                else
                {
                    if (delayLock > 98)
                    {
                      //  Debug.Log("TilesFinished");
                      if(myTileNumber[0] < 9 && myTileNumber[1] < 9 && myTileNumber[2] < 9)
                        {
                            checkWin();
                           
                        }
                    }
                }

            }
            
        }
   }
    void MoveLeft()
    {
        delayLock++;

        for (int i = 0; i < 3; i++)
        {
            if (pieceLocked[0] && pieceLocked[1] && pieceLocked[2] && delayLock > 100)
            {
                delayLock = 0;
                horizontalButton.GetComponent<PushButton>().isPressed = false;
                horizontalButton.GetComponent<PushButton>().isActive = false;
                moveH = false;
            }

            if (myTileNumber[i] < 9)
            {
                if (myTileNumber[i] > 0 && !tileOccupied[myTileNumber[i] - 1] && myTileNumber[i] > 0)
                {
                    if (Vector3.Distance(moveablePieces[i].transform.position, tiles[myTileNumber[i] - 1].transform.position) > 0.001f)
                    {
                        moveablePieces[i].transform.position = Vector3.MoveTowards(moveablePieces[i].transform.position, tiles[myTileNumber[i] - 1].transform.position, step);
                    }
                    else
                    {
                        pieceLocked[i] = true;
                        tileOccupied[myTileNumber[i]] = false;
                        tileOccupied[myTileNumber[i] - 1] = true;
                        myTileNumber[i]--;
                    }

                }
                else
                {
                    if (delayLock > 98)
                    {
                        //Debug.Log("TilesFinished");
                        if (myTileNumber[0] < 9 && myTileNumber[1] < 9 && myTileNumber[2] < 9)
                        {
                            checkWin();
                            
                        }
                    }
                }
            }

            
        }
    }

   void MoveUp()
    {
        for(int i = 0; i < 3; i++)
        {
            if(myTileNumber[i] == 9)
            {
                if (Vector3.Distance(moveablePieces[i].transform.position, tiles[2].transform.position) > 0.001f)
                {
                    moveablePieces[i].transform.position = Vector3.MoveTowards(moveablePieces[i].transform.position, tiles[2].transform.position, step);
                }
                else
                {
                    pieceLocked[i] = true;
                    tileOccupied[9] = false;
                    tileOccupied[2] = true;
                    myTileNumber[i] = 2;
                    delayLock = 0;
                    verticalButton.GetComponent<PushButton>().isPressed = false;
                    verticalButton.GetComponent<PushButton>().isActive = false;
                    moveV = false;
                }
            }

            if (myTileNumber[i] == 10)
            {
                if (Vector3.Distance(moveablePieces[i].transform.position, tiles[6].transform.position) > 0.001f)
                {
                    moveablePieces[i].transform.position = Vector3.MoveTowards(moveablePieces[i].transform.position, tiles[6].transform.position, step);
                }
                else
                {
                    pieceLocked[i] = true;
                    tileOccupied[10] = false;
                    tileOccupied[6] = true;
                    myTileNumber[i] = 6;
                    delayLock = 0;
                    verticalButton.GetComponent<PushButton>().isPressed = false;
                    verticalButton.GetComponent<PushButton>().isActive = false;
                    moveV = false;
                }
            }
        }
    }
    void MoveDown()
    {
        for (int i = 0; i < 3; i++)
        {
            if (myTileNumber[i] == 2)
            {
                if (Vector3.Distance(moveablePieces[i].transform.position, tiles[9].transform.position) > 0.001f)
                {
                    moveablePieces[i].transform.position = Vector3.MoveTowards(moveablePieces[i].transform.position, tiles[9].transform.position, step);
                }
                else
                {
                    pieceLocked[i] = true;
                    tileOccupied[2] = false;
                    tileOccupied[9] = true;
                    myTileNumber[i] = 9;
                    delayLock = 0;
                    verticalButton.GetComponent<PushButton>().isPressed = false;
                    verticalButton.GetComponent<PushButton>().isActive = false;
                    moveV = false;
                }
            }

            if (myTileNumber[i] == 6)
            {
                if (Vector3.Distance(moveablePieces[i].transform.position, tiles[10].transform.position) > 0.001f)
                {
                    moveablePieces[i].transform.position = Vector3.MoveTowards(moveablePieces[i].transform.position, tiles[10].transform.position, step);
                }
                else
                {
                    pieceLocked[i] = true;
                    tileOccupied[6] = false;
                    tileOccupied[10] = true;
                    myTileNumber[i] = 10;
                    delayLock = 0;
                    verticalButton.GetComponent<PushButton>().isPressed = false;
                    verticalButton.GetComponent<PushButton>().isActive = false;
                    moveV = false;
                }
            }

        }

    }
  
}
