using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class expendable_Canvas : MonoBehaviour
{
    private Canvas m_canvas;
    private Text m_text;
    private ChargeTracker m_mech;

    void Awake() {

        m_canvas = this.gameObject.GetComponent<Canvas>();
        m_text = this.gameObject.GetComponentInChildren<Text>();
    }

    // Start is called before the first frame update
    void Start()
    {
        m_mech = GameObject.FindObjectOfType<ChargeTracker>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_mech) m_mech = GameObject.FindObjectOfType<ChargeTracker>();

        m_text.text = m_mech.Charges + "/" + m_mech.MaxCharges;
    }
}
