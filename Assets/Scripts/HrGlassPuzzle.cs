using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// USE KEYS 1,2,3,4,5 to highlight buttons[0,1,2,3,4] push E to activate
// Activate using code[] integers to solve puzzle.
//code = keyboard  5, 2, 1, 3 
//controls are locked during animation.
public class HrGlassPuzzle : MonoBehaviour
{
    public GameObject[] buttons;//buttons from 0-5 
    public GameObject[] hrGlasses;//corresponding hour glasses
    
    public int[] code = { 4,1,0,2 }; //button puzzle order
    public int state = 0;//correct button push => ++;
    public int Focus = -1; // which button are we focused on -1 = none
    private int animCount = 0; //control animation duration

    private bool correct = false; // was the correct button pushed?
    private bool animating = false; // are we currently animating? take away control
    private bool PuzzleComplete = false; //When all is done, PuzzleComplete = true
    
    private KeyCode keyPressed; //for testing will remove later

    // Start is called before the first frame update
    void Start()
    {
               
    }

    // Update is called once per frame
    void Update()
    {
        if(PuzzleComplete) 
        {
            return;//animate, or exit, whatevs.
        }
        
        if(!animating)
        {           //TODO: get which button to focus by other means
            GetInput(); //for testing
        }

        if(animating)
        {
            AnimateGlasses();
        }

    }

    void AnimateGlasses()
    {
        if(correct)
        {
            switch (state)
            {
                case 1:
                    hrGlasses[4].transform.Rotate(Vector3.forward);
                    hrGlasses[1].transform.Rotate(Vector3.back);
                    break;
                case 2:
                    hrGlasses[1].transform.Rotate(Vector3.forward);
                    hrGlasses[0].transform.Rotate(Vector3.back);
                    break;
                case 3:
                    hrGlasses[0].transform.Rotate(Vector3.forward);
                    hrGlasses[2].transform.Rotate(Vector3.back);
                    break;
                case 4:
                    hrGlasses[2].transform.Rotate(Vector3.forward);
                   
                    break;
            }
        }
        else
        {
            hrGlasses[Focus].transform.Rotate(Vector3.back * 2);
        }
        //Reduce frames left, if were done, turn off animation, reset
        //focused button - if last state complete puzzle.
        animCount--;
        if (animCount <= 0)
        {
            animating = false;
            Focus = -1;
            if (state == 4) { PuzzleComplete = true; }
        }

    }
    void GetInput()//For testing will remove later: just stores which key has been pressed.
    {
        if (Input.anyKey)
        {
         
            foreach (KeyCode vKey in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKey(vKey))
                {
                    keyPressed = vKey;
                }
            }
        }
        CheckInput();//until I can set which button by locking on.
    }

    void CheckInput()//Also for testing. can remove later.
    {                

        switch (keyPressed)
        {
            case KeyCode.Alpha1: //Select Button 0,1,2,3,4 buttons[0,1,2,3,4]
                Focus = 0;
                break;
            case KeyCode.Alpha2:
                Focus = 1;
                break;
            case KeyCode.Alpha3:
                Focus = 2;
                break;
            case KeyCode.Alpha4:
                Focus = 3;
                break;
            case KeyCode.Alpha5:
                Focus = 4;
                break;
            case KeyCode.E://Activate Focussed Button
              if(Focus != -1)
                {
                if(Focus == code[state])
                    {
                        correct = true;
                        state++;
                        animating = true;
                        animCount = 180;
                    }
                else
                    {
                        correct = false;
                        animating = true;
                        animCount = 180;                           
                    }
                }
                break;
        }

    }
}
