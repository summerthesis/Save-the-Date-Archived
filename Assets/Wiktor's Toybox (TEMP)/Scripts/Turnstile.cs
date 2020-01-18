using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turnstile : MonoBehaviour
{
    public float RotateSpeed = 0.6f;
    public bool isPowered = false;
    public bool isPort;
    public GameObject port;

    int dir = 1;
    Quaternion fromAngle;
    Quaternion toAngle;
    bool isRunning = false;


    void Update()
    {
        if (!isPort)
        {
            if (port.GetComponent<PushButton>().isActive && !isRunning)
            {
                isRunning = true;
                TurnWheel();
            }
        }

        if (isPort)
        {
            if (port.GetComponent<ElectricalPort>().isActive  && !isRunning)
            {
                isRunning = true;
                TurnWheel();
            }
        }
    }

    public void TurnWheel()
    {
        isPowered = !isPowered;

        if (isPowered)
        {
            dir = -1;
        }
        else
        {
            dir = 1;
        }

        StartCoroutine(Turn(new Vector3(0, dir, 0) * 90, RotateSpeed));
    }

    IEnumerator Turn(Vector3 angle, float time)
    {
        fromAngle = transform.rotation;
        toAngle = Quaternion.Euler(transform.eulerAngles + angle);
        for (float t = 0f; t <= 1; t += Time.deltaTime / time)
        {
            transform.rotation = Quaternion.Slerp(fromAngle, toAngle, t);
            yield return null;
        }
        transform.rotation = toAngle;
        isRunning = false;

        if (!isPort)
            port.GetComponent<PushButton>().isActive = false;
        else
            port.GetComponent<ElectricalPort>().isActive = false;
    }
}