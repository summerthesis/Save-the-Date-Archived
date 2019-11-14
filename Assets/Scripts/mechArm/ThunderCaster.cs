using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderCaster : MonoBehaviour
{
    //Class meant to keep track of targets and fire the particle system accordingly

    private ParticleSystem m_pSystem;
    [SerializeField] private Transform m_targetTransform;

    void Awake() {

        m_pSystem = this.gameObject.GetComponent<ParticleSystem>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (m_targetTransform) this.gameObject.transform.LookAt(m_targetTransform);
    }

    public void SetTarget(Transform target) {

        m_targetTransform = target;
    }

    public void CastThunder() {

        if (!m_pSystem.isPlaying) m_pSystem.Play();
    }

    public void StopCasting() {
        //Stop (if needed)
    }
}
