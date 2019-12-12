using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public static class ItemData{

    private static JsonModel master;

    public static void Load()
    {
        TextAsset masterJson = Resources.Load<TextAsset>("Master/Item");
        master = JsonUtility.FromJson<JsonModel>(masterJson.ToString());
        Debug.Log(String.Format("Load Success Item Data : {0}", master.list[0].name));
    }
    public static List<ItemModel> GetRandoms(int n)
    {
        List<ItemModel> models = new List<ItemModel>();
        for (int i = 0; i < n; i++)
        {
            models.Add(master.list[Random.Range(0, master.list.Length-1)]);
        }
        return models;
    }
    
    public static ItemModel GetRandom()
    {
        return master.list[Random.Range(0, master.list.Length-1)];
    }
    class JsonModel
    {
        public ItemModel[] list;
    }
}

