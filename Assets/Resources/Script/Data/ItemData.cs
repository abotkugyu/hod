using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public static class ItemData{

    private static ItemModelJson item_master;

    public static void Load()
    {
        TextAsset item_master_json = Resources.Load<TextAsset>("Master/Item");
        item_master = JsonUtility.FromJson<ItemModelJson>(item_master_json.ToString());
        Debug.Log(String.Format("Load Success Item Data : {0}",item_master.item[0].name));
    }

    public static ItemModel GetRandom()
    {
        return item_master.item[Random.Range(0, item_master.item.Length-1)];
    }
}
