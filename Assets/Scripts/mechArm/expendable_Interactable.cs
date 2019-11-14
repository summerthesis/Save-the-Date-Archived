using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class expendable_Interactable : MonoBehaviour
{
    private Material m_mat;

    [SerializeField] private Color m_emissionColour;
    [SerializeField] private bool m_isCharged;
    public bool Charged { get { return m_isCharged; } }

    void Awake() {
        m_mat = this.gameObject.GetComponent<MeshRenderer>().material;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //update appearance based on charge
        //m_mat.color = m_isCharged ? Color.green : Color.gray;
        //_EmissionColor is the glow. May be best used in combination with halo
        m_mat.SetColor("_EmissionColor", m_isCharged ? m_emissionColour : Color.black);
    }

    public void Charge() {
        m_isCharged = true;
    }

    public void ReturnCharge() {
        m_isCharged = false;
    }
}
