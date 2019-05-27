using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//敵情報管理
public class EnemyListPresenter : MonoBehaviour
{

    public List<EnemyPresenter> enemy_list = new List<EnemyPresenter>();
    public int num = 100;

    public void generate(MapPresenter mapPresenter)
    {
        for (int x = 0; x < num; x++)
        {
            GameObject obj = Object.Instantiate(Resources.Load("Object/Enemy")) as GameObject;
            List<int> pos = mapPresenter.get_pop_point();
            int posx = pos[0];
            int posz = pos[1];
            //Debug.Log(posx);
            //Debug.Log(posz);
            obj.transform.position = new Vector3(posx, 0, posz);
            obj.layer = 9;

            EnemyPresenter enemyPresenter = obj.GetComponent<EnemyPresenter>();                    
            enemyPresenter.status = EnemyData.GetRandom();
            enemyPresenter.status.id = obj.GetInstanceID();
            enemyPresenter.status.type = 2;
            enemyPresenter.status.is_action = false;
            enemyPresenter.status.position = new Vector3(posx, 0, posz);
            enemy_list.Add(enemyPresenter);
            //mapに配置
            mapPresenter.map[posx, posz].charaId = enemyPresenter.status.id;
            mapPresenter.map[posx, posz].charaType = enemyPresenter.status.type;
        }
    }

    //敵が全部動いたかどうか
    public bool is_all_action()
    {
        bool is_end = true;
        for (int x = 0; x < enemy_list.Count; x++)
        {
            EnemyPresenter enemyPresenter = enemy_list[x];
            if (enemyPresenter.status.is_action == false && enemyPresenter.is_move == true)
            {
                is_end = !is_end;
                break;
            }
        }

        return is_end;
    }

    //全ての敵に行動させる
    public void all_action(MapPresenter mapPresenter)
    {
        for (int l = 0; l < enemy_list.Count; l++)
        {
            EnemyPresenter enemyPresenter = enemy_list[l];
            int action_type = enemyPresenter.get_action();
            //とりあえず1を移動
            if (action_type == 1)
            {
                float x = Mathf.Sign(Random.Range(-1.0f, 1.0f)) * 200;
                float z = Mathf.Sign(Random.Range(-1.0f, 1.0f)) * 200;
                int n_x = (x != 0 ? (int)Mathf.Sign(x) : 0);
                int n_z = (z != 0 ? (int)Mathf.Sign(z) : 0);
                int afterPositionX = (int) enemyPresenter.status.position.x + n_x;
                int afterPositionZ = (int) enemyPresenter.status.position.z + n_z;
                if (afterPositionX >= 0 && afterPositionZ >= 0 &&
                    mapPresenter.map[afterPositionX, afterPositionZ].charaType == 0 &&
					mapPresenter.map[afterPositionX, afterPositionZ].tileType == TileModel.TileType.Floor)
                {
                    mapPresenter.map[(int)enemyPresenter.status.position.x, (int)enemyPresenter.status.position.z].charaType = 0;
                    mapPresenter.map[afterPositionX, afterPositionZ].charaType = enemyPresenter.status.type;

                    mapPresenter.map[(int)enemyPresenter.status.position.x, (int)enemyPresenter.status.position.z].charaId = 0;
                    mapPresenter.map[afterPositionX, afterPositionZ].charaId = enemyPresenter.status.id;

                    enemyPresenter.set_position(new Vector3(n_x, 0, n_z));
                    enemyPresenter.set_direction(new Vector3(n_x, 0, n_z));

                    enemyPresenter.is_move = true;
                    enemyPresenter.move(x, z);
                }else{
                    enemyPresenter.status.is_action = true;
                }
            }else{
                enemyPresenter.status.is_action = true;
            }
        }
    }

    //敵の行動フラグをリセットする
    public void turn_reset(){
        for (int x = 0; x < enemy_list.Count; x++)
        {
            EnemyPresenter enemyPresenter = enemy_list[x];
            enemyPresenter.status.is_action = false;
        }
    }
    
	public void delete(int index)
	{
        Destroy(enemy_list[index]);
        enemy_list.RemoveAt(index);
        Debug.Log(enemy_list.Count);
	}
}
