using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DateBlockControl : MonoBehaviour
{

    Component halo;
    public GameObject player;
    public float speed = 3;
    private Vector3 screenPoint;
    private Vector3 offset;
    private Component placementCollider;
    private Transform placementTransform;
    public bool isSnap = false; //whether the block is snapped into place
    public bool isHeld = false; //whether the block is currently being grabbed
    private float rotateX;
    private float rotateY;
    private float rotateZ;
    private Vector3 initialRotation;
    Rigidbody rigidbody;

    void Start()
    {
        halo = GetComponent("Halo");
        halo.GetType().GetProperty("enabled").SetValue(halo, false, null);
        rigidbody = GetComponent<Rigidbody>();
        placementTransform = transform.parent;
        placementCollider = placementTransform.GetComponent<BoxCollider>();
        placementTransform.gameObject.GetComponent<Renderer>().enabled = false;
        rotateX = Random.Range(-1f, 1f);
        rotateY = Random.Range(-1f, 1f);
        rotateZ = Random.Range(-1f, 1f);
        initialRotation = placementTransform.forward;
    }

    void Update()
    {
        if (isSnap)
        {
            transform.position = Vector3.MoveTowards(transform.position, placementTransform.position, speed * Time.deltaTime);
        }
        if (!isSnap && !isHeld)
        {
            RotateBlock();
        }
        else
        {
            transform.forward = initialRotation;
        }
    }
    void OnMouseOver()
    {
        if (!isSnap)
        {
            halo.GetType().GetProperty("enabled").SetValue(halo, true, null);
            PlacementControl(true);
        }
    }
    void OnMouseExit()
    {
        halo.GetType().GetProperty("enabled").SetValue(halo, false, null);
        PlacementControl(false);
    }
    void OnMouseDown()
    {
        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
    }
    void OnMouseDrag() //block dragging mechanism. Magic lasso should go here.
    {   //Find the distance between the player and the block. Decide on maximum distance to drag.
        //Scroll with mouse wheel maybe?
        if (!isSnap)
        {
            Vector3 cursorPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
            Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(cursorPoint) + offset;
            transform.position = Vector3.MoveTowards(transform.position, cursorPosition, speed * Time.deltaTime);
            rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
            isHeld = true;
        }
    }
    private void OnMouseUp()
    {
        rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        isHeld = false;
    }
    void OnTriggerEnter(Collider other) //detects if the block is within it's placement
    {
        if (other == placementCollider)
        {
            isSnap = true;
        }
    }

    private void PlacementControl(bool state) //Whether the green placement blocks are visible
    {
        Component placementHalo = placementTransform.gameObject.GetComponent("Halo");
        placementHalo.GetType().GetProperty("enabled").SetValue(placementHalo, state, null);

        placementTransform.gameObject.GetComponent<Renderer>().enabled = state;
    }
    private void RotateBlock()
    {
        transform.Rotate(rotateX, rotateY, rotateZ);
    }
}
