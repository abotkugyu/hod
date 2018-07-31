using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
                 
public class Hud : MonoBehaviour {

    public Slider hp;
    // Use this for initialization
    void Start()
    {
        hp = (GameObject.Find("HealthSlider")).GetComponent<Slider>();
    }

    public void update_health(int health)
    {
        hp.value = health;
    }
}
