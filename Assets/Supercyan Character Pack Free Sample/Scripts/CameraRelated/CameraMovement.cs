/*********************************************************
* Hyukin Kwon
* Save The Date
* 
* CameraMovement
* Last Modified: 27 November 2019
* 
* Inherits from Monobehaviour
*
*
* - Do basic camera movement here
* *******************************************************/

using System.Collections;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    //for zoom over the shoulder variables
    [SerializeField] Transform m_aimingAxis;
    [SerializeField] float m_aimingZoomSpeed;
    [SerializeField] float m_aimingZoomRotSpeed;
    Transform m_saveCameraTrans;
    bool m_isChangingState = false;
    Vector3 m_saveEulerAngle;

    //regular camera movement variables
    [SerializeField] float m_distance;
    [SerializeField] float m_rotSpeed;
    [SerializeField] float m_zoomSpeed; 

    [SerializeField] float m_minHeight;
    [SerializeField] float m_maxHeight;
    [SerializeField] float m_minDistance;
    [SerializeField] float m_maxDistance;

    float m_mouseX;
    float m_mouseY;

    Vector3 m_eulerAngle;

   // bool m_isAiming = false;

    private void Update()
    {

        bool isAiming = GetComponent<CameraAxis>().GetIsAiming();
        if (Input.GetKeyDown(KeyCode.Z) && !m_isChangingState)
        {
            m_isChangingState = true;
            isAiming = !isAiming;

            if (isAiming)
            {
                m_saveEulerAngle = m_eulerAngle;
                m_saveCameraTrans = Camera.main.transform;
            }
            else
            {
                m_eulerAngle = m_saveEulerAngle;
            }
        }

        if(m_isChangingState && isAiming)
        {
            ZoomInForAiming();
        }
        else if(m_isChangingState &&!isAiming)
        {
            ZoomOutToRegular();
        }
        
        if(!m_isChangingState)
        {
            if(!isAiming)
                ZoomInAndOut();
            CameraRotate();
        }
    }

    void ZoomInAndOut() //this is just a regular zoom in and out using mouse scroll wheel.
    {
        m_distance -= Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * m_zoomSpeed;

        Vector3 back = transform.forward * -1f;
        m_distance = Mathf.Clamp(m_distance, m_minDistance, m_maxDistance);

        RaycastHit hit;
        Debug.DrawRay(transform.position, back * m_distance, Color.red);
        if (Physics.Raycast(transform.position, back, out hit, m_distance))
        {
            if (hit.transform.tag != "Player")
                back *= hit.distance - 0.1f;
            else
                back *= m_distance;
        }
        else
            back *= m_distance;

        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, transform.position + back, Time.deltaTime * 13f);
    }

    void CameraRotate()
    {
        m_mouseX = Input.GetAxis("Mouse X");
        m_mouseY = Input.GetAxis("Mouse Y");

        if (Input.GetMouseButton(1)) //make rotation only when right mouse button is being pressed.
        {
            m_eulerAngle.y += m_mouseX * Time.deltaTime * m_rotSpeed;
            m_eulerAngle.x -= m_mouseY * Time.deltaTime * m_rotSpeed;
            m_eulerAngle.x = Mathf.Clamp(m_eulerAngle.x, m_minHeight, m_maxHeight);
        }

        Quaternion rotation = Quaternion.Euler(m_eulerAngle);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, m_rotSpeed * Time.deltaTime);
    }

    void ZoomInForAiming()
    {
        m_eulerAngle.x = m_aimingAxis.rotation.x;
        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, m_aimingAxis.position, Time.deltaTime * m_aimingZoomSpeed);
        transform.rotation = Quaternion.Lerp(transform.rotation, m_aimingAxis.rotation, Time.deltaTime * m_aimingZoomRotSpeed);
        if (Vector3.Distance(Camera.main.transform.position, m_aimingAxis.position) < 0.05f)
            m_isChangingState = false;
    }

    void ZoomOutToRegular()
    {
        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, m_saveCameraTrans.position, Time.deltaTime * m_aimingZoomSpeed);
        transform.rotation = Quaternion.Lerp(transform.rotation, m_saveCameraTrans.rotation, Time.deltaTime * m_aimingZoomRotSpeed);
        if (Vector3.Distance(Camera.main.transform.position, m_saveCameraTrans.position) < 0.05f)
            m_isChangingState = false;
    }
}
