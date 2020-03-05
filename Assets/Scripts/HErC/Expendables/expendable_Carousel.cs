using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlatformRotation { NoRotation = 0,
                               TowardsCenter = 1,
                               AwayFromCenter = 1 << 1}

public class expendable_Carousel : MonoBehaviour
{
    [SerializeField] private PrimitiveType m_pTypeToCreate;
    [SerializeField] private float m_fRotationSpeed;
    [SerializeField] private float m_fCarouselRadius;
    [SerializeField] private float m_fPlatformSize;
    [SerializeField] private PlatformRotation m_PlatformRotation;    
    [SerializeField] private GameObject m_PlatformPrefab;
    [SerializeField] private GameObject[] m_Platforms;

    float angle;
    private float angleDiv;

    #region EditorVariables
    public int NumberOfPlatforms { get { return m_Platforms.Length; } }
    public float CarouselRadius { get { return m_fCarouselRadius; } }
    public PlatformRotation platformRotation { get { return m_PlatformRotation; } }

    #endregion
        
    void Awake() {

        angle = m_Platforms.Length > 0 ? 2 * Mathf.PI / m_Platforms.Length : 0;
        for  (int i = 0; i < m_Platforms.Length; ++i) {
            angleDiv = i * angle;
            if (m_PlatformPrefab) {
                m_Platforms[i] = GameObject.Instantiate(m_PlatformPrefab, //template
                                                        CalculateInitialPosition(angleDiv), //position
                                                        Quaternion.identity, this.gameObject.transform); //rotation/parent
            }
            else {
                m_Platforms[i] = GameObject.CreatePrimitive(m_pTypeToCreate);
                m_Platforms[i].transform.parent = transform;
                m_Platforms[i].transform.position = CalculateInitialPosition(angleDiv);

                switch (m_pTypeToCreate) {
                    case PrimitiveType.Sphere:
                    case PrimitiveType.Capsule:
                    case PrimitiveType.Plane:
                    case PrimitiveType.Quad:
                        m_Platforms[i].transform.localScale = Vector3.one * m_fPlatformSize;
                        break;
                    case PrimitiveType.Cylinder:
                        Destroy(m_Platforms[i].GetComponent<Collider>());
                        m_Platforms[i].transform.localScale = new Vector3(m_fPlatformSize, 0.5f, m_fPlatformSize);
                        m_Platforms[i].AddComponent<BoxCollider>();
                        break;
                    case PrimitiveType.Cube:
                        m_Platforms[i].transform.localScale = new Vector3(m_fPlatformSize, 1, m_fPlatformSize);
                        break;
                    default:
                        break;
                }
            }
            m_Platforms[i].AddComponent<expendable_PlatformParenting>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(transform.position, transform.up, m_fRotationSpeed*Time.deltaTime);

        if (m_pTypeToCreate != PrimitiveType.Quad) {
            for (int i = 0; i < m_Platforms.Length; ++i) {
                angleDiv = angle * i;
                if (m_PlatformRotation == PlatformRotation.NoRotation) {
                    m_Platforms[i].transform.rotation = Quaternion.identity;
                }
                else { 
                    Vector3 endResult = this.gameObject.transform.position;
                    endResult.y = m_Platforms[i].transform.position.y;
                    m_Platforms[i].transform.LookAt(endResult);

                    if (m_PlatformRotation == PlatformRotation.AwayFromCenter) {
                        m_Platforms[i].transform.Rotate(Vector3.up * 180);
                    }
                }
            }
        } else {
            foreach (GameObject g in m_Platforms) {
                Vector3 endResult = g.transform.position - this.gameObject.transform.position;
                g.transform.LookAt(endResult*-1);
                g.transform.Rotate(this.gameObject.transform.up, 180, Space.Self);
            }
        }
    }

    /// <summary>
    /// Calculates the platform's initial position based on the parent's initial position and the 
    /// angle returned by the number of subdivisions
    /// </summary>
    /// <param name="angle"></param>
    /// <returns></returns>
    public Vector3 CalculateInitialPosition(float angle) {
        return (this.gameObject.transform.position + 
            transform.TransformDirection( new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle))) * m_fCarouselRadius);
    }
}
