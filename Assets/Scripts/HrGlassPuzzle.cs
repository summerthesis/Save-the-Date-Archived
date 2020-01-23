using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// USE KEYS 1,2,3,4,5 to highlight buttons[0,1,2,3,4] push E to activate
// Activate using code[] integers to solve puzzle.
//code = keyboard  5, 2, 1, 3 
//controls are locked during animation.
public class HrGlassPuzzle : MonoBehaviour
{

    public GameObject[] buttons;
    public GameObject[] hrGlasses;
    private bool[] isFlipped = { false, false, false, true, false };
    public int[] code = { 4, 1, 0, 2 }; //button puzzle order
    public int state = 0;//correct button push => ++;
    public int Focus = -1; // which button are we focused on -1 = none
    private int animCount = 0; //control animation duration


    private bool correct = false; // was the correct button pushed?
    public bool animating = false; // are we currently animating? take away control
    private bool PuzzleComplete = false; //When all is done, PuzzleComplete = true

    public Material myMaterial;
    public Material myMaterialTesting; // just for testing
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (PuzzleComplete)
        {
            return;
        }

        if (!animating)
        {          
            CheckButtons();
        }

        if (animating)
        {
            AnimateGlasses();
        }

    }

    void AnimateGlasses()
    {
        if (correct)
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
            hrGlasses[Focus].transform.Rotate(Vector3.back);
        }
        //Reduce frames left, if were done, turn off animation, reset
        //focused button - if last state complete puzzle.
        animCount--;
        if (animCount <= 0)
        {
            for(int i = 0; i < 5; i++)
            {
            buttons[i].GetComponent<PushButton>().isPressed = false;
            buttons[i].GetComponent<PushButton>().isActive = false;
            }
            
            animating = false;
            Focus = -1;
            if (state == 4) { PuzzleComplete = true; }
        }

    }
    void CheckButtons()
    {
        for(int i = 0; i < 5; i++)
        {
           if(buttons[i].GetComponent<PushButton>().isPressed == true)
           {
                Focus = i;

                if (Focus != -1)
                {
                    if (Focus == code[state])
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
            }
        }
    }
}

