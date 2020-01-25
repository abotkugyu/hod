using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//アイテム情報管理
public class ItemListPresenter : MonoBehaviour
{

    public Dictionary<int, GameObject> itemListObject = new Dictionary<int, GameObject>();
    public Dictionary<int, ItemPresenter> itemListPresenter = new Dictionary<int, ItemPresenter>();
    public int serialGuid = 1;

    public void Generate(MapPresenter mapPresenter, ItemModel model)
    { 
        // item object作成
        GameObject res = Resources.Load("Object/"+model.modelName.Replace('_','/')) as GameObject;
        Vector2Int pos = mapPresenter.GetPopPoint();
        GameObject obj = Object.Instantiate(res, new Vector3(pos.x, 0, pos.y), Quaternion.identity) as GameObject;
        obj.name = "Item;" + serialGuid;
        obj.layer = 9;
        obj.transform.parent = transform;
        
        // item model作成
        ItemPresenter itemPresenter = obj.GetComponent<ItemPresenter>();      
        itemPresenter.Initialize(model, serialGuid);
        itemPresenter.SetMapData(
            mapPresenter.GetTileModel(pos.x, pos.y).floorId,
            new Vector3(pos.x, 0, pos.y),
            new Vector3(0, 0, -1)
        );     
                    
        itemListPresenter[serialGuid] = itemPresenter;
        itemListObject[serialGuid] = obj;
        serialGuid++;
        
        mapPresenter.SetItemModel(pos.x, pos.y, itemPresenter.status);                        
    }
    
    public ItemPresenter Find(int guid)
    {
        return itemListPresenter.TryGetValue(guid, out var val) ? val : null;
    }
        
    public void Delete(ItemPresenter itemPresenter)
    {
        int guid = itemPresenter.status.guid;
        if (itemPresenter != null)
        {
            Destroy(itemListObject[guid]);
            itemListObject.Remove(guid);
            itemListPresenter.Remove(guid);
        }
        else
        {
            Debug.Log("Cant Delete:"+guid);            
        }
        Debug.Log(itemListPresenter.Count);
    }
}
