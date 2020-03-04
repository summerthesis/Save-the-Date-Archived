using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class expendable_CubeMultiplier : MonoBehaviour
{
    [SerializeField] private GameObject m_cubeModel; 

    // Start is called before the first frame update
    void Awake()
    {
        for (float i = -55.0f; i <= 55.0f; i += 1.1f*m_cubeModel.transform.localScale.x) {
            for (float j = -55.0f; j <= 55.0f; j += 1.1f*m_cubeModel.transform.localScale.z) {
                Vector3 temp = new Vector3(i, -0.55f*m_cubeModel.transform.localScale.y, j);
                temp = this.gameObject.transform.position + temp;
                Instantiate(m_cubeModel, temp, this.gameObject.transform.rotation, this.gameObject.transform);
            }
        }
    }
}
