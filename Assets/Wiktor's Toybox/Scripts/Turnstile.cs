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

    /// <summary>
    /// HErC's observation: I changed the tracking system of the Port:
    /// if it's charged but not active, it activates.
    /// After the activation ends, it deactivates
    /// </summary>
    void Update()
    {
        if (!isPort)
        {
            if (port.GetComponentInChildren<PushButton>().isActive && !isRunning)
            {
                isRunning = true;
                TurnWheel();
            }
        }

        if (isPort)
        {
            if (port.GetComponentInChildren<ElectricalPort>().Active && !isRunning)
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
        {
            port.GetComponentInChildren<PushButton>().isActive = false;
        }
        else
        {
            port.GetComponentInChildren<ElectricalPort>().SetActiveState(port.GetComponentInChildren<ElectricalPort>().isCharged);
        }
    }
}