using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        Stairs,
        Path
    }
    
    public enum CharaType
    {
        None,        
        Player,
        Enemy,
        Support
    }
    
    /// 0:壁 1:床 2:水 3:階段
    public TileType tileType = 0;

    public int floorId = 0;
    
    public int guid;
    
    /// characterId (objects instance id)
    public int charaId;
    
    /// 現在のタイルにいるキャラクター
    /// 0:none 1:player 2:enemy 3:support
    public CharaType charaType = 0;
    
    /// <summary>
    /// itemId (objects instance id)
    /// </summary>
    public int itemGuid;
    
    /// 現在のタイルにあるアイテム
    public int itemId = 0;
    
    /// 高さ
    public int height = 0;
    
    /// 罠 0:none 1~:any
    public int trapType = 0;
    
    /// 罠が見えているか
    public int isVisibleTrap = 0;

}


public class FloorModel
{
    public enum FloorType
    {
        Default,
    }

    public FloorModel(int floorId, Vector2Int floorSize, Vector2Int floorPoint, Vector2Int roomSize ,Vector2Int roomPoint,
        PathModel outerPath, PathModel innerPath)
    {
        this.floorId = floorId;
        this.floorSize = floorSize;
        this.roomSize = roomSize;
        this.floorPoint = floorPoint;
        this.roomPoint = roomPoint;
        this.outerPath = outerPath;
        this.innerPath = innerPath;
    }

    public int floorId;
    public Vector2Int floorSize;
    public Vector2Int roomSize;
    public Vector2Int floorPoint;
    public Vector2Int roomPoint;
    
    
    public PathModel outerPath;
    public PathModel innerPath;

}

public class PathModel
{
    public Vector2Int up = Vector2Int.zero;
    public Vector2Int down = Vector2Int.zero;
    public Vector2Int left = Vector2Int.zero;
    public Vector2Int right = Vector2Int.zero;

    public Vector2Int GetRandom()
    {
        var to = new List<Vector2Int>();
        if (this.up != Vector2Int.zero) to.Add(this.up);
        if (this.down != Vector2Int.zero) to.Add(this.down);
        if (this.left != Vector2Int.zero) to.Add(this.left);
        if (this.right != Vector2Int.zero) to.Add(this.right);
        if (to.Count > 0)
        {
            return to.OrderBy(i => Guid.NewGuid()).First();
        }

        return Vector2Int.zero;
    }
}