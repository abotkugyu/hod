using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class ItemModel
{
    public ItemModel(ItemModel model)
    {
        id = model.id;
        name = model.name;
        type  = model.type;
        subType = model.subType;
        value = model.value;
        remainCount = model.remainCount;
        useCount = model.useCount;
        modelName = model.modelName;
        attack = model.attack;
        defence = model.defence;
    }

    public ItemModel(int id, string name)
    {
        id = id;
        name = name;
    }
    
    public int guid;
    /// id : アイテムid
    public int id;
    /// name : アイテム名
    public string name;
    /// type : 1:武器 2:防具 3:弓 4:杖 5:食べ物 6:薬草 7:矢 8:箱 9:お金
    public int type = 0;
    /// sub_type : typeに応じた小分類
    public int subType = 0;
    /// value : 効果値
    public int value = 0;
    /// attack : 効果値
    public int attack = 0;
    /// defence : 効果値
    public int defence = 0;
    /// position : 座標
    public Vector3 position = new Vector3(0, 0, 0);
    /// used_count : 使用回数
    public int useCount = 0;    //使用回数
    /// remain_count : 残り回数
    public int remainCount = 0; //残り回数
    /// model_name
    public string modelName = ""; //モデル名
    
    public int floorId = 0;    
    //向き(デフォルト下向き)
    public Vector3 direction = new Vector3(0, 0, -1);
   
}