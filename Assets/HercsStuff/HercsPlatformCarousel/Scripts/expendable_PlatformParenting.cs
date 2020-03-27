using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class expendable_PlatformParenting : MonoBehaviour
{
    void OnCollisionEnter(Collision other) {
        if (other.gameObject.tag == "Player") {
            other.transform.parent.parent = this.gameObject.transform;
        }
    }
    void OnCollisionExir(Collision other) {
        if (other.gameObject.tag == "Player") {
            other.transform.parent.parent = null;
        }
    }
}
