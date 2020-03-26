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
    public GameObject CharacterSubBox;
    public GameObject InventorySubBox;
    public GameObject AudioLogSubBox;
    public GameObject InvBackAnimPanel;
    public GameObject[] MenuTabs;
    public GameObject[] MainMenuCogs;
    public GameObject[] MainMenuMemoirs;
    public GameObject[] MemoirsPopUps;
    public GameObject MenuSelector;
    public GameObject SelectInventory;
    public GameObject SelectCharacter;
    public GameObject[] Texts;
    public GameObject[] NoteTexts;
    public GameObject[] AudioLogs;
    public AudioClip[] AudioLogClips;
    public GameObject NoteSelector;
    public GameObject AudioSelector;
    public AudioSource audioSource;
    public bool isAudioPlaying;

    enum State { closed = 0, Opening = 1, Closing = 2, Reserved = 3 }
    private int timer1; 
    public int tabState, noteState, audioLogState;
    private byte ColorTick = 0;
    public string state;
    private float alpha0;
    private Color color0;

    void Awake()
    {
        controls = new PlayerInputAction();
        controls.InventoryControls.LeftBumper.performed += ctx => LBPushed();
        controls.InventoryControls.RightBumper.performed += ctx => RBPushed();
        controls.InventoryControls.SouthButton.performed += ctx => SBPushed();
        controls.InventoryControls.EastButton.performed += ctx => EBPushed();
        controls.InventoryControls.Up.performed += ctx => UpPushed();
        controls.InventoryControls.Down.performed += ctx => DownPushed();
        controls.InventoryControls.Left.performed += ctx => LeftPushed();
        controls.InventoryControls.Right.performed += ctx => RightPushed();
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
            if(tabState == 1)
            {

            }
            if (tabState == 0)
            { 
            
            }
        }
        if (state == "Closing")
        {
            closeAnim();
        }


    }
    public void OpenToLog(int log)
    {

    }
    public void Open()
    {
        InvBackAnimPanel.SetActive(true);
        alpha0 = 0; timer1 = 0; ColorTick = 0;
        SampleButton.SetActive(false); SampleButton2.SetActive(false); InventoryBox.SetActive(true); CharacterSubBox.SetActive(false); InventorySubBox.SetActive(true);
        AudioSelector.SetActive(false); NoteSelector.SetActive(true);
        for(int i = 0; i < 5; i++) { NoteTexts[i].SetActive(false); NoteTexts[0].SetActive(true); }
        state = "Opening";
        MenuSelector.transform.position = MenuTabs[0].transform.position;
        NoteSelector.transform.position = MainMenuMemoirs[0].transform.position;
        AudioSelector.transform.position = AudioLogs[0].transform.position;

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
        MenuTabs[0].GetComponent<Image>().color = new Color(100 / 255, 100 / 255, 100 / 255, alpha0);
        MenuTabs[1].GetComponent<Image>().color = new Color(100 / 255, 100 / 255, 100 / 255, alpha0);
        MenuTabs[2].GetComponent<Image>().color = new Color(100 / 255, 100 / 255, 100 / 255, alpha0);
        MenuSelector.GetComponent<Image>().color = new Color(1, 1, 1, alpha0);
        AudioSelector.GetComponent<Image>().color = new Color(0, 1, 0, alpha0);
        NoteSelector.GetComponent<Image>().color = new Color(0, 1, 0, alpha0);
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
        alpha0 = 1; timer1 = 0; ColorTick = 255;
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
        MenuTabs[0].GetComponent<Image>().color = new Color(100 / 255, 100 / 255, 100 / 255, alpha0);
        MenuTabs[1].GetComponent<Image>().color = new Color(100 / 255, 100 / 255, 100 / 255, alpha0);
        MenuTabs[2].GetComponent<Image>().color = new Color(100 / 255, 100 / 255, 100 / 255, alpha0);
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
        if (tabState == 2) return;

        if(tabState == 0)
        {
            InventorySubBox.SetActive(false); AudioLogSubBox.SetActive(true);
            AudioSelector.SetActive(true); NoteSelector.SetActive(false);
            MenuSelector.transform.position = MenuTabs[1].transform.position;
            tabState = 1;
            return;
        }
        if(tabState == 1)
        {
            AudioLogSubBox.SetActive(false); CharacterSubBox.SetActive(true);
            AudioSelector.SetActive(false); NoteSelector.SetActive(false);
            MenuSelector.transform.position = MenuTabs[2].transform.position;
            tabState = 2;
            return;
        }
    }
    void LBPushed()
    {
        if (tabState == 0) return;
       
        if(tabState == 1)
        {
            AudioLogSubBox.SetActive(false); InventorySubBox.SetActive(true);
            AudioSelector.SetActive(false); NoteSelector.SetActive(true);
            MenuSelector.transform.position = MenuTabs[0].transform.position;
            tabState = 0;
            return;
        }
        if(tabState == 2)
        {
            CharacterSubBox.SetActive(false); AudioLogSubBox.SetActive(true);
            MenuSelector.transform.position = MenuTabs[1].transform.position;
            AudioSelector.SetActive(true); NoteSelector.SetActive(false);
            tabState = 1;
            return;
        }
    }

    void EBPushed()
    {
        
        if(tabState == 1)
        {
            if (audioSource.isPlaying) { audioSource.Stop(); }
        }
    }

    void SBPushed()
    {
        if (tabState == 1)
        {
            if (audioSource.isPlaying) { audioSource.Stop(); }
            audioSource.clip = AudioLogClips[audioLogState];
            audioSource.Play();
        }
       
    }

    void UpPushed()
    {
        if (tabState == 1)
        {
            if (audioLogState > 4)
            {
                audioLogState -= 5;
                AudioSelector.transform.position = AudioLogs[audioLogState].transform.position;
            }
        }
    }
    void DownPushed()
    {
        if (tabState == 1)
        {
            if(audioLogState < 10)
            {
                audioLogState += 5;
                AudioSelector.transform.position = AudioLogs[audioLogState].transform.position;
            }
        }
    }
    void LeftPushed()
    {
        if (tabState == 0)
        {
            if (noteState > 0)
            {
                noteState--;
                NoteSelector.transform.position = MainMenuMemoirs[noteState].transform.position;
                NoteTexts[noteState + 1].SetActive(false); NoteTexts[noteState].SetActive(true);
            }
        }
        if (tabState == 1)
        {
            if (audioLogState > 0)
            {
                audioLogState--;
                AudioSelector.transform.position = AudioLogs[audioLogState].transform.position;
            }
        }
    }
    void RightPushed()
    {
        if(tabState == 0)
        {
            if(noteState < 4)
            {
                noteState++;
                NoteSelector.transform.position = MainMenuMemoirs[noteState].transform.position;
                NoteTexts[noteState - 1].SetActive(false); NoteTexts[noteState].SetActive(true);
            }
        }
        if(tabState == 1)
        {
            if (audioLogState < 14)
            {
                audioLogState++;
                AudioSelector.transform.position = AudioLogs[audioLogState].transform.position;
            }
        }
    }
    void SelectPushed()
    {
        AudioLogSubBox.SetActive(false); CharacterSubBox.SetActive(false); InventorySubBox.SetActive(true);
        tabState = 0; audioLogState = 0; noteState = 0;
        if(state == "Closed")
        {
            Open();
        }
        if (state == "Open")
        {
            CharacterSubBox.SetActive(false); AudioLogSubBox.SetActive(false);
            AudioSelector.SetActive(false); NoteSelector.SetActive(false);
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


