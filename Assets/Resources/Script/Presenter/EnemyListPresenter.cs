using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//敵情報管理
public class EnemyListPresenter : MonoBehaviour
{

    public List<EnemyPresenter> enemyList = new List<EnemyPresenter>();
    public int num = 100;

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
            enemyPresenter.status = EnemyData.GetRandom();
            enemyPresenter.status.id = obj.GetInstanceID();
            enemyPresenter.status.type = TileModel.CharaType.Enemy;
            enemyPresenter.status.isAction = false;
            enemyPresenter.status.position = new Vector3(posX, 0, posZ);
            enemyList.Add(enemyPresenter);
            //mapに配置
            mapPresenter.map[posX, posZ].charaId = enemyPresenter.status.id;
            mapPresenter.map[posX, posZ].charaType = enemyPresenter.status.type;
        }
    }

    //敵が全部動いたかどうか
    public bool IsAllAction()
    {
        bool is_end = true;
        for (int x = 0; x < enemyList.Count; x++)
        {
            EnemyPresenter enemyPresenter = enemyList[x];
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
        for (int l = 0; l < enemyList.Count; l++)
        {
            EnemyPresenter enemyPresenter = enemyList[l];
            int action_type = enemyPresenter.GetAction();
            //とりあえず1を移動
            if (action_type == 1)
            {
                float x = Mathf.Sign(Random.Range(-1.0f, 1.0f)) * 200;
                float z = Mathf.Sign(Random.Range(-1.0f, 1.0f)) * 200;
                int n_x = (x != 0 ? (int)Mathf.Sign(x) : 0);
                int n_z = (z != 0 ? (int)Mathf.Sign(z) : 0);
                int afterPositionX = (int) enemyPresenter.status.position.x + n_x;
                int afterPositionZ = (int) enemyPresenter.status.position.z + n_z;
                if (mapPresenter.IsCanMove(afterPositionX, afterPositionZ, TileModel.CharaType.Enemy))
                {
                    mapPresenter.map[(int)enemyPresenter.status.position.x, (int)enemyPresenter.status.position.z].charaType = 0;
                    mapPresenter.map[afterPositionX, afterPositionZ].charaType = enemyPresenter.status.type;

                    mapPresenter.map[(int)enemyPresenter.status.position.x, (int)enemyPresenter.status.position.z].charaId = 0;
                    mapPresenter.map[afterPositionX, afterPositionZ].charaId = enemyPresenter.status.id;

                    enemyPresenter.SetPosition(new Vector3(n_x, 0, n_z));
                    enemyPresenter.SetDirection(new Vector3(n_x, 0, n_z));

                    enemyPresenter.StartMove(x, z);
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
    public void TurnReset(){
        for (int x = 0; x < enemyList.Count; x++)
        {
            EnemyPresenter enemyPresenter = enemyList[x];
            enemyPresenter.SetIsAction(false);
        }
    }
    
	public void Delete(int index)
	{
        Destroy(enemyList[index]);
        enemyList.RemoveAt(index);
        Debug.Log(enemyList.Count);
	}
}
