using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//アイテム情報管理
public class ItemListPresenter : MonoBehaviour
{

    public List<GameObject> item_objects = new List<GameObject>();
    public List<ItemPresenter> item_list = new List<ItemPresenter>();
    public int num = 100;

    public void generate(MapPresenter mapPresenter)
    {
        for (int x = 0; x < num; x++)
        {
            // item object作成
            GameObject obj = Object.Instantiate(Resources.Load("Object/Item")) as GameObject;
            List<int> pos = mapPresenter.get_pop_point();
            int posx = pos[0];
            int posz = pos[1];
            obj.transform.position = new Vector3(posx, 0, posz);
            obj.layer = 9;

            // item model作成
            ItemPresenter itemPresenter = obj.GetComponent<ItemPresenter>();
            itemPresenter.status.id = obj.GetInstanceID();
            itemPresenter.status.type = 1;
            itemPresenter.status.position = new Vector3(posx, 0, posz);
            
            item_list.Add(itemPresenter);
            item_objects.Add(obj);
            
            //mapに配置
            mapPresenter.map[posx, posz].item_type = itemPresenter.status.type;
            mapPresenter.map[posx, posz].item_id = itemPresenter.status.id;
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
