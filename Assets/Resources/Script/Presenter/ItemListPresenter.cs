using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//アイテム情報管理
public class ItemListPresenter : MonoBehaviour
{

    public List<GameObject> item_objects = new List<GameObject>();
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
            itemPresenter.status = ItemData.GetRandom();
            itemPresenter.status.guid = obj.GetInstanceID();
            itemPresenter.status.position = new Vector3(posx, 0, posz);            
            
            //mapに配置
            mapPresenter.map[posx, posz].item_type = 1;
            mapPresenter.map[posx, posz].item_guid = itemPresenter.status.guid;
            
            item_objects.Add(obj);
                        
        }
    }
    
    public ItemModel find(int guid)
    {
        var obj = item_objects.FirstOrDefault(i => i.GetInstanceID() == guid);
        if (obj != null)
        {
            return obj.GetComponent<ItemPresenter>().status;
        }
        return null;
    }
    
    public void delete(int guid)
    {
        // 遅かったらindex検索にする
        GameObject obj = item_objects.FirstOrDefault(i => i.GetInstanceID() == guid);
        
        if (obj != null)
        {
            Destroy(obj);
            Debug.Log(item_objects.Count);
        }
    }
}
