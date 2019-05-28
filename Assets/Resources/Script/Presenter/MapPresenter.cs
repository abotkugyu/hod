using System.Collections;
using System.Collections.Generic;
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

        List<int> map_x = GenerateMap ();
        List<List<int>> map_y = new List<List<int>>();
        int count_x = 0;
        for (int x = 0; x < map_x.Count; x++)
        {
            List<int> list_y = GenerateMap ();
            map_y.Add(list_y);
            int count_y = 0;

            for (int y = 0; y < list_y.Count; y++){
                //int[,] floor = 
                GenerateFloor(map_x[x],list_y[y],count_x,count_y);
                count_y += list_y[y];
            }
            count_x += map_x[x];
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

    int[,] GenerateFloor(int x,int y,int seq_x,int seq_y)
    {
        //maxだと部屋同士でくっつくので-1
        int[,] result = new int[x, y];
        int floor_x = Random.Range(5, x-2);
        int floor_y = Random.Range(5, y-2);
        int start_x = Random.Range(1, x-floor_x-1);
        int start_y = Random.Range(1, y-floor_y-1);
        for (int l = 0; l < x; l++)
        {
            for (int m = 0; m < y; m++)
            {
                if (l >= start_x && l <= start_x + floor_x && m >= start_y && m <= start_y + floor_y)
                {
                    GameObject original = Object.Instantiate(Resources.Load("Object/Tile")) as GameObject;
                    original.transform.Translate(seq_x + l, 0, seq_y + m);
                    result[l, m] = 1;
                    map[l + seq_x, m + seq_y].tileType = TileModel.TileType.Floor;
                    popPoint.Add((l + seq_x)+","+(m + seq_y));
                }else{
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
		Random.Range (0, 2);
		//show_list_log (data);
		for (int x = 0; x < data.Count; x++) {
			int is_split = Random.Range (0, 10);
			if (data [x] < 20) {
				continue;
			}
			if (is_split <= 8) {
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
    /// 移動できるかどうかs
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
}
