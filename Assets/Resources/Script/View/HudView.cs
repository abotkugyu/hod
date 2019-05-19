using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
                 
public class HudView : MonoBehaviour {

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
    
    /*
    //ダメージ計算
    void set_hp(int value)
    {
        HudView hudView = (GameObject.Find("Hud")).GetComponent<HudView>();
        hudView.update_health(value);
    }
    //mp計算
    void set_mp(int value)
    {
        HudView hudView = (GameObject.Find("Hud")).GetComponent<HudView>();
        hudView.update_magic(value);
    }
    //ep計算
    void set_ep(int value)
    {
        HudView hudView = (GameObject.Find("Hud")).GetComponent<HudView>();
        hudView.update_energy(value);
    }
     */
}
