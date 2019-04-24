using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//敵情報管理
public class ItemController : MonoBehaviour
{

    public List<GameObject> item_objects = new List<GameObject>();
    public List<Item> item_list = new List<Item>();
    public int num = 100;

    public void generate(GameMap map)
    {
        for (int x = 0; x < num; x++)
        {
            // item object作成
            GameObject obj = Object.Instantiate(Resources.Load("Object/Item")) as GameObject;
            List<int> pos = map.get_pop_point();
            int posx = pos[0];
            int posz = pos[1];
            obj.transform.position = new Vector3(posx, 0, posz);
            obj.layer = 9;

            // item model作成
            Item item = obj.GetComponent<Item>();
            item.status.id = obj.GetInstanceID();
            item.status.type = 1;
            item.status.position = new Vector3(posx, 0, posz);
            
            item_list.Add(item);
            item_objects.Add(obj);
            
            //mapに配置
            map.map[posx, posz].item_type = item.status.type;
            map.map[posx, posz].item_id = item.status.id;
        }
    }

    public void delete(int id)
    {
        // 遅かったらindex検索にする
        GameObject item = item_objects.FirstOrDefault(i => i.GetInstanceID() == id);
        
        if (item != null)
        {
            Destroy(item);
            var x = item_list.Select((i, index) => new {i, index}).First(i => i.i.status.id == id).index;
            item_list.RemoveAt(x);
            Debug.Log(item_list.Count);
        }
    }

}
