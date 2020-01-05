using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;


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
                var isPath = new IsPath();
                if (x == 0)
                {
                    isPath.left = false;
                }
                if (x == mapX.Count - 1)
                {
                    isPath.right = false;
                }
                if (y == 0)
                {
                    isPath.up = false;
                }
                if (y == mapY.Count - 1)
                {
                    isPath.down = false;
                }
                
                //部屋作成
                floorList[new Vector2Int(x,y)] = GenerateRoom(floorId, floorSize, floorPoint, isPath);
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
    FloorModel GenerateRoom(int floorId, Vector2Int floorSize, Vector2Int floorPoint, IsPath isPath)
    {
        //roomの部屋の大きさ
        Vector2Int roomSize = new Vector2Int(Random.Range(5, floorSize.x-2), Random.Range(5, floorSize.y-2));
        //roomの開始xy座標
        Vector2Int roomPoint = new Vector2Int(Random.Range(1, floorSize.x-roomSize.x-1) + floorPoint.x,Random.Range(1, floorSize.y-roomSize.y-1) + floorPoint.y);
                              
        //通路に使う座標を辺からランダムで取得
        Vector2Int pathUp = new Vector2Int(Random.Range(roomPoint.x, roomPoint.x + roomSize.x), roomPoint.y);
        if (!isPath.up)
        {
            pathUp = new Vector2Int(-1,-1);
        }
        Vector2Int pathDown = new Vector2Int(Random.Range(roomPoint.x, roomPoint.x + roomSize.x), roomPoint.y + roomSize.y);
        if (!isPath.down)
        {
            pathDown = new Vector2Int(-1,-1);
        }
        Vector2Int pathLeft = new Vector2Int(roomPoint.x,Random.Range(roomPoint.y, roomPoint.y + roomSize.y));
        if (!isPath.left)
        {
            pathLeft = new Vector2Int(-1,-1);
        }
        Vector2Int pathRight = new Vector2Int(roomPoint.x + roomSize.x,Random.Range(roomPoint.y, roomPoint.y + roomSize.y));
        if (!isPath.right)
        {
            pathRight = new Vector2Int(-1,-1);
        }
        
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
                else if (pathUp.x == x && y < roomPoint.y && isPath.up)
                {
                    // 上通路
                    SetTileModel(TileModel.TileType.Floor, pos, floorId);
                }
                else if(pathDown.x == x &&  y > roomPoint.y + roomSize.y && isPath.down)
                {
                    // 下通路        
                    SetTileModel(TileModel.TileType.Floor, pos, floorId);                    
                }
                else if(pathLeft.y == y &&  x < roomPoint.x && isPath.left)
                {   
                    // 左通路   
                    SetTileModel(TileModel.TileType.Floor, pos, floorId);                    
                }
                else if(pathRight.y == y &&  x > roomPoint.x + roomSize.x && isPath.right)
                {
                    // 右通路   
                    SetTileModel(TileModel.TileType.Floor, pos, floorId);                    
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
            var searchPathLeft =  new Vector2Int(floorList[floorIndex].pathUp.x - x, floorList[floorIndex].floorPoint.y - 1);
            var searchPathRight =  new Vector2Int(floorList[floorIndex].pathUp.x + x, floorList[floorIndex].floorPoint.y - 1);
            if (map.ContainsKey(searchPathLeft))
            {
                if (map[searchPathLeft].tileType == TileModel.TileType.Floor)
                {
                    for (var l = 1; l <= x; l++ )
                    {
                        SetTileModel(TileModel.TileType.Floor, new Vector2Int(floorList[floorIndex].pathUp.x - l, floorList[floorIndex].floorPoint.y), floorList[floorIndex].floorId); 
                    }
                    break;
                }
            }
            if (map.ContainsKey(searchPathRight))
            {
                if (map[searchPathRight].tileType == TileModel.TileType.Floor)
                {
                    for (var l = 1; l <= x; l++ )
                    {
                        SetTileModel(TileModel.TileType.Floor, new Vector2Int(floorList[floorIndex].pathUp.x + l, floorList[floorIndex].floorPoint.y), floorList[floorIndex].floorId); 
                    }
                    break;
                }
            }
        }

        for (var x = 1; x < maxMapX / 2; x++)
        {
            var searchPathLeft =  new Vector2Int(floorList[floorIndex].pathDown.x - x, floorList[floorIndex].floorPoint.y + floorList[floorIndex].floorSize.y + 1);
            var searchPathRight =  new Vector2Int(floorList[floorIndex].pathDown.x + x, floorList[floorIndex].floorPoint.y + floorList[floorIndex].floorSize.y + 1);
            if (map.ContainsKey(searchPathLeft))
            {
                if (map[searchPathLeft].tileType == TileModel.TileType.Floor)
                {
                    for (var l = 1; l <= x; l++ )
                    {
                        SetTileModel(TileModel.TileType.Floor, new Vector2Int(floorList[floorIndex].pathDown.x - l, floorList[floorIndex].floorPoint.y + floorList[floorIndex].floorSize.y), floorList[floorIndex].floorId); 
                    }
                    break;
                }
            }
            if (map.ContainsKey(searchPathRight))
            {
                if (map[searchPathRight].tileType == TileModel.TileType.Floor)
                {
                    for (var l = 1; l <= x; l++ )
                    {
                        SetTileModel(TileModel.TileType.Floor, new Vector2Int(floorList[floorIndex].pathDown.x + l, floorList[floorIndex].floorPoint.y + floorList[floorIndex].floorSize.y), floorList[floorIndex].floorId); 
                    }
                    break;
                }
            }
        }
                
        // 左右検索
        for (var y = 1 ; y < maxMapY / 2; y++)
        {
            var searchPathUp =  new Vector2Int(floorList[floorIndex].floorPoint.x - 1, floorList[floorIndex].pathLeft.y - y);
            var searchPathDown =  new Vector2Int(floorList[floorIndex].floorPoint.x - 1, floorList[floorIndex].pathLeft.y + y);
            if (map.ContainsKey(searchPathUp))
            {
                if (map[searchPathUp].tileType == TileModel.TileType.Floor)
                {
                    for (var l = 1; l <= y; l++ )
                    {
                        SetTileModel(TileModel.TileType.Floor, new Vector2Int(floorList[floorIndex].floorPoint.x, floorList[floorIndex].pathLeft.y - l), floorList[floorIndex].floorId); 
                    }
                    break;
                }
            }
            if (map.ContainsKey(searchPathDown))
            {
                if (map[searchPathDown].tileType == TileModel.TileType.Floor)
                {
                    for (var l = 1; l <= y; l++ )
                    {
                        SetTileModel(TileModel.TileType.Floor, new Vector2Int(floorList[floorIndex].floorPoint.x, floorList[floorIndex].pathLeft.y + l), floorList[floorIndex].floorId); 
                    }
                    break;
                }
            }
        }

        for (var y = 1 ; y < maxMapY / 2; y++)
        {
            var searchPathUp =  new Vector2Int(floorList[floorIndex].floorPoint.x + floorList[floorIndex].floorSize.x + 1, floorList[floorIndex].pathRight.y - y);
            var searchPathDown =  new Vector2Int(floorList[floorIndex].floorPoint.x + floorList[floorIndex].floorSize.x + 1, floorList[floorIndex].pathRight.y + y);
            if (map.ContainsKey(searchPathUp))
            {
                if (map[searchPathUp].tileType == TileModel.TileType.Floor)
                {
                    for (var l = 1; l <= y; l++ )
                    {
                        SetTileModel(TileModel.TileType.Floor, new Vector2Int(floorList[floorIndex].floorPoint.x + floorList[floorIndex].floorSize.x, floorList[floorIndex].pathRight.y - l), floorList[floorIndex].floorId); 
                    }
                    break;
                }
            }
            if (map.ContainsKey(searchPathDown))
            {
                if (map[searchPathDown].tileType == TileModel.TileType.Floor)
                {
                    for (var l = 1; l <= y; l++ )
                    {
                        SetTileModel(TileModel.TileType.Floor, new Vector2Int(floorList[floorIndex].floorPoint.x + floorList[floorIndex].floorSize.x, floorList[floorIndex].pathRight.y + l), floorList[floorIndex].floorId); 
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
            GameObject original = Object.Instantiate(Resources.Load("Object/Tile")) as GameObject;            
            original.transform.Translate(position.x, 0, position.y);
            mapListObject[position] = original;
        } else if (t.tileType == TileModel.TileType.Wall)
        {
            GameObject original = Object.Instantiate(Resources.Load("Object/Block")) as GameObject;            
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
    /// <param name="x"></param>
    /// <param name="z"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public bool IsCanMove(int x, int z, TileModel.CharaType type)
    {
        //移動先が0以上
        if (x > 0 && z > 0)
        {            
            var t = GetTileModel(new Vector2Int(x, z));
            if (type == TileModel.CharaType.Player)
            {
                if (
                    (t.tileType == TileModel.TileType.Floor || t.tileType == TileModel.TileType.Stairs) && 
                    t.charaType == TileModel.CharaType.None)
                {
                    return true;
                }
            }else if (type == TileModel.CharaType.Enemy)
            {
                if (t.tileType == TileModel.TileType.Floor && t.charaType == TileModel.CharaType.None)
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
    
    public void SetUserModel(int x, int z, UserModel user)
    {
        var t = GetTileModel(new Vector2Int(x, z));
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
}
