using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

public class GearPlacementUI : MonoBehaviour
{
    GraphicRaycaster m_Raycaster;
    PointerEventData m_PointerEventData;
    EventSystem m_EventSystem;
    public GameObject m_Camera;
    public GameObject m_Canvas;
    public Canvas canvas;
    public GameObject PanelParent;
    public GameObject BackingPanel;
    public GameObject Button;
    public GameObject UI_GearPanel;
    public GameObject[] InventoryItems;
    public Vector3[] startPosItems;
    public GameObject[] InventorySlots;
    public GameObject[] GearReceivers;
    public GameObject Highlighter;
    
    private GameObject itemHoveringThis;
    public bool[] isCollected;
    public bool[] isPlaced;
    int numPlaced;
    int animCount;
    public int Highlighted = 0;
    GameObject oRayHit;

    float dragSpeed;
    float hAxis, vAxis;
    float posX, posY, pixelX, pixelY;
    public enum States {Start = 0, Open = 1, MovingItem = 2, Returning = 3, Close = 4, Closed = 5};
    public States State;
    public bool lockControl;
    int lockCount;
    public Vector3 returnScaleHighlighter;
    public Vector3 returnScaleItem;
    public Vector3 cursorPos;
    public Collider m_Collider;
    public Collider m_OtherCollider;
    public string RaycastReturn;
    public Material placedMat;
    public bool PuzzleCompleted;
    // Start is called before the first frame update
    void Start()
    {   
     
        dragSpeed = 150;
        State = States.Closed;
        m_Raycaster = m_Canvas.GetComponent<GraphicRaycaster>();
        m_EventSystem = GetComponent<EventSystem>(); 
        
        
    }
    
    // Update is called once per frame
    void Update()
    {
        if(State == States.Closed)
        {
            if (Button.GetComponent<PushButton>().isPressed)
            {
                for (int i = 0; i < 8; i++)
                {
                    if (isCollected[i] == false)
                    {
                        InventoryItems[i].GetComponent<Image>().enabled = false;
                    }
                } 
                State = States.Start;
            }
               

        }
        
        if(State == States.Start)
        {
            animCount++;
            this.transform.Rotate(-1.25f, 0f, 0f, Space.Self);
            UI_GearPanel.transform.Translate(new Vector3(-10.245f, 0, 0), Space.Self);
            if (animCount > 25)
            {
                animCount = 0;
                State = States.Open;
                for(int i = 0; i < 8; i++)
                {
                    startPosItems[i] = InventoryItems[i].transform.position;
                }
            }

        }

        if(State == States.Open)
        {
            numPlaced = 0;
            for(int i = 0; i < 8; i++)
            {
                if (isPlaced[i]) { numPlaced++; }
            }
            if(numPlaced == 8) { State = States.Close; Highlighter.GetComponent<Image>().enabled = false; }

            if (Highlighter.transform.position != InventoryItems[Highlighted].transform.position)
            {
                Highlighter.transform.localPosition = Vector3.MoveTowards(Highlighter.transform.localPosition, InventoryItems[Highlighted].transform.localPosition, dragSpeed);
            }
            if (!lockControl)
            {
                if(Input.GetButtonUp("Submit"))
                {
                    if(isCollected[Highlighted])
                    {
                        State = States.MovingItem;
                        Highlighter.transform.localScale = new Vector3(2.35f, 0.65f, 1);
                        InventoryItems[Highlighted].transform.localScale = new Vector3(2.35f, 0.65f, 1);
                        
                        m_Collider = InventoryItems[Highlighted].GetComponent<Collider>();
                        m_OtherCollider = GearReceivers[Highlighted].GetComponent<Collider>();
                        return;
                    }
                }
               
                if (Input.GetAxis("Horizontal") == 1 && Highlighted % 2 == 0)
                {
                    Highlighted++;
                    lockControl = true;
                                       
                }
                if (Input.GetAxis("Horizontal") == -1 && Highlighted % 2 != 0)
                {
                    Highlighted--;
                    lockControl = true;
                }
                if (Input.GetAxis("Vertical") == 1 && Highlighted > 1)
                {
                    Highlighted -= 2;
                    lockControl = true;
                }
                if (Input.GetAxis("Vertical") == -1 && Highlighted < 6)
                {
                    Highlighted += 2;
                    lockControl = true;
                } 
            }
            else 
            {
                lockCount++; if(lockCount > 15) { lockCount = 0; lockControl = false; }
            }

        }

        if (State == States.MovingItem)
        {
            Highlighter.transform.Translate(new Vector3(Input.GetAxis("Horizontal") * 5.06f, Input.GetAxis("Vertical") * 5.06f, 0), Space.World);
            InventoryItems[Highlighted].transform.Translate(new Vector3(Input.GetAxis("Horizontal") * 5.06f, Input.GetAxis("Vertical") * 5.06f, 0), Space.World);
            if (Input.GetButtonUp("Cancel"))
            {
                State = States.Returning;
                return;
            }
            CheckRay();

        }
        
        if(State == States.Returning)
        {
            if (Highlighter.transform.localPosition != InventorySlots[Highlighted].transform.localPosition)
            {
                InventoryItems[Highlighted].transform.localPosition = Vector3.MoveTowards(Highlighter.transform.localPosition, startPosItems[Highlighted], dragSpeed * 3);
                Highlighter.transform.localPosition = Vector3.MoveTowards(Highlighter.transform.localPosition, InventorySlots[Highlighted].transform.localPosition, dragSpeed * 3);
            }
            else
            {
                
                InventoryItems[Highlighted].transform.rotation = Quaternion.identity;
                InventoryItems[Highlighted].transform.position = startPosItems[Highlighted];
                Highlighter.transform.localScale = returnScaleHighlighter;
                InventoryItems[Highlighted].transform.localScale = returnScaleItem;
                State = States.Open;
             }
        }

        if(State == States.Close)
        {
            animCount++;
            this.transform.Rotate(+1.25f, 0f, 0f, Space.Self);
            UI_GearPanel.transform.Translate(new Vector3(+10.245f, 0, 0), Space.Self);
            if (animCount > 25)
            {
                animCount = 0;
                State = States.Closed;
                for (int i = 0; i < 8; i++)
                {
                    startPosItems[i] = InventoryItems[i].transform.position;
                }
            }

        }
        if (State == States.Closed)
        {

        }
    }

    void CheckRay()
    {   
        cursorPos = m_Camera.GetComponent<Camera>().ScreenToWorldPoint(Highlighter.transform.position);
        cursorPos = m_Camera.GetComponent<Camera>().WorldToScreenPoint(cursorPos);
        Ray uiRay = Camera.main.ScreenPointToRay(cursorPos);
        
        RaycastHit uiHit;
        if(Physics.Raycast(uiRay,out uiHit))
        {
            if(uiHit.collider != null)
            {
                RaycastReturn = uiHit.collider.gameObject.name;
                itemHoveringThis = GameObject.Find(RaycastReturn);
            
                if(itemHoveringThis != null) if(itemHoveringThis == GearReceivers[Highlighted])
                {
                        InventoryItems[Highlighted].transform.Rotate(new Vector3(0, 0, 1), 3);
                       // Highlighter.transform.Rotate(new Vector3(0,0,1), 3);
                        if (Input.GetButtonUp("Submit"))
                        {
                            State = States.Open;
                            isCollected[Highlighted] = false;
                            isPlaced[Highlighted] = true;
                            InventoryItems[Highlighted].GetComponent<Image>().enabled = false;
                            InventoryItems[Highlighted].transform.localPosition = InventorySlots[Highlighted].transform.localPosition;
                            GearReceivers[Highlighted].GetComponent<MeshRenderer>().material = placedMat;
                            Highlighter.transform.localScale = returnScaleHighlighter;
                        }
                }
            }
        }
        Debug.DrawRay(uiRay.origin, uiRay.direction * 10, Color.blue);
    }
}
