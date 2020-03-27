using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class subtitleScript : MonoBehaviour
{
    public GameObject player;
    public float textTime = 5f;
    public GameObject subtitleUi;
    public string storyText = "Story Text";
    private bool isfade;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        subtitleUi = GameObject.Find("Subtitles");

    }
    private void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            subtitleUi.GetComponent<UnityEngine.UI.Text>().text = GetComponent<UnityEngine.UI.Text>().text;
            subtitleUi.GetComponent<UnityEngine.UI.Text>().color = Color.white;
            isfade = false;
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            isfade = true;
            StartCoroutine(Fade());
        }
    }

    private IEnumerator Fade()
    {
        Color originalColor = subtitleUi.GetComponent<UnityEngine.UI.Text>().color;
        for (float t = 0.01f; t < textTime; t += Time.deltaTime)
        {
            if (!isfade) break;
            if (player.GetComponent<DeathControl>().checkpointBlock != gameObject) break;
            subtitleUi.GetComponent<UnityEngine.UI.Text>().color = Color.Lerp(originalColor, Color.clear, Mathf.Min(1, t / textTime));
            yield return null;
        }
        isfade = false;
    }
}
