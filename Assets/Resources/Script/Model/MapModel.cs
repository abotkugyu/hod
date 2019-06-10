using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
//1*1のタイル情報
public class TileModel
{
    public enum TileType
    {
        Wall,        
        Floor,
        Water,
        Stairs
    }
    
    public enum CharaType
    {
        None,        
        Player,
        Enemy,
        Support
    }
    
    /// 0=壁
    /// 1=床
    /// 2=水
    /// 3=階段
    public TileType tileType = 0;

    public int floorId = 0;
    
    public int guid;
    
    /// charactor_id (objects instance id)
    public int charaId;
    
    /// 現在のタイルにいるキャラクター
    /// 0=none
    /// 1=player
    /// 2=enemy
    /// 3=support
    public CharaType charaType = 0;
    
    /// <summary>
    /// item_id (objects instance id)
    /// </summary>
    public int itemGuid;
    
    /// 現在のタイルにあるアイテム
    public int itemType = 0;
    
    /// 高さ
    public int height = 0;
    
    /// 罠 0=none 1~=any
    public int trapType = 0;
    
    /// 罠が見えているか
    public int isVisibleTrap = 0;

}
