using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementBlockControl : MonoBehaviour
{
    private Vector3 position;
    private Quaternion rotation;

    // Start is called before the first frame update
    void Start()
    {
        position = transform.position;
        rotation = transform.rotation;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.SetPositionAndRotation(position, rotation);
    }
}
