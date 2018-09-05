using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMain : MonoBehaviour {

    // Use this for initialization
    public Player player;
    public EnemyControll enemies;
    public GameStatus status;
    public GameMap map;
	void Start () {
        map = GetComponent<GameMap>();
        GameObject obj = GameObject.Find("Player");
        player = (obj).GetComponent<Player>();
        Debug.Log(obj.GetInstanceID());
        player.status.id = obj.GetInstanceID();

        status = new GameStatus();
        enemies = new EnemyControll();
	}
    public int phase = 2;
    public int turn = 1;
    public bool is_create_map = false;
    // Update is called once per frame
	void Update () {
        
		//var conf = ConfigProvider.Api;

        //rest_map=1,dungen_map=2
        if (phase == 1) {
            
        } else if (phase == 2) {
            //user_turn=1,enemy_turn=2
            if (status.turn == 1)
            {
                //攻撃
                if (Input.GetKey(KeyCode.X) && player.status.is_action == false)
                {
                    player.attack(map);
                }

                //移動
                float x = Input.GetAxisRaw("Horizontal") * 200;
                float z = Input.GetAxisRaw("Vertical") * 200;
                if (!player.is_move && (x != 0 || z != 0) && player.status.is_action == false)
                {
                    player.move(x, z);
                    player.set_position(new Vector3((x != 0 ? Mathf.Sign(x) : 0), 0, (z != 0 ? Mathf.Sign(z) : 0)));
                    player.set_direction(new Vector3((x != 0 ? Mathf.Sign(x) : 0), 0, (z != 0 ? Mathf.Sign(z) : 0)));
                }

                //自分が動いたら敵のターンにする
                if (player.status.is_action == true) {
                    enemies.turn_reset();
                    enemies.all_action();
                    status.turn = 2;
                }
            }
            else if (status.turn == 2)
            {
                //敵が全部動いていればユーザーのターンにする
                if(enemies.is_all_action() == true)
                {
                    player.status.is_action = false;
                    status.turn = 1;
                }
            }

        }
        if (is_create_map == false)
        {
            map.generate();
            is_create_map = !is_create_map;
            //player配置 デフォルト下向き
            for (int x = 0; x < map.max_map_x; x++)
            {
                for (int z = 0; z < map.max_map_y; z++)
                {
                    if (map.map[x, z].chara_type == 0)
                    {
                        map.map[x, z].chara_type = 1;
//                        player.set_position(new Vector3(0, 0, 0));
                        break;
                    }
                }
            }
            player.set_direction(new Vector3(0, 0, -1));

            //enemy 配置
            enemies.generate();
        }
	}
		
}
