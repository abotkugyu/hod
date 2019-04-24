using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMain : MonoBehaviour {

    // Use this for initialization
    public Player player;
    public ItemController items;
    public EnemyController enemies;
    public GameModel status;
    public GameMap map;
    public ItemMenu item_menu;

	void Start () {
        map = GetComponent<GameMap>();
        GameObject obj = GameObject.Find("Player");
        player = (obj).GetComponent<Player>();
        player.status.id = obj.GetInstanceID();

        status = new GameModel();
        enemies = new EnemyController();
        items = new ItemController();
        
        //item menu
        item_menu = (GameObject.Find("ItemMenu")).GetComponent<ItemMenu>();
	}
    public int phase = 2;
    public bool is_menu = false;
    public int turn = 1;
    public bool is_create_map = false;
    // Update is called once per frame
	void Update () {
        
		//var conf = ConfigProvider.Api;

        if (is_create_map == false)
        {
            map.generate();
            is_create_map = !is_create_map;

            //player 配置
            map.map[0, 0].chara_type = 1;
            map.map[0, 0].chara_id = player.status.id;

            //enemy 配置
            enemies.generate(map);

			List<int> pos = map.get_pop_point();
			int posx = pos[0];
			int posz = pos[1];
			player.transform.position = new Vector3(posx, 0, posz);
			player.set_position(new Vector3(posx, 0, posz));
			player.set_direction(new Vector3(0, 0, -1));
			//mapに配置
			map.map[posx, posz].chara_type = player.status.type;
			map.map[posx, posz].chara_id = player.status.id;
			
			
            //item 配置
            items.generate(map);
        }

        //rest_map=1,dungen_map=2
        if (phase == 1) {
            
        } else if (phase == 2) {
            //user_turn=1,enemy_turn=2
            if (status.turn == 1)
            {
                if (Input.GetKeyDown(KeyCode.I))
                {
                    if (!item_menu.is_visible())
                    {
                        item_menu.show();
                    }
                    else
                    {
                        item_menu.hide();
                    }
                }
                
                if (item_menu.is_visible())
                {
                    if (Input.GetKeyDown(KeyCode.DownArrow))
                    {
                    }
                    else if (Input.GetKeyDown(KeyCode.UpArrow))
                    {
                    }
                } else {
                    //攻撃
                    if (Input.GetKeyDown(KeyCode.X) && player.status.is_action == false && !player.is_move)
                    {
                        player.attack(map, enemies);
                    }

                    bool is_lshift = Input.GetKey(KeyCode.LeftShift);
                    //移動
                    float x = Input.GetAxisRaw("Horizontal") * 200;
                    float z = Input.GetAxisRaw("Vertical") * 200;
                    
                    Debug.Log("move");
                    if (x != 0 || z != 0)
                    {
                        //移動先の情報、何もいないか
                        int n_x = (x != 0 ? (int) Mathf.Sign(x) : 0);
                        int n_z = (z != 0 ? (int) Mathf.Sign(z) : 0);
                        if ((int) player.status.position.x + n_x >= 0 && (int) player.status.position.z + n_z >= 0 &&
                            map.map[(int) player.status.position.x + n_x, (int) player.status.position.z + n_z]
                                .chara_type == 0 &&
                            map.map[(int) player.status.position.x + n_x, (int) player.status.position.z + n_z]
                                .tile_type == 1)
                        {
                            if (!player.is_move && player.status.is_action == false)
                            {
                                if (!is_lshift)
                                {
                                    Debug.Log("move:x=" + x);
                                    Debug.Log("move:z=" + z);
                                    player.move(x, z);
                                    map.map[(int) player.status.position.x, (int) player.status.position.z].chara_type =
                                        0;
                                    map.map[(int) player.status.position.x + n_x, (int) player.status.position.z + n_z]
                                        .chara_type = 1;
                                    map.map[(int) player.status.position.x, (int) player.status.position.z].chara_id =
                                        0;
                                    map.map[(int) player.status.position.x + n_x, (int) player.status.position.z + n_z]
                                        .chara_id = player.status.id;
                                    player.set_position(new Vector3(n_x, 0, n_z));
                                    player.set_direction(new Vector3(n_x, 0, n_z));
                                    
                                    //アイテムがあれば取得
                                    if (map.map[(int) player.status.position.x,
                                                (int) player.status.position.z]
                                            .item_type == 1)
                                    {
                                        Debug.Log("get item");
                                        
                                        items.delete(map.map[(int)player.status.position.x, (int)player.status.position.z].item_id);
                                    }
                                }
                            }
                        }

                        if (!player.is_move && is_lshift)
                        {
                            player.set_direction(new Vector3(n_x, 0, n_z));
                        }
                    }

                    //自分が動いたら敵のターンにする
                    if (player.status.is_action == true)
                    {
                        enemies.turn_reset();
                        enemies.all_action(map);
                        status.turn = 2;
                    }
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
	}
		
}
