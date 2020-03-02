using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    private int m_iPlayerLives;
    public int Lives { get { return m_iPlayerLives; } }

    [SerializeField] private int m_iMaxGears;
    private int m_iPlayerGears;
    public int PlayerGears { get { return m_iPlayerGears; } }

    void Awake() {
        m_iPlayerLives = 3;
        m_iPlayerGears = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddGear() {
        m_iPlayerGears++;
        if (m_iPlayerGears == m_iMaxGears) {
            m_iPlayerGears = 0;
            m_iPlayerLives++;
        }
    }
}
