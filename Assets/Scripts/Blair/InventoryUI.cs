using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryUI : MonoBehaviour
{
    PlayerInputAction controls;

    public GameObject FadePanel;
    public GameObject SampleButton;
    public GameObject SampleButton2;
    public GameObject InventoryBox;
    public GameObject CharacterBox;
    public GameObject InventorySubBox;
    public GameObject InvBackAnimPanel;
    public GameObject[] MainMenuCogs;
    public GameObject[] MainMenuMemoirs;
    public GameObject[] MemoirsPopUps;
    public GameObject MenuSelector;
    public GameObject SelectInventory;
    public GameObject SelectCharacter;
    public GameObject[] Texts;

    enum State { closed = 0, Opening = 1, Closing = 2, Reserved = 3 }
    private int menuSelected, cogSelected, timer1, timer2;
    private byte ColorTick = 0;
    public string state;
    private float alpha0;
    private Color color0, color1, color2;

    void Awake()
    {
        controls = new PlayerInputAction();
        controls.InventoryControls.LeftBumper.performed += ctx => LBPushed();
        controls.InventoryControls.RightBumper.performed += ctx => RBPushed();
        controls.InventoryControls.SouthButton.performed += ctx => SBPushed();
        controls.InventoryControls.EastButton.performed += ctx => EBPushed();
        controls.InventoryControls.SelectPushed.performed += ctx => SelectPushed();
        controls.InventoryControls.Enable();
    }

    // Start is called before the first frame update
    void Start()
    {
        state = "Closed";
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
            openAnim();
        }
        if (state == "Open")
        {

            FadePanel.GetComponent<Image>().color = new Color32(0, 0, 0, 255);
        }
        if (state == "Closing")
        {
            closeAnim();
        }


    }

    public void Open()
    {
        InvBackAnimPanel.SetActive(true);
        alpha0 = 0; timer1 = 0; ColorTick = 0;
        SampleButton.SetActive(false); SampleButton2.SetActive(false); InventoryBox.SetActive(true); CharacterBox.SetActive(false); InventorySubBox.SetActive(true);
        
        state = "Opening";
        MenuSelector.transform.position = SelectInventory.transform.position;

        if (InvBackAnimPanel != null)
        {
            Animator animator = InvBackAnimPanel.GetComponent<Animator>();
            if (animator != null)
            {
                bool isOpen = animator.GetBool("open");
                animator.SetBool("open", !isOpen);

            }
        }


    }
    void openAnim()
    {
        if (timer1 == 54) { SampleButton2.SetActive(true); alpha0 = 1.0f; }
        alpha0 += 0.015f;
        SelectCharacter.GetComponent<Image>().color = new Color(100 / 255, 100 / 255, 100 / 255, alpha0);
        SelectInventory.GetComponent<Image>().color = new Color(100 / 255, 100 / 255, 100 / 255, alpha0);
        MenuSelector.GetComponent<Image>().color = new Color(1, 1, 1, alpha0);
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

        if (ColorTick < 255) ColorTick += 15;
        timer1++;
        state = timer1 < 55 ? "Opening" : "Open";
        FadePanel.GetComponent<Image>().color = new Color32(0, 0, 0, ColorTick);
    }

    public void Close()
    {
        cogSelected = 0; menuSelected = 0; alpha0 = 1; timer1 = 0; ColorTick = 255;
        SampleButton.SetActive(false); SampleButton2.SetActive(false);
        Animator animator = InvBackAnimPanel.GetComponent<Animator>();
        state = "Closing";

        if (InvBackAnimPanel != null)
        {
            if (animator != null)
            {
                bool isOpen = animator.GetBool("open");
                animator.SetBool("open", !isOpen);
            }

        }

    }
    void closeAnim()
    {
        if (timer1 == 14) { InvBackAnimPanel.SetActive(false); SampleButton.SetActive(true); alpha0 = 0.0f; }
        alpha0 -= 0.115f;
        SelectCharacter.GetComponent<Image>().color = new Color(100 / 255, 100 / 255, 100 / 255, alpha0);
        SelectInventory.GetComponent<Image>().color = new Color(100 / 255, 100 / 255, 100 / 255, alpha0);
        MenuSelector.GetComponent<Image>().color = new Color(1, 1, 1, alpha0);
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
        if (ColorTick < 0) { ColorTick = 0; }
        timer1++;
        FadePanel.GetComponent<Image>().color = new Color32(0, 0, 0, ColorTick);

        state = timer1 < 15 ? "Closing" : "Closed";

    }

    void RBPushed()
    {
        MenuSelector.transform.position = SelectCharacter.transform.position;
        InventorySubBox.SetActive(false); CharacterBox.SetActive(true);
    }
    void LBPushed()
    {
        MenuSelector.transform.position = SelectInventory.transform.position;
        InventorySubBox.SetActive(true); CharacterBox.SetActive(false);

    }

    void EBPushed()
    {

    }

    void SBPushed()
    {

    }

    void SelectPushed()
    {
        if(state == "Closed")
        {
            Open();
        }
        if (state == "Open")
        {
            Close();
        }
    }
    void onEnable()
    {
        controls.InventoryControls.Enable();
    }

    void onDisable()
    {
        controls.InventoryControls.Disable();
    }
}


