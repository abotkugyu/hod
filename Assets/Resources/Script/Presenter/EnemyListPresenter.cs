using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//敵情報管理
public class EnemyListPresenter : MonoBehaviour
{
    public Dictionary<int, GameObject> enemyListObject = new Dictionary<int, GameObject>();
    public Dictionary<int, EnemyPresenter> enemyListPresenter = new Dictionary<int, EnemyPresenter>();
    public int uniqueKey = 1;
    public int num = 1;

    public void Generate(MapPresenter mapPresenter)
    {
        for (int x = 0; x < num; x++)
        {
            GameObject obj = Object.Instantiate(Resources.Load("Object/Enemy")) as GameObject;
            List<int> pos = mapPresenter.GetPopPoint();
            int posX = pos[0];
            int posZ = pos[1];
            obj.transform.position = new Vector3(posX, 0, posZ);
            obj.layer = 9;

            EnemyPresenter enemyPresenter = obj.GetComponent<EnemyPresenter>();
            enemyPresenter.Initialize(EnemyData.GetRandom(), pos, uniqueKey);
            
            enemyListPresenter[uniqueKey] = enemyPresenter;
            enemyListObject[uniqueKey] = obj;
            uniqueKey++;

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
            for (int x = 0; x < 1; x++)
            {
                GameObject obj = Object.Instantiate(Resources.Load("Object/Enemy")) as GameObject;
                obj.transform.position = new Vector3(pos[0], 0, pos[1]);
                obj.layer = 9;

                EnemyPresenter enemyPresenter = obj.GetComponent<EnemyPresenter>();
                enemyPresenter.Initialize(EnemyData.GetRandom(), pos, uniqueKey);
                                
                enemyListPresenter[uniqueKey] = enemyPresenter;
                enemyListObject[uniqueKey] = obj;
                uniqueKey++;
                
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
        foreach (KeyValuePair<int, EnemyPresenter> enemy in enemyListPresenter)
        {
            EnemyPresenter enemyPresenter = enemy.Value;
            int action_type = enemyPresenter.GetAction();
            //とりあえず1を移動
            if (action_type == 1)
            {
                float x = Mathf.Sign(Random.Range(-1.0f, 1.0f)) * 200;
                float z = Mathf.Sign(Random.Range(-1.0f, 1.0f)) * 200;
                int n_x = (x != 0 ? (int)Mathf.Sign(x) : 0);
                int n_z = (z != 0 ? (int)Mathf.Sign(z) : 0);
                
                int beforePositionX = (int) enemyPresenter.status.position.x;
                int beforePositionZ = (int) enemyPresenter.status.position.z;
                int afterPositionX = beforePositionX + n_x;
                int afterPositionZ = beforePositionZ + n_z;
                if (mapPresenter.IsCanMove(afterPositionX, afterPositionZ, TileModel.CharaType.Enemy))
                {
                    
                    Debug.Log(mapPresenter.map[afterPositionX, afterPositionZ].tileType);
                    Debug.Log(enemyPresenter.status.position.x + ":"+ enemyPresenter.status.position.z + "," + enemyPresenter.enemyView.trans.position.x + ":" + enemyPresenter.enemyView.trans.position.z);
                    
                    enemyPresenter.Move(x, z);
                    
                    //移動元と移動先にキャラクター情報を設定
                    mapPresenter.SetUserModel(beforePositionX, beforePositionZ, null);
                    mapPresenter.SetUserModel(afterPositionX, afterPositionZ, enemyPresenter.status);
                    
                    //StartMoveしてからSetPositionをする。
                    enemyPresenter.SetPosition(new Vector3(afterPositionX, 0, afterPositionZ));
                    enemyPresenter.SetDirection(new Vector3(n_x, 0, n_z));

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
    
	public void Delete(int id)
	{
        EnemyPresenter enemyPresenter = enemyListPresenter.FirstOrDefault(enemy =>
            enemy.Key == id).Value;
        if (enemyPresenter != null)
        {
            Debug.Log("Attack:" + enemyPresenter.status.id);
            enemyPresenter.status.hp = 0;
            Destroy(enemyListObject[id]);
            enemyListPresenter.Remove(id);
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
