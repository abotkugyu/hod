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
            obj.transform.Translate(x*2 + 1, 0, x*2 + 1);
            Enemy com = obj.GetComponent<Enemy>();
            com.status.id = obj.GetInstanceID();
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
    public void all_action()
    {
        for (int x = 0; x < enemy_list.Count; x++)
        {
            enemy_list[x].action();
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
