using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public static class ItemData{

    private static ItemModelJson master;

    public static void Load()
    {
        TextAsset item_master_json = Resources.Load<TextAsset>("Master/Item");
        master = JsonUtility.FromJson<ItemModelJson>(item_master_json.ToString());
        Debug.Log(String.Format("Load Success Item Data : {0}",master.list[0].name));
    }

    public static ItemModel GetRandom()
    {
        return master.list[Random.Range(0, master.list.Length-1)];
    }
}
public class ItemModelJson
{
    public ItemModel[] list;
}

