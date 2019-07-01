using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
                 
public class HudView : MonoBehaviour {

    public Slider hp;
    public Slider mp;
    public Slider ep;

    public void updateHealth(int value)
    {
        hp.value = value;
    }

    public void updateMagic(int value)
    {
        mp.value = value;
    }

    public void updateEnergy(int value)
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
