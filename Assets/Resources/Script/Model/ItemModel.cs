using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
//item情報
//id : アイテム_id
//type : 1:お金 2:武器 3:防具 4:装飾品 5:銃弾 6:食べ物
//sub_type : typeに応じた小分類
//value : 効果値
//used_count : 使用回数
//remain_count : 残り回数
//max_count : 最大数
public class ItemModel
{
    public ItemModel()
    {
        
    }
    public ItemModel(int id, string name)
    {
        id = id;
        name = name;
    }
    
    public int guid;
    public int id;
    public string name;
    public int type = 0;
    public int sub_type = 0;
    public int value = 0;
    public Vector3 position = new Vector3(0, 0, 0);
    [FormerlySerializedAs("use_count")] public int useCount = 0;    //使用回数
    public int remain_count = 0; //残り回数
   
}