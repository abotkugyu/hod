using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MapStatus
{
    public int map_type;
}

//1*1のタイル情報
public class TileStatus
{
    //現在のタイルにいるキャラクター 0:none 1:my 2:enemy 3:support
    public int on_type;
    //高さ
    public int height;
    //キャラクター_id
    public int chara_id;
    //罠 0:none 1~:any
    public int trap_type;
    //罠が見えているか
    public int is_visible_trap;
    //
}