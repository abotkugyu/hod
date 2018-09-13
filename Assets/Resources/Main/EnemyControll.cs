using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//敵情報管理
public class EnemyControll : MonoBehaviour
{

    public List<Enemy> enemy_list = new List<Enemy>();
    public int num = 2;

    public void generate()
    {
        for (int x = 0; x < num; x++)
        {
            GameObject obj = Object.Instantiate(Resources.Load("Object/Enemy")) as GameObject;
            obj.transform.position = new Vector3(x * 2 + 1, 0, x * 2 + 1);
            obj.layer = 9;
            Enemy com = obj.GetComponent<Enemy>();
            com.status.id = obj.GetInstanceID();
            com.status.position = new Vector3(x * 2 + 1, 0, x * 2 + 1);
            enemy_list.Add(com);
        }
    }

    //敵が全部動いたかどうか
    public bool is_all_action()
    {
        bool is_end = true;
        for (int x = 0; x < enemy_list.Count; x++)
        {
            if (enemy_list[x].status.is_action == false)
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
            int action_type = enemy_list[l].get_action();
            //とりあえず1を移動
            if (action_type == 1)
            {
                float x = Mathf.Sign(Random.Range(-1.0f, 1.0f)) * 200;
                float z = Mathf.Sign(Random.Range(-1.0f, 1.0f)) * 200;
                int n_x = (x != 0 ? (int)Mathf.Sign(x) : 0);
                int n_z = (z != 0 ? (int)Mathf.Sign(z) : 0);
                if ((int)enemy_list[l].status.position.x + n_x >= 0 && (int)enemy_list[l].status.position.z + n_z >= 0 &&
                    map.map[(int)enemy_list[l].status.position.x + n_x, (int)enemy_list[l].status.position.z + n_z].chara_type == 0)
                {
                    map.map[(int)enemy_list[l].status.position.x, (int)enemy_list[l].status.position.z].chara_type = 0;
                    map.map[(int)enemy_list[l].status.position.x + n_x, (int)enemy_list[l].status.position.z + n_z].chara_type = 1;

                    enemy_list[l].set_position(new Vector3(n_x, 0, n_z));
                    enemy_list[l].set_direction(new Vector3(n_x, 0, n_z));

                    enemy_list[l].is_move = true;
                    enemy_list[l].move(x, z);
                }else{
                    Debug.Log("can't move");
                    enemy_list[l].status.is_action = true;
                }
            }
        }
    }

    //敵の行動フラグをリセットする
    public void turn_reset(){
        for (int x = 0; x < enemy_list.Count; x++)
        {
            enemy_list[x].status.is_action = false;
        }
    }

}
