using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GameMain : MonoBehaviour {

    public GameStatusModel gameStatus;
    
    // Use this for initialization
    [SerializeField]
    public PlayerPresenter playerPresenter;
    [SerializeField]
    private MapPresenter mapPresenter;
    [SerializeField]    
    public ItemListPresenter itemsListPresenter;
    [SerializeField]    
    public EnemyListPresenter enemiesListPresenter;
    [SerializeField]    
    public MenuPresenter menuPresenter;

	void Start () {
        ItemData.Load();
        
        playerPresenter.initialize();
        playerPresenter.status.id = playerPresenter.GetInstanceID();
        gameStatus = new GameStatusModel();
        
        //map 生成
        mapPresenter.generate();

        //enemy 配置
        enemiesListPresenter.generate(mapPresenter);

        //player 配置
        List<int> pos = mapPresenter.get_pop_point();
        playerPresenter.dummy_initialize(pos);
        mapPresenter.map[pos[0], pos[1]].chara_type = playerPresenter.status.type;
        mapPresenter.map[pos[0], pos[1]].chara_id = playerPresenter.status.id;
			
        //item 配置
        itemsListPresenter.generate(mapPresenter);

        menuPresenter.itemMenuPresenter.Initialize(playerPresenter.itemsModels);

    }
	
    public int phase = 2;
    // Update is called once per frame
	void Update () {
        
		//var conf = ConfigProvider.Api;

        //rest_map=1,dungen_map=2
        if (phase == 1) {
            
        } else if (phase == 2) {
            //user_turn=1,enemy_turn=2
            if (gameStatus.turn == 1)
            {
                if (Input.GetKeyDown(KeyCode.I))
                {
                    menuPresenter.itemMenuPresenter.ShowItemMenu(!menuPresenter.itemMenuPresenter.GetIsShowItemMenu());
                }
                
                if (!menuPresenter.itemMenuPresenter.GetIsShowItemMenu())
                {
                    //攻撃
                    if (Input.GetKeyDown(KeyCode.X) && playerPresenter.status.is_action == false && !playerPresenter.is_move)
                    {
                        playerPresenter.attack(mapPresenter, enemiesListPresenter);
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
                        if ((int) playerPresenter.status.position.x + n_x >= 0 && (int) playerPresenter.status.position.z + n_z >= 0 &&
                            mapPresenter.map[(int) playerPresenter.status.position.x + n_x, (int) playerPresenter.status.position.z + n_z]
                                .chara_type == 0 &&
                            mapPresenter.map[(int) playerPresenter.status.position.x + n_x, (int) playerPresenter.status.position.z + n_z]
                                .tile_type == 1)
                        {
                            if (!playerPresenter.is_move && playerPresenter.status.is_action == false)
                            {
                                if (!is_lshift)
                                {
                                    Debug.Log("move:x=" + x);
                                    Debug.Log("move:z=" + z);
                                    playerPresenter.move(x, z);
                                    mapPresenter.map[(int) playerPresenter.status.position.x, (int) playerPresenter.status.position.z].chara_type =
                                        0;
                                    mapPresenter.map[(int) playerPresenter.status.position.x + n_x, (int) playerPresenter.status.position.z + n_z]
                                        .chara_type = 1;
                                    mapPresenter.map[(int) playerPresenter.status.position.x, (int) playerPresenter.status.position.z].chara_id =
                                        0;
                                    mapPresenter.map[(int) playerPresenter.status.position.x + n_x, (int) playerPresenter.status.position.z + n_z]
                                        .chara_id = playerPresenter.status.id;
                                    playerPresenter.set_position(new Vector3(n_x, 0, n_z));
                                    playerPresenter.set_direction(new Vector3(n_x, 0, n_z));
                                    
                                    //アイテムがあれば取得
                                    if (mapPresenter.map[(int) playerPresenter.status.position.x,
                                                (int) playerPresenter.status.position.z]
                                            .item_type == 1)
                                    {
                                        Debug.Log("get item");
                                        
                                        itemsListPresenter.delete(mapPresenter.map[(int)playerPresenter.status.position.x, (int)playerPresenter.status.position.z].item_id);
                                    }
                                }
                            }
                        }

                        if (!playerPresenter.is_move && is_lshift)
                        {
                            playerPresenter.set_direction(new Vector3(n_x, 0, n_z));
                        }
                    }

                    //自分が動いたら敵のターンにする
                    if (playerPresenter.status.is_action)
                    {
                        enemiesListPresenter.turn_reset();
                        enemiesListPresenter.all_action(mapPresenter);
                        gameStatus.turn = 2;
                    }
                }
            }
            else if (gameStatus.turn == 2)
            {
                //敵が全部動いていればユーザーのターンにする
                if(enemiesListPresenter.is_all_action())
                {
                    playerPresenter.status.is_action = false;
                    gameStatus.turn = 1;
                }
            }

        }
	}
		
}
