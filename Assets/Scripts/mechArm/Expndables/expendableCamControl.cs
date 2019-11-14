using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class expendableCamControl : MonoBehaviour
{
    private Camera m_cam;
    [SerializeField] private GameObject m_player;
    [SerializeField] private Vector3 m_offset;

    void Awake() {
        m_cam = this.gameObject.GetComponent<Camera>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.gameObject.transform.position = m_player.transform.position + m_offset;
        this.gameObject.transform.LookAt(m_player.transform);
    }
}
