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
    
    enum State { closed = 0, Opening = 1, Closing = 2, Reserved = 3}

    private byte ColorTick = 0;
    
    public string state;
    
    
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
            ColorTick+=15;
            state = ColorTick < 255 ? "Opening" : "Open";
            FadePanel.GetComponent<Image>().color = new Color32(0, 0, 0, ColorTick);
        }
        if (state == "Open")
        {
          FadePanel.GetComponent<Image>().color = new Color32(0, 0, 0, 255);
        }
        if (state == "Closing")
        {
            
            if (ColorTick < 0) { ColorTick = 0;  }
            FadePanel.GetComponent<Image>().color = new Color32(0, 0, 0, ColorTick);
            ColorTick -= 15;
            state = ColorTick > 0 ? "Closing" : "Closed";
        }


    }

    public void Open()
    {
        InventoryBox.SetActive(true);
        ColorTick = 0;  state = "Opening"; SampleButton.SetActive(false); SampleButton2.SetActive(true); 
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
        ColorTick = 255;  state = "Closing"; SampleButton.SetActive(true); SampleButton2.SetActive(false); 
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


