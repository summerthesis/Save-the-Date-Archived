using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public GameObject FadePanel;
    public GameObject SampleButton;
    public GameObject SampleButton2;
    public GameObject InventoryBox;
    public GameObject InvBackAnimPanel;
    public GameObject[] MainMenuCogs;
    public GameObject[] MainMenuMemoirs;
    public GameObject[] MemoirsPopUps;
    public GameObject SelectInventory;
    public GameObject SelectCharacter;
    public GameObject[] Texts;
    enum State { closed = 0, Opening = 1, Closing = 2, Reserved = 3}

    private byte ColorTick = 0;
    
    public string state;

    private int timer1, timer2;
    private float alpha0;
    private Color color0, color1, color2;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (state == "Closed")
        {
            InventoryBox.SetActive(false);
            FadePanel.GetComponent<Image>().color = new Color32(0, 0, 0, 0);
        }
        if (state == "Opening")
        {
         
            if (timer1 == 54) { SampleButton2.SetActive(true); alpha0 = 1.0f; }
            alpha0+=0.015f;
            SelectCharacter.GetComponent<Image>().color = new Color(100/255, 100/255, 100/255, alpha0);
            SelectInventory.GetComponent<Image>().color = new Color(100/255, 100/255, 100/255, alpha0);
            for (int i = 0; i < 15; i++)//todo get total number of all elements 
            {
               if(i < MainMenuCogs.Length)
               {
                MainMenuCogs[i].GetComponent<Image>().color = new Color(1, 1, 1, alpha0);
               }
               if (i < MainMenuMemoirs.Length)
               {
                MainMenuMemoirs[i].GetComponent<Image>().color = new Color(1, 1, 1, alpha0);
               }
               if (i < Texts.Length)
               {
                Texts[i].GetComponent<Text>().color = new Color(1, 1, 1, alpha0);
               }
            }
        
            if(ColorTick < 255) ColorTick+=15;
            timer1++;
            state = timer1 < 55 ? "Opening" : "Open";
            FadePanel.GetComponent<Image>().color = new Color32(0, 0, 0, ColorTick);
        }
        if (state == "Open")
        {
          FadePanel.GetComponent<Image>().color = new Color32(0, 0, 0, 255);
        }
        if (state == "Closing")
        {
            if (timer1 == 14) { SampleButton.SetActive(true); alpha0 = 0.0f; }
            alpha0 -= 0.115f;
            SelectCharacter.GetComponent<Image>().color = new Color(100 / 255, 100 / 255, 100 / 255, alpha0);
            SelectInventory.GetComponent<Image>().color = new Color(100 / 255, 100 / 255, 100 / 255, alpha0);
            for (int i = 0; i < 15; i++)//todo get total number of all elements 
            {
                if (i < MainMenuCogs.Length)
                {
                    MainMenuCogs[i].GetComponent<Image>().color = new Color(1, 1, 1, alpha0);
                }
                if (i < MainMenuMemoirs.Length)
                {
                    MainMenuMemoirs[i].GetComponent<Image>().color = new Color(1, 1, 1, alpha0);
                }
                if (i < Texts.Length)
                {
                    Texts[i].GetComponent<Text>().color = new Color(1, 1, 1, alpha0);
                }
            }
            ColorTick -= 15;
            if (ColorTick < 0) { ColorTick = 0;  }
            timer1++;
            FadePanel.GetComponent<Image>().color = new Color32(0, 0, 0, ColorTick);
          
            state = timer1 < 15 ? "Closing" : "Closed";
        }


    }

    public void Open()
    {
        InventoryBox.SetActive(true);
        alpha0 = 0; timer1 = 0; 
        ColorTick = 0;  state = "Opening"; SampleButton.SetActive(false); SampleButton2.SetActive(false); 
        if(InvBackAnimPanel != null)
        {
        Animator animator = InvBackAnimPanel.GetComponent<Animator>();
          if(animator != null)
            {
                bool isOpen = animator.GetBool("open");
                animator.SetBool("open", !isOpen);
               
            }
        }
        

    }
    public void Close() 
    {
        alpha0 = 1; timer1 = 0;
        ColorTick = 255;  state = "Closing"; SampleButton.SetActive(false); SampleButton2.SetActive(false); 
        Animator animator = InvBackAnimPanel.GetComponent<Animator>();
        if(InvBackAnimPanel != null)
        {
            if (animator != null)
            {
            bool isOpen = animator.GetBool("open");
            animator.SetBool("open", !isOpen);
            }

        }
        
    }
}


