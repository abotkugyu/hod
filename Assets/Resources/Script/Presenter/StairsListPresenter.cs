using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//階段情報管理
public class StairsListPresenter : MonoBehaviour
{

    public List<GameObject> stairsList = new List<GameObject>();
    public int num = 1;

    public void Generate(MapPresenter mapPresenter)
    {
        for (int x = 0; x < num; x++)
        {
            GameObject obj = Object.Instantiate(Resources.Load("Object/Stairs")) as GameObject;
            Vector2Int pos = mapPresenter.GetPopPoint();

            obj.transform.position = new Vector3(pos.x, 0, pos.y);
            obj.layer = 9;

            stairsList.Add(obj);
            //mapに配置
            mapPresenter.GetTileModel(pos.x, pos.y).tileType = TileModel.TileType.Stairs;
        }
    }

    /// <summary>
    /// 階段場所直接指定
    /// </summary>
    /// <param name="mapPresenter"></param>
    /// <param name="pos"></param>
    public void DummyGenerate(MapPresenter mapPresenter, Vector2Int pos)
    {
        if (pos != null)
        {
            for (int x = 0; x < num; x++)
            {
                GameObject obj = Object.Instantiate(Resources.Load("Object/Stairs")) as GameObject;

                obj.transform.position = new Vector3(pos.x, 0, pos.y);
                obj.layer = 9;

                stairsList.Add(obj);
                //mapに配置
                mapPresenter.GetTileModel(pos.x, pos.y).tileType = TileModel.TileType.Stairs;
            }
        }
    }

	public void delete(int index)
	{
        Destroy(stairsList[index]);
        stairsList.RemoveAt(index);
        Debug.Log(stairsList.Count);
	}
}
