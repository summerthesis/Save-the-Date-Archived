using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class expendable_Follow : MonoBehaviour
{
    Transform m_playerTransform;
    float m_fMaxAngle = 90;
    float m_fAmplitude = 300;
    // Start is called before the first frame update
    void Start()
    {
        m_playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        float xOffset, yOffset, zOffset;
        xOffset = Mathf.Clamp(Mathf.Abs(m_playerTransform.position.x - this.gameObject.transform.position.x), 0, m_fMaxAngle);
        zOffset = Mathf.Clamp(Mathf.Abs(m_playerTransform.position.z - this.gameObject.transform.position.z), 0, m_fMaxAngle); ;
        yOffset = m_fAmplitude * Mathf.Cos(Mathf.Deg2Rad * zOffset) * Mathf.Cos(Mathf.Deg2Rad * xOffset);
        yOffset = Mathf.Clamp(yOffset, 0, m_fAmplitude * 0.99f);
        this.gameObject.transform.localPosition = new Vector3(this.gameObject.transform.localPosition.x, yOffset - m_fAmplitude, this.gameObject.transform.localPosition.z);
    }
}
