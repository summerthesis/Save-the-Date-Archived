using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class expendable_lights : MonoBehaviour
{
    GameObject lightHolder;
    GameObject red_Spot;
    GameObject green_Spot;
    GameObject blue_Spot;
    Transform player;

    void Awake() {
        player = GameObject.FindWithTag("Player").transform;

        lightHolder = new GameObject();
        lightHolder.name = "LightHolder";
        lightHolder.transform.parent = this.gameObject.transform;
        lightHolder.transform.position = this.gameObject.transform.position + Vector3.up * 5;
        lightHolder.transform.rotation = this.gameObject.transform.rotation;
        
        Vector3 rePosition = Vector3.forward;
        red_Spot =  new GameObject();
        red_Spot.name = "RedSpot";
        red_Spot.transform.parent = lightHolder.transform;
        red_Spot.transform.position = lightHolder.transform.position + rePosition;
        red_Spot.AddComponent<Light>();

        rePosition = new Vector3(-Mathf.Sqrt(3) / 2, 0, -1);
        green_Spot = new GameObject();
        green_Spot.name = "GreenSpot";
        green_Spot.transform.parent = lightHolder.transform;
        green_Spot.transform.position = lightHolder.transform.position + rePosition;
        green_Spot.AddComponent<Light>();

        rePosition = new Vector3(Mathf.Sqrt(3) / 2, 0, -1);
        blue_Spot = new GameObject();
        blue_Spot.name = "BlueSpot";
        blue_Spot.transform.parent = lightHolder.transform;
        blue_Spot.transform.position = lightHolder.transform.position + rePosition;
        blue_Spot.AddComponent<Light>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Light redLight = red_Spot.GetComponent<Light>();
        Light greenLight = green_Spot.GetComponent<Light>();
        Light blueLight = blue_Spot.GetComponent<Light>();

        redLight.type = LightType.Spot;
        greenLight.type = LightType.Spot;
        blueLight.type = LightType.Spot;

        redLight.color = Color.red;
        greenLight.color = Color.green;
        blueLight.color = Color.blue;

        redLight.range = 20;
        greenLight.range = 20;
        blueLight.range = 20;

        redLight.spotAngle = 120;
        greenLight.spotAngle = 120;
        blueLight.spotAngle = 120;

        redLight.intensity = 7;
        greenLight.intensity = 7;
        blueLight.intensity = 7;
    }

    // Update is called once per frame
    void Update()
    {
        lightHolder.transform.position = player.transform.position + Vector3.up * 5;
        lightHolder.transform.Rotate(this.gameObject.transform.up, Space.Self);
        red_Spot.transform.LookAt(player);
        green_Spot.transform.LookAt(player);
        blue_Spot.transform.LookAt(player);
    }
}
