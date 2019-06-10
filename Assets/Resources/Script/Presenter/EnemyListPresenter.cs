using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//敵情報管理
public class EnemyListPresenter : MonoBehaviour
{
    public Dictionary<int, GameObject> enemyListObject = new Dictionary<int, GameObject>();
    public Dictionary<int, EnemyPresenter> enemyListPresenter = new Dictionary<int, EnemyPresenter>();
    public int guid = 1;
    public int num = 1;

    public void Generate(MapPresenter mapPresenter)
    {
        GameObject res = Resources.Load("Object/Enemy") as GameObject;
        for (int x = 0; x < num; x++)
        {
            List<int> pos = mapPresenter.GetPopPoint();
            int posX = pos[0];
            int posZ = pos[1];
            GameObject obj = Object.Instantiate(res, new Vector3(posX, 0, posZ), Quaternion.identity) as GameObject;
            obj.layer = 9;

            EnemyPresenter enemyPresenter = obj.GetComponent<EnemyPresenter>();
            enemyPresenter.Initialize(EnemyData.GetRandom(), guid);

            enemyPresenter.SetMapData(
                mapPresenter.map[posX, posZ].floorId,
                new Vector3(posX, 0, posZ),
                new Vector3(0, 0, -1)
                );
            
            enemyListPresenter[guid] = enemyPresenter;
            enemyListObject[guid] = obj;
            guid++;

            //mapに配置
            mapPresenter.SetUserModel(posX, posZ, enemyPresenter.status);
    
            Debug.Log(enemyPresenter.status.position.x + ":"+ enemyPresenter.status.position.z + "," + enemyPresenter.enemyView.trans.position.x + ":" + enemyPresenter.enemyView.trans.position.z);
        }
        Debug.Log("***********");
    }
    
    /// <summary>
    /// 階段場所直接指定
    /// </summary>
    /// <param name="mapPresenter"></param>
    /// <param name="pos"></param>
    public void DummyGenerate(MapPresenter mapPresenter, List<int> pos)
    {
        if (pos != null)
        {
            GameObject res = Resources.Load("Object/Enemy") as GameObject;
            for (int x = 0; x < 1; x++)
            {
                int posX = pos[0];
                int posZ = pos[1];
                GameObject obj = Object.Instantiate(res, new Vector3(posX, 0, posZ), Quaternion.identity) as GameObject;
                obj.layer = 9;

                EnemyPresenter enemyPresenter = obj.GetComponent<EnemyPresenter>();
                enemyPresenter.Initialize(EnemyData.GetRandom(), guid);
                
                enemyPresenter.SetMapData(
                    mapPresenter.map[posX, posZ].floorId,
                    new Vector3(posX, 0, posZ),
                    new Vector3(0, 0, -1)
                );
                                
                enemyListPresenter[guid] = enemyPresenter;
                enemyListObject[guid] = obj;
                guid++;
                
                //mapに配置
                mapPresenter.SetUserModel(pos[0], pos[1], enemyPresenter.status);
            }
        }
    }

    //敵が全部動いたかどうか
    public bool IsAllAction()
    {
        bool is_end = true;
        foreach (KeyValuePair<int, EnemyPresenter> enemy in enemyListPresenter)
        {
            EnemyPresenter enemyPresenter = enemy.Value;
            if (enemyPresenter.status.isAction == false && enemyPresenter.isMove == true)
            {
                is_end = !is_end;
                break;
            }
        }

        return is_end;
    }

    //全ての敵に行動させる
    public void AllAction(MapPresenter mapPresenter)
    {
        //Dictionary<int, EnemyPresenter> plauerInSideFloorEnemy = enemyListPresenter.Select(enemy =>
        //    enemy.Value.status.floorId == 0).ToDictionary(enemy => );
        
        Dictionary<int, EnemyPresenter> plauerInSideFloorEnemy = new Dictionary<int, EnemyPresenter>();
        Dictionary<int, EnemyPresenter> plauerOutSideFloorEnemy  = new Dictionary<int, EnemyPresenter>();

        foreach (KeyValuePair<int, EnemyPresenter> enemy in enemyListPresenter)
        {
            EnemyPresenter enemyPresenter = enemy.Value;
            int actionType = enemyPresenter.GetAction();
            //とりあえず1を移動
            if (actionType == 1)
            {
                
                float x = Mathf.Sign(Random.Range(-1.0f, 1.0f)) * 200;
                float z = Mathf.Sign(Random.Range(-1.0f, 1.0f)) * 200;
                int nX = (x != 0 ? (int)Mathf.Sign(x) : 0);
                int nZ = (z != 0 ? (int)Mathf.Sign(z) : 0);
                
                int beforePositionX = (int) enemyPresenter.status.position.x;
                int beforePositionZ = (int) enemyPresenter.status.position.z;
                int afterPositionX = beforePositionX + nX;
                int afterPositionZ = beforePositionZ + nZ;
                if (mapPresenter.IsCanMove(afterPositionX, afterPositionZ, TileModel.CharaType.Enemy))
                {
                    
                    Debug.Log(mapPresenter.map[afterPositionX, afterPositionZ].tileType);
                    Debug.Log(enemyPresenter.status.position.x + ":"+ enemyPresenter.status.position.z + "," + enemyPresenter.enemyView.trans.position.x + ":" + enemyPresenter.enemyView.trans.position.z);
                    
                    enemyPresenter.Move(x, z);
                    
                    //移動元と移動先にキャラクター情報を設定
                    mapPresenter.SetUserModel(beforePositionX, beforePositionZ, null);
                    mapPresenter.SetUserModel(afterPositionX, afterPositionZ, enemyPresenter.status);
                    
                    //StartMoveしてからSetPositionをする。
                    
                    enemyPresenter.SetMapData(
                        mapPresenter.map[afterPositionX, afterPositionZ].floorId,
                        new Vector3(afterPositionX, 0, afterPositionZ),
                        new Vector3(nX, 0, nZ)
                    );
                }else{
                    //移動先がなければ行動済みにする。
                    enemyPresenter.SetIsAction(true);
                }
            }else{
                enemyPresenter.SetIsAction(true);
            }
        }
    }

    //敵の行動フラグをリセットする
    public void TurnReset()
    {    
        foreach (KeyValuePair<int, EnemyPresenter> enemy in enemyListPresenter)
        {
            enemy.Value.SetIsAction(false);
        }
    }
    
	public void Delete(int guid)
	{
        EnemyPresenter enemyPresenter = enemyListPresenter.FirstOrDefault(enemy =>
            enemy.Key == guid).Value;
        if (enemyPresenter != null)
        {
            Debug.Log("Attack:" + enemyPresenter.status.guid);
            enemyPresenter.status.hp = 0;
            Destroy(enemyListObject[guid]);
            enemyListPresenter.Remove(guid);
        }
        Debug.Log(enemyListPresenter.Count);
	}
	
    public void ShowLog()
    {

        foreach (KeyValuePair<int, EnemyPresenter> enemy in enemyListPresenter)
        {
            Debug.Log(enemy.Key);
            EnemyPresenter enemyPresenter = enemy.Value;
            Debug.Log(enemyPresenter.status.position.x + ":"+ enemyPresenter.status.position.z + "," + enemyPresenter.enemyView.trans.position.x + ":" + enemyPresenter.enemyView.trans.position.z);
        }
    }

    
}
