using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//敵情報管理
public class ItemController : MonoBehaviour
{

    public List<GameObject> item_list = new List<GameObject>();
    public int num = 100;

    public void generate(GameMap map)
    {
        for (int x = 0; x < num; x++)
        {
            GameObject obj = Object.Instantiate(Resources.Load("Object/Item")) as GameObject;
            List<int> pos = map.get_pop_point();
            int posx = pos[0];
            int posz = pos[1];
            obj.transform.position = new Vector3(posx, 0, posz);
            obj.layer = 9;

            Item item = obj.GetComponent<Item>();
            item.status.id = obj.GetInstanceID();
            item.status.type = 1;
            item.status.position = new Vector3(posx, 0, posz);
            item_list.Add(obj);
            //mapに配置
            map.map[posx, posz].item_type = item.status.type;
            map.map[posx, posz].chara_id = item.status.id;
        }
    }

    public void delete(int x, int y)
    {
        
    }

}
