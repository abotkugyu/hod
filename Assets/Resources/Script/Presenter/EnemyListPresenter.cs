using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//敵情報管理
public class EnemyListPresenter : MonoBehaviour
{

    public List<GameObject> enemy_list = new List<GameObject>();
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
            enemyPresenter.status.position = new Vector3(posx, 0, posz);
            enemy_list.Add(obj);
            //mapに配置
            mapPresenter.map[posx, posz].charaType = enemyPresenter.status.type;
            mapPresenter.map[posx, posz].charaId = enemyPresenter.status.id;
        }
    }

    //敵が全部動いたかどうか
    public bool is_all_action()
    {
        bool is_end = true;
        for (int x = 0; x < enemy_list.Count; x++)
        {
            EnemyPresenter com = enemy_list[x].GetComponent<EnemyPresenter>();
            if (com.status.is_action == false)
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
            EnemyPresenter com = enemy_list[l].GetComponent<EnemyPresenter>();
            int action_type = com.get_action();
            //とりあえず1を移動
            if (action_type == 1)
            {
                float x = Mathf.Sign(Random.Range(-1.0f, 1.0f)) * 200;
                float z = Mathf.Sign(Random.Range(-1.0f, 1.0f)) * 200;
                int n_x = (x != 0 ? (int)Mathf.Sign(x) : 0);
                int n_z = (z != 0 ? (int)Mathf.Sign(z) : 0);
                if ((int)com.status.position.x + n_x >= 0 && (int)com.status.position.z + n_z >= 0 &&
                    mapPresenter.map[(int)com.status.position.x + n_x, (int)com.status.position.z + n_z].charaType == 0 &&
					mapPresenter.map[(int)com.status.position.x + n_x, (int)com.status.position.z + n_z].tileType == TileModel.TileType.Floor)
                {
                    mapPresenter.map[(int)com.status.position.x, (int)com.status.position.z].charaType = 0;
                    mapPresenter.map[(int)com.status.position.x + n_x, (int)com.status.position.z + n_z].charaType = com.status.type;

                    mapPresenter.map[(int)com.status.position.x, (int)com.status.position.z].charaId = 0;
                    mapPresenter.map[(int)com.status.position.x + n_x, (int)com.status.position.z + n_z].charaId = com.status.id;

                    com.set_position(new Vector3(n_x, 0, n_z));
                    com.set_direction(new Vector3(n_x, 0, n_z));

                    com.is_move = true;
                    com.move(x, z);
                }else{
                    Debug.Log("can't enemy move");
                    com.status.is_action = true;
                }
            }
        }
    }

    //敵の行動フラグをリセットする
    public void turn_reset(){
        for (int x = 0; x < enemy_list.Count; x++)
        {
            EnemyPresenter com = enemy_list[x].GetComponent<EnemyPresenter>();
            com.status.is_action = false;
        }
    }

	public void delete(int index)
	{
        Destroy(enemy_list[index]);
        enemy_list.RemoveAt(index);
        Debug.Log(enemy_list.Count);
	}
}
