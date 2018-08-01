using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
                 
public class Hud : MonoBehaviour {

    public Slider hp;
    public Slider mp;
    public Slider ep;
    // Use this for initialization
    void Start()
    {
        hp = (GameObject.Find("HealthSlider")).GetComponent<Slider>();
        mp = (GameObject.Find("MagicSlider")).GetComponent<Slider>();
        ep = (GameObject.Find("EnergySlider")).GetComponent<Slider>();
    }

    public void update_health(int value)
    {
        hp.value = value;
    }

    public void update_magic(int value)
    {
        mp.value = value;
    }

    public void update_energy(int value)
    {
        ep.value = value;
    }
}
