using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gameEndScript : MonoBehaviour
{
    [SerializeField] private bool isEnd = false;
    private GameObject mainCamera;
    public GameObject[] endBlocks;
    private DateBlockControl[] isSnap;
    private GameObject myCamera;
    private Vector3 finalPosition;
    private Quaternion finalRotation;
    private GameObject finalImage;
    public float speed = 3;
    public int imageTime = 25;
    // Start is called before the first frame update
    void Start()
    {
        isSnap = new DateBlockControl[endBlocks.Length];
        mainCamera = GameObject.Find("Main Camera");
        myCamera = GameObject.Find("My Camera");
        finalImage = GameObject.Find("EndImage");

        for (int i = 0; i < endBlocks.Length; i += 1)
        {
            isSnap[i] = endBlocks[i].GetComponent<DateBlockControl>();
        }
        finalPosition = myCamera.transform.position;
        finalRotation = myCamera.transform.rotation;
        myCamera.SetActive(false);
    }
    private void Update()
    {
        if (!isEnd)//This will run until isEnd = true, and then never run again.
        {
            bool allSnap = true;
            for (int i = 0; i < endBlocks.Length; i += 1)
            {
                if (isSnap[i].isSnap == false)
                {
                    allSnap = false;
                }
            }
            if (allSnap == true)
            {
                isEnd = true;
            }
            if (isEnd)
            {
                StartCoroutine(moveCamera());
                endgameAudio();
            }
        }
    }

    private IEnumerator moveCamera()
    {
        int i = 0;
        myCamera.SetActive(true);
        mainCamera.SetActive(false);
        myCamera.transform.position = mainCamera.transform.position;
        myCamera.transform.rotation = mainCamera.transform.rotation;
        while (myCamera.transform.position != finalPosition && myCamera.transform.rotation != finalRotation)
        {
            myCamera.transform.position = Vector3.Lerp(myCamera.transform.position, finalPosition, speed * Time.deltaTime);
            myCamera.transform.rotation = Quaternion.Lerp(myCamera.transform.rotation, finalRotation, speed * Time.deltaTime);
            i += 1;
            if (i == imageTime)
            {
                StartCoroutine(fadeImage());
            }
            yield return null;
        }
        if (i < imageTime)
        {
            StartCoroutine(fadeImage());
        }
    }
    private IEnumerator fadeImage()
    {
        Color originalColor = finalImage.GetComponent<RawImage>().color;
        for (float i = 0.01f; i < speed; i += Time.deltaTime)
        {
            finalImage.GetComponent<RawImage>().color = Color.Lerp(originalColor, Color.white, Mathf.Min(1, i / speed));
            yield return null;
        }
    }
    private void endgameAudio()
    {
        AudioSource myAudio = GetComponent<AudioSource>();
        myAudio.Play();
    }
}
