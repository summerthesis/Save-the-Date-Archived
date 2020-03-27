using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class expendable_CubeMultiplier : MonoBehaviour
{
    [SerializeField] private float m_fCubeSize;
    [SerializeField] private float m_fAmplitude;

    // Start is called before the first frame update
    void Awake() {

        GameObject temp = GameObject.CreatePrimitive(PrimitiveType.Cube);
            temp.transform.localScale = Vector3.one * m_fCubeSize;
            temp.AddComponent<expendable_Follow>();

        for (float i = -55.0f; i <= 55.0f; i += 1.1f*m_fCubeSize) {
            for (float j = -55.0f; j <= 55.0f; j += 1.1f*m_fCubeSize) {
                Vector3 initPos = new Vector3(i, -1.1f*m_fAmplitude, j);
                initPos = this.gameObject.transform.position + initPos;
                GameObject g = GameObject.Instantiate(temp, initPos, this.gameObject.transform.rotation, this.gameObject.transform);
                Destroy(g.GetComponent<BoxCollider>());
            }
        }
        Destroy(temp);
    }
}
