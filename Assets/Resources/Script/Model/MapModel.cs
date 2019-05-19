using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MapModel
{
    public int map_type;
}

//1*1のタイル情報
public class TileModel
{
    /// 0=壁
    /// 1=床
    /// 2=水
    /// 3=階段
    public int tile_type = 0;
    
    /// charactor_id (objects instance id)
    public int chara_id;
    
    /// 現在のタイルにいるキャラクター
    /// 0=none
    /// 1=player
    /// 2=enemy
    /// 3=support
    public int chara_type = 0;
    
    /// <summary>
    /// item_id (objects instance id)
    /// </summary>
    public int item_id;
    
    /// 現在のタイルにあるアイテム
    public int item_type = 0;
    
    /// 高さ
    public int height = 0;
    
    /// 罠 0=none 1~=any
    public int trap_type = 0;
    
    /// 罠が見えているか
    public int is_visible_trap = 0;

}
