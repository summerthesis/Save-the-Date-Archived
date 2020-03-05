using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class expendable_DeathPlane : MonoBehaviour
{
    [SerializeField] Vector3 m_vRespawnPoint;

    void OnCollisionEnter(Collision other) {
        if (other.gameObject.tag == "Player") {
            other.gameObject.transform.position = m_vRespawnPoint;
            other.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }
}
