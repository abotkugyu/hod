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
    //0:壁、1:床、2:水 3:階段
    public int tile_type = 0;
    //キャラクター_id
    public int chara_id;
    //現在のタイルにいるキャラクター 0:none 1:player 2:enemy 3:support
    public int chara_type = 0;
    //高さ
    public int height = 0;
    //罠 0:none 1~:any
    public int trap_type = 0;
    //罠が見えているか
    public int is_visible_trap = 0;
}