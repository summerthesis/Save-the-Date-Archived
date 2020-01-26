using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearPlacementUI : MonoBehaviour
{
    public GameObject PanelParent;
    public GameObject LeftPanel;
    public GameObject RightPanel;
    public GameObject BackingPanel;
    public GameObject Button;

    public GameObject[] InventoryItems;
    public GameObject[] InventoryHolders;
    public GameObject[] PlacementReceivers;

    public int numPlaced;
    public int animCount;
    public enum States {Start = 0, Open = 1, MovingItem = 2,Close = 3, Closed = 4};
    public States State;
    // Start is called before the first frame update
    void Start()
    {
        State = States.Closed;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(State == States.Closed)
        {
            if (Button.GetComponent<PushButton>().isPressed)
                State = States.Start;
        }
        
        if(State == States.Start)
        {
            animCount++;
            LeftPanel.transform.Rotate(-1f, 0f, 0f, Space.Self);
            RightPanel.transform.Rotate(-1f, 0f, 0f, Space.Self);
            BackingPanel.transform.Rotate(-1f, 0f, 0f, Space.Self);

            if (animCount > 25)
            {
                animCount = 0;
                State = States.Open;
            }
        }

        if(State == States.Open)
        {

        }
    }
}
