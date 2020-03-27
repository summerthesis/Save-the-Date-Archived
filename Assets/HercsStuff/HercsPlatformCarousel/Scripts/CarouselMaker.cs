using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarouselMaker : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float carouselRadius;
    [SerializeField] private Vector3 rotationAxis;
    [SerializeField] private float platformRadius;
    [SerializeField] private GameObject[] platforms;
    private void Awake()
    {
        for (int i = 0; i < platforms.Length; ++i) {
            platforms[i] = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            platforms[i].transform.parent = this.gameObject.transform;
            platforms[i].transform.localScale = new Vector3(platformRadius, 0.5f, platformRadius);
            platforms[i].transform.position = new Vector3(carouselRadius * Mathf.Sin(i*Mathf.PI * 2 / platforms.Length), 0, carouselRadius * Mathf.Cos(i* Mathf.PI * 2 / platforms.Length)) + this.gameObject.transform.position;
            Destroy(platforms[i].GetComponent<Collider>());
            platforms[i].AddComponent<BoxCollider>();
            platforms[i].GetComponent<BoxCollider>().size = new Vector3(1, 2, 1);
            platforms[i].AddComponent<PlatformParenting>();
        }
    }
    // Update is called once per frame
    void Update()
    {
        this.gameObject.transform.Rotate(rotationAxis * rotationSpeed, Space.Self);
        foreach (GameObject g in platforms) {
            g.transform.Rotate(rotationAxis * -rotationSpeed, Space.Self);
            Debug.DrawLine(this.gameObject.transform.position, g.transform.position, new Color(1.0f, 0.5f, 0.0f));
        }
    }
}
