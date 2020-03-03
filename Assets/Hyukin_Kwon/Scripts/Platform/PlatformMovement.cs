/*********************************************************
* Hyukin Kwon
* Save The Date
* 
* PlatformMovement
* Created: 27 November 2019
* Last Modified: 17 Jan 2020
* 
* Inherits from Monobehaviour
*
*
* - Moving Platform through waypoints
* - Player moves with platform only when colliding
* - Adding start delay time
* *******************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovement : MonoBehaviour
{
    [SerializeField] List<Transform> waypoints = new List<Transform>();
    int m_curWaypoint = 0;
    bool m_dir = true;

    [SerializeField] float m_speed;

    //time that platform will stay in position once it reaches a waypoint and start moving onto next waypoint.
    [SerializeField] float m_stopTime;

    //if false it will stop moving once it reaches to the last waypoint
    [SerializeField] bool m_loop = true;

    [SerializeField] bool m_isMoving = true; //if false, platform won't move
    bool GetIsMoving() { return m_isMoving; }
    void SetIsMoving(bool isMoving) { m_isMoving = isMoving; }

    //Start Delay related
    bool m_isStartMoving = false;
    [SerializeField] float m_starDelayTime;

    //

    private void FixedUpdate()
    {
        MovePlatform();
    }

    private void MovePlatform()
    {
        if (!m_isStartMoving)
        {
            StartCoroutine(StartMovingTimer());
            return;
        }
        if (!m_isMoving) return;

        if (transform.position != waypoints[m_curWaypoint].transform.position)
        {
            transform.position = Vector3.MoveTowards(transform.position,
                waypoints[m_curWaypoint].transform.position, m_speed * Time.fixedDeltaTime);
        }
        if (transform.position == waypoints[m_curWaypoint].transform.position)
        {
            StartCoroutine(StopMovingForCertainTime());
            m_curWaypoint = m_dir ? (m_curWaypoint + 1) : (m_curWaypoint - 1);

            if (m_curWaypoint >= waypoints.Count || m_curWaypoint < 0)
            {
                m_curWaypoint = (m_curWaypoint >= waypoints.Count) ? (waypoints.Count - 1) : 0;
                if (m_loop)
                {
                    m_dir = !m_dir;
                }
                else
                {
                    m_isMoving = false;
                }
            }
        }
    }


    private IEnumerator StartMovingTimer()
    {
        yield return new WaitForSeconds(m_starDelayTime);
        m_isStartMoving = true;
    }

    private IEnumerator StopMovingForCertainTime()
    {
        m_isMoving = false;
        yield return new WaitForSeconds(m_stopTime);
        m_isMoving = true;
    }


    private void OnCollisionStay(Collision collision)
    {
        if (collision.transform.tag == "Player")
            collision.transform.parent = transform;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.tag == "Player")
            collision.transform.parent = null;
    }

}
