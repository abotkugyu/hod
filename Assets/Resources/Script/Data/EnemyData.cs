using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public static class EnemyData{

    private static JsonModel master;

    public static void Load()
    {
        TextAsset masterJson = Resources.Load<TextAsset>("Master/Enemy");
        master = JsonUtility.FromJson<JsonModel>(masterJson.ToString());
        Debug.Log(String.Format("Load Success Enemy Data : {0}",master.list[0].name));
    }    

    public static UserModel GetRandom()
    {
        return master.list[Random.Range(0, master.list.Length-1)];
    }
    
    public static List<UserModel> GetRandoms(int n)
    {
        List<UserModel> models = new List<UserModel>();
        for (int i = 0; i < n; i++)
        {
            models.Add(master.list[Random.Range(0, master.list.Length-1)]);
        }
        return models;
    }
    class JsonModel
    {
        public UserModel[] list;
    }
}
