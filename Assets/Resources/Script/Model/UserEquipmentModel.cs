using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UserEquipmentModel
{
    
    public UserEquipmentModel(UserEquipmentModel model)
    {
        id = model.id;
    }
    
    public int id = 0;
    
    public int[] weaponIds;
    public int[] armorIds;
    public int[] ringIds;
}
