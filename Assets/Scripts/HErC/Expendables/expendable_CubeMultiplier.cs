using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class expendable_CubeMultiplier : MonoBehaviour
{
    [SerializeField] private GameObject m_cubeModel;
    [SerializeField] private GameObject m_player;
    [SerializeField] private float m_height;
    private GameObject m_lightHolder;

    // Start is called before the first frame update
    void Awake()
    {
        m_lightHolder = this.gameObject.transform.GetChild(0).gameObject;
        for (float i = -55.0f; i <= 55.0f; i += 1.1f*m_cubeModel.transform.localScale.x) {
            for (float j = -55.0f; j <= 55.0f; j += 1.1f*m_cubeModel.transform.localScale.z) {
                Vector3 temp = new Vector3(i, -0.55f*m_cubeModel.transform.localScale.y, j);
                temp = this.gameObject.transform.position + temp;
                Instantiate(m_cubeModel, temp, this.gameObject.transform.rotation, this.gameObject.transform);
            }
        }
    }

    void Update() {
        Vector3 playerPos = new Vector3(m_player.transform.position.x, m_player.transform.position.y + m_height, m_player.transform.position.z);
        m_lightHolder.transform.position = playerPos;
    }
}
