using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using ExtensionMethods;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

//100*100を50,25...と分割していき確率で部屋を作る
public class MapPresenter : MonoBehaviour {

	public int maxMapX = 100;
    public int maxMapY = 100;
    public class IsPath
    {
        public bool left { get; set; } = true;
        public bool right { get; set; } = true;
        public bool up { get; set; } = true;
        public bool down { get; set; } = true;
        
        public IsPath() {}
        public IsPath(bool left, bool right, bool up, bool down) { this.down = down; this.up = up; this.right = right; this.left = left; }
    }

    public Dictionary<Vector2Int, TileModel> map = new Dictionary<Vector2Int, TileModel>();
    public Dictionary<Vector2Int, FloorModel> floorList = new Dictionary<Vector2Int, FloorModel>();
    public Dictionary<Vector2Int, GameObject> mapListObject = new Dictionary<Vector2Int, GameObject>();

    public List<Vector2Int> popPoint = new List<Vector2Int>();

    private void Initialize()
    {
        //map初期化
        for (int x = 0; x < maxMapX; x++)
        {
            for (int z = 0; z < maxMapY; z++)
            {
                map[new Vector2Int(x,z)] = new TileModel();
            }
        }
    }
    public void Generate()
    {
        Initialize();
        
        // mapModel生成
        // 横軸をGenerateMapで分割
        List<int> mapX = GenerateMap (maxMapX);
        int countX = 0;
        int floorId = 1;
        int maxSplitY = 0;
        for (int x = 0; x < mapX.Count; x++)
        {
            //縦軸をGenerateMapで分割
            List<int> mapY = GenerateMap (maxMapY);
            maxSplitY = mapY.Count > maxSplitY ? mapY.Count : maxSplitY;
            int countY = 0;
            for (int y = 0; y < mapY.Count; y++){
                //部屋サイズ
                Vector2Int floorSize = new Vector2Int(mapX[x], mapY[y]);
                Vector2Int floorPoint = new Vector2Int(countX, countY);
                
                //通路を作るかどうか
                var pathType = Enum.GetValues(typeof(GameConfig.Around4Type)).Cast<GameConfig.Around4Type>().ToList();
                if (x == 0)
                {
                    pathType.RemoveAll(i => i == GameConfig.Around4Type.Left);
                }
                if (x == mapX.Count - 1)
                {
                    pathType.RemoveAll(i => i == GameConfig.Around4Type.Right);
                }
                if (y == 0)
                {
                    pathType.RemoveAll(i => i == GameConfig.Around4Type.Up);
                }
                if (y == mapY.Count - 1)
                {
                    pathType.RemoveAll(i => i == GameConfig.Around4Type.Down);
                }
                
                //部屋作成
                floorList[new Vector2Int(x,y)] = GenerateRoom(floorId, floorSize, floorPoint, pathType);
                floorId++;
                
                //次の部屋のy軸記憶
                countY += mapY[y];
            }
            //次の部屋のx軸記憶
            countX += mapX[x];
        }
        
        // 通路繋ぐ
        for (int x = 0; x < mapX.Count; x++)
        {
            //最大分割数を取得できないのでとりあえず10分割
            for (int y = 0; y < maxSplitY; y++)
            {
                if (floorList.ContainsKey(new Vector2Int(x, y)))
                {
                    ConnectPath(new Vector2Int(x, y));
                }
            }
        }

        // mapオブジェクト生成
        for (int x = 0; x < maxMapX; x++)
        {
            for (int z = 0; z < maxMapY; z++)
            {
                GenerateTileObject(new Vector2Int(x,z));
            }
        }
	}

    List<int> GenerateMap(int size){
        return SplitMap(new List<int>{size});
    }

    List<int> SplitMap(List<int> data){
        //dataを繰り返し分割していく
        //例:100→50,50→25,25,50
        for (int x = 0; x < data.Count; x++) {
            if (data [x] < 20) {
                continue;
            }
            //11分の9でマップを分割する
            if (Random.Range (1, 11) > 2) {
                int split = data [x] / 2;
                data [x] = split;
                data.Insert (x, split);
                x--;
            } 
        }
        return data;
    }
    
    /// <summary>
    /// 四角いエリアにroomを生成
    /// </summary>
    /// <param name="floorSize">floorの縦幅</param>
    /// <param name="floorPoint">開始縦横座標</param>
    /// <param name="floorId"></param>
    /// <returns></returns>
    FloorModel GenerateRoom(int floorId, Vector2Int floorSize, Vector2Int floorPoint, List<GameConfig.Around4Type> pathType)
    {
        //roomの部屋の大きさ
        Vector2Int roomSize = new Vector2Int(Random.Range(5, floorSize.x-2), Random.Range(5, floorSize.y-2));
        //roomの開始xy座標
        Vector2Int roomPoint = new Vector2Int(Random.Range(1, floorSize.x-roomSize.x-1) + floorPoint.x,Random.Range(1, floorSize.y-roomSize.y-1) + floorPoint.y);
        
        //通路座標
        Vector2Int pathUp = new Vector2Int(Random.Range(roomPoint.x, roomPoint.x + roomSize.x), floorPoint.y);
        Vector2Int pathDown = new Vector2Int(Random.Range(roomPoint.x, roomPoint.x + roomSize.x), floorPoint.y + floorSize.y);
        Vector2Int pathLeft = new Vector2Int(floorPoint.x,Random.Range(roomPoint.y, roomPoint.y + roomSize.y));
        Vector2Int pathRight = new Vector2Int(floorPoint.x + floorSize.x,Random.Range(roomPoint.y, roomPoint.y + roomSize.y));
                
        //通路を何方向に出すかランダムで取得
        var path = pathType.OrderBy(a => Guid.NewGuid ()).ToArray().Take(Random.Range(1, pathType.Count));
        
        for (int x = floorPoint.x; x < floorPoint.x + floorSize.x; x++)
        {
            for (int y = floorPoint.y; y < floorPoint.y + floorSize.y; y++)
            {
                var pos = new Vector2Int(x, y);
                if (x >= roomPoint.x && x <= roomPoint.x + roomSize.x && y >= roomPoint.y && y <= roomPoint.y + roomSize.y)
                {
                    SetTileModel(TileModel.TileType.Floor, pos, floorId);
                    popPoint.Add(pos);
                }
                else if (pathUp.x == x && y < roomPoint.y && path.Any(i => i == GameConfig.Around4Type.Up))
                {
                    // 上通路
                    SetTileModel(TileModel.TileType.Path, pos, floorId);
                }
                else if(pathDown.x == x &&  y > roomPoint.y + roomSize.y && path.Any(i => i == GameConfig.Around4Type.Down))
                {
                    // 下通路        
                    SetTileModel(TileModel.TileType.Path, pos, floorId);                    
                }
                else if(pathLeft.y == y &&  x < roomPoint.x && path.Any(i => i == GameConfig.Around4Type.Left))
                {   
                    // 左通路   
                    SetTileModel(TileModel.TileType.Path, pos, floorId);                    
                }
                else if(pathRight.y == y &&  x > roomPoint.x + roomSize.x && path.Any(i => i == GameConfig.Around4Type.Right))
                {
                    // 右通路   
                    SetTileModel(TileModel.TileType.Path, pos, floorId);                    
                }
                else
                {
                    SetTileModel(TileModel.TileType.Wall, pos, floorId);
                }
            }
        }
        
        return new FloorModel(floorId, floorSize, floorPoint, roomSize, roomPoint,
            pathUp, pathDown, pathLeft, pathRight);
    }

    /// <summary>
    /// 通路をつなぐ
    /// 検索範囲をfloorの上下通路の場合は左右に、floorの左右通路の場合は上下に、
    /// 端から広げていき一番近いfloorTileまでつなげる
    /// </summary>
    /// <param name="floorIndex"></param>
    private void ConnectPath(Vector2Int floorIndex)
    {
        // 上下検索
        for (var x = 1 ; x < maxMapX / 2; x++)
        {
            foreach (var n in new[] {1,-1})
            {
                var searchPath = floorList[floorIndex].pathUp.AddXY(x * n, -1);

                if (map.ContainsKey(searchPath) && map[searchPath].tileType == TileModel.TileType.Floor)
                {
                    for (var l = 0; l <= x; l++ )
                    {
                        SetTileModel(TileModel.TileType.Path, floorList[floorIndex].pathUp.AddX(l * n), floorList[floorIndex].floorId); 
                    }
                    break;
                }
            }
        }

        for (var x = 1; x < maxMapX / 2; x++)
        {
            foreach (var n in new[] {1,-1})
            {
                var searchPath = floorList[floorIndex].pathDown.AddXY(x * n, 1);

                if (map.ContainsKey(searchPath) && map[searchPath].tileType == TileModel.TileType.Floor)
                {
                    for (var l = 0; l <= x; l++ )
                    {
                        SetTileModel(TileModel.TileType.Path, floorList[floorIndex].pathDown.AddX(l * n), floorList[floorIndex].floorId); 
                    }
                    break;
                }
            }
        }
                
        // 左右検索
        for (var y = 1 ; y < maxMapY / 2; y++)
        {
            foreach (var n in new[] {1,-1})
            {
                var searchPath = floorList[floorIndex].pathLeft.AddXY(-1, y * n);

                if (map.ContainsKey(searchPath) && map[searchPath].tileType == TileModel.TileType.Floor)
                {
                    for (var l = 0; l <= y; l++ )
                    {
                        SetTileModel(TileModel.TileType.Path, floorList[floorIndex].pathLeft.AddY(l * n), floorList[floorIndex].floorId); 
                    }
                    break;
                }
            }
        }

        for (var y = 1 ; y < maxMapY / 2; y++)
        {
            foreach (var n in new[] {1, -1})
            {
                var searchPath = floorList[floorIndex].pathRight.AddXY(1, y * n);

                if (map.ContainsKey(searchPath) && map[searchPath].tileType == TileModel.TileType.Floor)
                {
                    for (var l = 0; l <= y; l++ )
                    {
                        SetTileModel(TileModel.TileType.Path, floorList[floorIndex].pathRight.AddY(l * n), floorList[floorIndex].floorId); 
                    }
                    break;
                }
            }
        }
    }
    public void SetTileModel(TileModel.TileType tileType, Vector2Int position, int floorId)
    {        
        var t = GetTileModel(position);
        t.tileType = tileType;
        t.floorId = floorId;    
    }
    public void GenerateTileObject(Vector2Int position)
    {
        var t = GetTileModel(position);
        if (t.tileType == TileModel.TileType.Floor)
        {
            GameObject obj = Object.Instantiate(Resources.Load("Object/Map/Tile"), transform, true) as GameObject;            
            obj.transform.Translate(position.x, 0, position.y);
            mapListObject[position] = obj;
        } else if (t.tileType == TileModel.TileType.Path)
        {
            GameObject original = Object.Instantiate(Resources.Load("Object/Map/Tile"), transform, true) as GameObject;            
            original.transform.Translate(position.x, 0, position.y);
            mapListObject[position] = original;
        } else if (t.tileType == TileModel.TileType.Wall)
        {
            GameObject original = Object.Instantiate(Resources.Load("Object/Map/Block"), transform, true) as GameObject;            
            original.transform.Translate(position.x, 0, position.y);
            mapListObject[position] = original;
        }        
    }

    public void Regenerate()
    {
        Scene loadScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(loadScene.name);
    }

    void GenerateWall (int[,] data){
        for (int x = 0; x < data.Length; x++) {
            for (int y = 0; y < data.Length; y++) {
                if(data[x+1,y] == 1){
                    
                }
            }
        }
    }

	void ShowListLog(List<int> data){
		string log = "";
		for (int x = 0; x < data.Count; x++) {
			log += data[x]+",";
		}
	}

    public Vector2Int GetPopPoint()
    {
        for (int l = 0; l < 1000; l++){
            int pos_s = Random.Range(0, popPoint.Count - 1);
            var pos = popPoint[pos_s];
            var t = GetTileModel(pos);
			if (t.tileType == TileModel.TileType.Floor && 
                t.charaType == TileModel.CharaType.None &&
                t.itemGuid == 0)
            {
                return pos;
            }
        }
        //全部map埋まってたりしたら諦める。
        return new Vector2Int( 0, 0 );
    }
    
    /// <summary>
    /// 移動できるかどうか
    /// </summary>
    /// <param name="axis"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public bool IsCanMove(Vector2Int axis, CharacterPresenter characterPresenter)
    {
        Vector2Int beforePosition = new Vector2Int((int) characterPresenter.status.position.x, (int) characterPresenter.status.position.z);
        Vector2Int afterPosition = new Vector2Int(beforePosition.x + axis.x, beforePosition.y + axis.y);
        
        //移動先が0以上
        if (afterPosition.x > 0 && afterPosition.y > 0)
        {            
            var t = GetTileModel(new Vector2Int(afterPosition.x, afterPosition.y));
            
            // 斜め移動の際は壁がないことを確認
            if (axis.x != 0 && axis.y != 0)
            {
                if (axis.x == -1 && axis.y == -1)
                {
                    if (GetTileModel(beforePosition + Vector2Int.down).tileType == TileModel.TileType.Wall ||
                        GetTileModel(beforePosition + Vector2Int.left).tileType == TileModel.TileType.Wall
                        )
                    {
                        return false;
                    }                    
                }
                if (axis.x == 1 && axis.y == -1)
                {
                    if (GetTileModel(beforePosition + Vector2Int.down).tileType == TileModel.TileType.Wall ||
                        GetTileModel(beforePosition + Vector2Int.right).tileType == TileModel.TileType.Wall
                    )
                    {
                        return false;
                    }                    
                }
                if (axis.x == -1 && axis.y == 1)
                {
                    if (GetTileModel(beforePosition + Vector2Int.up).tileType == TileModel.TileType.Wall ||
                        GetTileModel(beforePosition + Vector2Int.left).tileType == TileModel.TileType.Wall
                    )
                    {
                        return false;
                    }                    
                }
                if (axis.x == 1 && axis.y == 1)
                {
                    if (GetTileModel(beforePosition + Vector2Int.up).tileType == TileModel.TileType.Wall ||
                        GetTileModel(beforePosition + Vector2Int.right).tileType == TileModel.TileType.Wall
                    )
                    {
                        return false;
                    }                    
                }
            }
            
            if (characterPresenter.status.type == TileModel.CharaType.Player)
            {
                if (
                    (t.tileType == TileModel.TileType.Floor || t.tileType == TileModel.TileType.Path || t.tileType == TileModel.TileType.Stairs) && 
                    t.charaType == TileModel.CharaType.None)
                {
                    return true;
                }
            }else if (characterPresenter.status.type == TileModel.CharaType.Enemy)
            {
                if ((t.tileType == TileModel.TileType.Floor || t.tileType == TileModel.TileType.Path) && t.charaType == TileModel.CharaType.None)
                {
                    return true;
                }
            }
        }

        return false;
    }

    /// <summary>
    /// 八方向List<int>を返す
    /// </summary>
    /// <returns></returns>
    public Vector2Int CanSetObject(Vector2Int pos)
    {
        List<Vector2Int> directions = new List<Vector2Int>
        {
            new Vector2Int (-1, -1),
            new Vector2Int (-1, 0),
            new Vector2Int (-1, 1),
            new Vector2Int (0, -1),
            new Vector2Int (0, 1),
            new Vector2Int (1, -1),
            new Vector2Int (1, 0),
            new Vector2Int (1, 1),
        };

        foreach (Vector2Int direction in directions)
        {
            Vector2Int resPosition = new Vector2Int(pos[0] + direction[0], pos[1] + direction[1]);
            var vector2Int = new Vector2Int(resPosition.x, resPosition.y);
            if (GetTileModel(vector2Int).tileType == TileModel.TileType.Floor && GetTileModel(vector2Int).charaType == 0)
            {
                return resPosition;
            }
        }

        return new Vector2Int (0, 0);
    }
    
    public void SetUserModel(Vector2Int pos, UserModel user)
    {
        var t = GetTileModel(pos);
        if (user != null)
        {
            t.guid = user.guid;
            t.charaId = user.id;
            t.charaType = user.type;
        }
        else
        {            
            t.guid = 0;
            t.charaId = 0;
            t.charaType = TileModel.CharaType.None;
        }
    }
    
    public void SetItemModel(int x, int z, ItemModel item)
    {
        var t = GetTileModel(new Vector2Int(x, z));
        if (item != null)
        {
            t.itemGuid = item.guid;
            t.itemId = item.id;
        }
        else
        {            
            t.itemGuid = 0;
            t.itemId = 0;
        }
    }

    public void ShowMapInfo()
    {
        for (int z = maxMapY-1; z >= 0; z--)
        {
            string m = "";
            for (int x = 0; x < maxMapX; x++)
            {
                var t = GetTileModel(new Vector2Int(x, z));
                string m1 = t.tileType.ToString().Substring(0, 1);
                string m2 = t.charaType.ToString().Substring(0, 1);
                string m3 = t.floorId.ToString();
                m += m1+m2+m3+",";
            }
            Debug.Log(m);
        }
    }

    public TileModel GetTileModel(Vector2Int pos)
    {
        return map[pos];
    }
    public TileModel GetTileModel(int x, int y)
    {
        return map[new Vector2Int(x,y)];
    }

    public bool SearchCharaType(Vector2Int v2, TileModel.CharaType t)
    {
        if (map[v2].charaType == t)
        {
            return true;
        }
        return false;
    }
    
    public bool SearchTileType(Vector2Int v2, TileModel.TileType t)
    {
        if (map[v2].tileType == t)
        {
            return true;
        }
        return false;
    }
}
