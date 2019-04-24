using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//敵情報管理
public class EnemyController : MonoBehaviour
{

    public List<GameObject> enemy_list = new List<GameObject>();
    public int num = 100;

    public void generate(GameMap map)
    {
        for (int x = 0; x < num; x++)
        {
            GameObject obj = Object.Instantiate(Resources.Load("Object/Enemy")) as GameObject;
            List<int> pos = map.get_pop_point();
            int posx = pos[0];
            int posz = pos[1];
            //Debug.Log(posx);
            //Debug.Log(posz);
            obj.transform.position = new Vector3(posx, 0, posz);
            obj.layer = 9;

            Enemy com = obj.GetComponent<Enemy>();
            com.status.id = obj.GetInstanceID();
            com.status.type = 2;
            com.status.position = new Vector3(posx, 0, posz);
            enemy_list.Add(obj);
            //mapに配置
            map.map[posx, posz].chara_type = com.status.type;
            map.map[posx, posz].chara_id = com.status.id;
        }
    }

    //敵が全部動いたかどうか
    public bool is_all_action()
    {
        bool is_end = true;
        for (int x = 0; x < enemy_list.Count; x++)
        {
            Enemy com = enemy_list[x].GetComponent<Enemy>();
            if (com.status.is_action == false)
            {
                is_end = !is_end;
                break;
            }
        }

        return is_end;
    }

    //全ての敵に行動させる
    public void all_action(GameMap map)
    {
        for (int l = 0; l < enemy_list.Count; l++)
        {
            Enemy com = enemy_list[l].GetComponent<Enemy>();
            int action_type = com.get_action();
            //とりあえず1を移動
            if (action_type == 1)
            {
                float x = Mathf.Sign(Random.Range(-1.0f, 1.0f)) * 200;
                float z = Mathf.Sign(Random.Range(-1.0f, 1.0f)) * 200;
                int n_x = (x != 0 ? (int)Mathf.Sign(x) : 0);
                int n_z = (z != 0 ? (int)Mathf.Sign(z) : 0);
                if ((int)com.status.position.x + n_x >= 0 && (int)com.status.position.z + n_z >= 0 &&
                    map.map[(int)com.status.position.x + n_x, (int)com.status.position.z + n_z].chara_type == 0 &&
					map.map[(int)com.status.position.x + n_x, (int)com.status.position.z + n_z].tile_type == 1)
                {
                    map.map[(int)com.status.position.x, (int)com.status.position.z].chara_type = 0;
                    map.map[(int)com.status.position.x + n_x, (int)com.status.position.z + n_z].chara_type = com.status.type;

                    map.map[(int)com.status.position.x, (int)com.status.position.z].chara_id = 0;
                    map.map[(int)com.status.position.x + n_x, (int)com.status.position.z + n_z].chara_id = com.status.id;

                    com.set_position(new Vector3(n_x, 0, n_z));
                    com.set_direction(new Vector3(n_x, 0, n_z));

                    com.is_move = true;
                    com.move(x, z);
                }else{
                    Debug.Log("can't move");
                    com.status.is_action = true;
                }
            }
        }
    }

    //敵の行動フラグをリセットする
    public void turn_reset(){
        for (int x = 0; x < enemy_list.Count; x++)
        {
            Enemy com = enemy_list[x].GetComponent<Enemy>();
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
