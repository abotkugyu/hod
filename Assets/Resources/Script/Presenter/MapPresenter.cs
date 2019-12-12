using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;


//100*100を50,25...と分割していき確率で部屋を作る
public class MapPresenter : MonoBehaviour {

	public int maxMapX = 100;
    public int maxMapY = 100;
    public TileModel[,] map;
    public List<string> popPoint = new List<string>();

    public void Generate() {
        map = new TileModel[maxMapX, maxMapY];
        for (int x = 0; x < maxMapX; x++)
        {
            for (int z = 0; z < maxMapY; z++)
            {
                map[x, z] = new TileModel();
            }
        }

        //横軸をGenerateMapで分割
        List<int> mapX = GenerateMap ();
        List<List<int>> mapY = new List<List<int>>();
        int countX = 0;
        int floorId = 1;
        for (int x = 0; x < mapX.Count; x++)
        {
            
            //縦軸をGenerateMapで分割
            List<int> listY = GenerateMap ();
            mapY.Add(listY);
            int countY = 0;
            for (int y = 0; y < listY.Count; y++){
                //int[,] floor = 
                GenerateFloor(mapX[x],listY[y], countX, countY, floorId);
                countY += listY[y];
                floorId++;
            }
            countX += mapX[x];
            //map_y[x];
            //map_list[x,0] = 1;
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

    int[,] GenerateFloor(int x, int y, int seqX, int seqY, int floorId)
    {
        //maxだと部屋同士でくっつくので-1
        int[,] result = new int[x, y];
        int floorX = Random.Range(5, x-2);
        int floorY = Random.Range(5, y-2);
        int startX = Random.Range(1, x-floorX-1);
        int startY = Random.Range(1, y-floorY-1);
        for (int l = 0; l < x; l++)
        {
            for (int m = 0; m < y; m++)
            {
                if (l >= startX && l <= startX + floorX && m >= startY && m <= startY + floorY)
                {
                    //床
                    GameObject original = Object.Instantiate(Resources.Load("Object/Tile")) as GameObject;
                    original.transform.Translate(seqX + l, 0, seqY + m);
                    map[l + seqX, m + seqY].tileType = TileModel.TileType.Floor;
                    map[l + seqX, m + seqY].floorId = floorId;
                    popPoint.Add((l + seqX)+","+(m + seqY));
                    result[l, m] = 1;
                }else{
                    //壁                    
                    GameObject original = Object.Instantiate(Resources.Load("Object/Block")) as GameObject;
                    original.transform.Translate(seqX + l, 0, seqY + m);
                    map[l + seqX, m + seqY].tileType = TileModel.TileType.Wall;
                    map[l + seqX, m + seqY].floorId = floorId;
                    result[l, m] = 0;
                }
            }
        }
        return result;
    }

    List<int> GenerateMap(){
        return SplitMap(new List<int>{maxMapX});
	}

    List<int> SplitMap(List<int> data){
		//Random.Range (0, 2);
		//show_list_log (data);
		
		//全体のサイズを繰り返し分割していく
		//100→50,50→25,25,50
		for (int x = 0; x < data.Count; x++) {
			if (data [x] < 20) {
				continue;
			}
            int isSplit = Random.Range (0, 10);
			if (isSplit <= 8) {
				int ins = data [x] / 2;
				data [x] = data [x] / 2;
				data.Insert (x, ins);
				x--;
				continue;
			} 
		}
		//show_list_log (data);
        return data;
	}

	void ShowListLog(List<int> data){
		string log = "";
		for (int x = 0; x < data.Count; x++) {
			log += data[x].ToString ()+",";
		}
	}

    public List<int> GetPopPoint()
    {
        for (int l = 0; l < 1000; l++){
            int pos_s = Random.Range(0, popPoint.Count - 1);
            string[] pos = popPoint[pos_s].Split(',');
            int x = int.Parse(pos[0]);
            int z = int.Parse(pos[1]);
			if (map[x,z].tileType == TileModel.TileType.Floor && map[x,z].charaType == TileModel.CharaType.None)
            {
                return new List<int>{x, z};
            }
        }
        //全部map埋まってたりしたら諦める。
        return new List<int> { 0, 0 };
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
            if (type == TileModel.CharaType.Player)
            {
                if ((map[x, z].tileType == TileModel.TileType.Floor || map[x, z].tileType == TileModel.TileType.Stairs) && map[x, z].charaType == TileModel.CharaType.None)
                {
                    return true;
                }
            }else if (type == TileModel.CharaType.Enemy)
            {
                if (map[x, z].tileType == TileModel.TileType.Floor && map[x, z].charaType == TileModel.CharaType.None)
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
    public List<int> CanSetObject(List<int> pos)
    {
        List<List<int>> directions = new List<List<int>>
        {
            new List<int> {-1, -1},
            new List<int> {-1, 0},
            new List<int> {-1, 1},
            new List<int> {0, -1},
            new List<int> {0, 0},
            new List<int> {0, 1},
            new List<int> {1, -1},
            new List<int> {1, 0},
            new List<int> {1, 1},
        };

        foreach (List<int> direction in directions)
        {
            List<int> resPosition = new List<int>();
            resPosition.Add(pos[0] + direction[0]);
            resPosition.Add(pos[1] + direction[1]);
            if (map[resPosition[0], resPosition[1]].tileType == TileModel.TileType.Floor && map[resPosition[0], resPosition[1]].charaType == 0)
            {
                return resPosition;
            }
        }

        return null;
    }
    
    public void SetUserModel(int x, int z, UserModel user)
    {
        if (user != null)
        {
            map[x, z].guid = user.guid;
            map[x, z].charaId = user.id;
            map[x, z].charaType = user.type;
        }
        else
        {            
            map[x, z].guid = 0;
            map[x, z].charaId = 0;
            map[x, z].charaType = TileModel.CharaType.None;
        }
    }
    
    public void SetItemModel(int x, int z, ItemModel item)
    {
        if (item != null)
        {
            map[x, z].guid = item.guid;
            map[x, z].itemId = item.id;
        }
        else
        {            
            map[x, z].guid = 0;
            map[x, z].itemId = 0;
        }
    }

    public void ShowMapInfo()
    {
        for (int z = maxMapY-1; z >= 0; z--)
        {
            string m = "";
            for (int x = 0; x < maxMapX; x++)
            {
                string m1 = map[x, z].tileType.ToString().Substring(0, 1);
                string m2 = map[x, z].charaType.ToString().Substring(0, 1);
                string m3 = map[x, z].floorId.ToString();
                m += m1+m2+m3+",";
            }
            Debug.Log(m);
        }
    }
}
