using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HudPresenter : MonoBehaviour {
    
    [SerializeField]
    public HudView hudView;
    public void UpdateHud(UserModel userModel)
    {
        hudView.updateHealth(userModel.hp);
        hudView.updateMagic(userModel.mp);
        hudView.updateEnergy(userModel.ep);
    }
}
