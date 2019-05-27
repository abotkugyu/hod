using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public static class EnemyData{

    private static EnemyModelJson master;

    public static void Load()
    {
        TextAsset master_json = Resources.Load<TextAsset>("Master/Enemy");
        master = JsonUtility.FromJson<EnemyModelJson>(master_json.ToString());
        Debug.Log(String.Format("Load Success Enemy Data : {0}",master.list[0].name));
    }    

    public static UserModel GetRandom()
    {
        return master.list[Random.Range(0, master.list.Length-1)];
    }
}

public class EnemyModelJson
{
    public UserModel[] list;
}