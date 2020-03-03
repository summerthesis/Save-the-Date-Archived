using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityWaper : MonoBehaviour
{
    [SerializeField] private float m_gGravityOverride;
    private void Awake()
    {
        Physics.gravity = Vector3.up * -m_gGravityOverride;
    }
}
