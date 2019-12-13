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
        GameObject res = Resources.Load("Object/"+model.modelName) as GameObject;
        List<int> pos = mapPresenter.GetPopPoint();
        int posX = pos[0];
        int posZ = pos[1];
        GameObject obj = Object.Instantiate(res, new Vector3(posX, 0, posZ), Quaternion.identity) as GameObject;
        obj.layer = 9;
        
        // item model作成
        ItemPresenter itemPresenter = obj.GetComponent<ItemPresenter>();      
        itemPresenter.Initialize(model, serialGuid);
        itemPresenter.SetMapData(
            mapPresenter.map[posX, posZ].floorId,
            new Vector3(posX, 0, posZ),
            new Vector3(0, 0, -1)
        );     
                    
        itemListPresenter[serialGuid] = itemPresenter;
        itemListObject[serialGuid] = obj;
        serialGuid++;
        
        mapPresenter.SetItemModel(posX, posZ, itemPresenter.status);                        
        
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
            itemListPresenter.Remove(guid);
        }
        Debug.Log(itemListPresenter.Count);
    }
}
