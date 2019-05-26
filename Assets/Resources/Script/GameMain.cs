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
    public StairsListPresenter stairsListPresenter;
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
        mapPresenter.map[pos[0], pos[1]].charaType = playerPresenter.status.type;
        mapPresenter.map[pos[0], pos[1]].charaId = playerPresenter.status.id;
			
        //item 配置
        itemsListPresenter.generate(mapPresenter);
        
        //階段配置
        stairsListPresenter.Generate(mapPresenter);
        
        //階段配置
        stairsListPresenter.DummyGenerate(mapPresenter, mapPresenter.CanSetObject(pos));

        menuPresenter.itemMenuPresenter.Initialize(playerPresenter.itemModels);

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
                    
                    if (x != 0 || z != 0)
                    {
                        //移動先の情報、何もいないか
                        int n_x = (x != 0 ? (int) Mathf.Sign(x) : 0);
                        int n_z = (z != 0 ? (int) Mathf.Sign(z) : 0);

                        int afterPositionX = (int) playerPresenter.status.position.x + n_x;
                        int afterPositionZ = (int) playerPresenter.status.position.z + n_z;
                        if (afterPositionX >= 0 && (int) playerPresenter.status.position.z + n_z >= 0 &&
                            mapPresenter.map[afterPositionX, afterPositionZ]
                                .charaType == 0 &&
                            (mapPresenter.map[afterPositionX, afterPositionZ]
                                .tileType == TileModel.TileType.Floor ||
                            mapPresenter.map[afterPositionX, afterPositionZ]
                                .tileType == TileModel.TileType.Stairs))
                        {
                            if (!playerPresenter.is_move && playerPresenter.status.is_action == false)
                            {
                                if (!is_lshift)
                                {
                                    Debug.Log("move:x=" + x);
                                    Debug.Log("move:z=" + z);
                                    playerPresenter.move(x, z);
                                    
                                    mapPresenter.map[(int) playerPresenter.status.position.x, (int) playerPresenter.status.position.z].charaId = 0;
                                    mapPresenter.map[(int) playerPresenter.status.position.x, (int) playerPresenter.status.position.z].charaType = 0;
                                    
                                    mapPresenter.map[afterPositionX, afterPositionZ].charaType = 1;
                                    mapPresenter.map[afterPositionX, afterPositionZ].charaId = playerPresenter.status.id;
                                    
                                    playerPresenter.set_position(new Vector3(n_x, 0, n_z));
                                    playerPresenter.set_direction(new Vector3(n_x, 0, n_z));
                                    
                                    //アイテムがあれば取得
                                    if (mapPresenter.map[(int) playerPresenter.status.position.x,
                                                (int) playerPresenter.status.position.z]
                                            .itemType == 1)
                                    {
                                        Debug.Log("get item");
                                        ItemModel itemModel = itemsListPresenter.find(mapPresenter.map[(int)playerPresenter.status.position.x, (int)playerPresenter.status.position.z].itemGuid);
                                        
                                        
                                        if (itemModel != null)
                                        {
                                            // itemを取得出来たらマップ上のitemを削除
                                            if (playerPresenter.SetItem(itemModel))
                                            {
                                                itemsListPresenter.delete(mapPresenter
                                                    .map[(int) playerPresenter.status.position.x,
                                                        (int) playerPresenter.status.position.z].itemGuid);
                                            }
                                        }
                                        else
                                        {                                            
                                            Debug.Log(String.Format("FATAL ERROR:Get Item By GUID {0}",mapPresenter.map[(int)playerPresenter.status.position.x, (int)playerPresenter.status.position.z].itemGuid.ToString()));
                                        }
                                        
                                    }
                                    
                                    //階段あれば移動
                                    if(mapPresenter.map[afterPositionX, afterPositionZ]
                                           .tileType == TileModel.TileType.Stairs)
                                    {
                                        Debug.Log("In Stairs");
                                        mapPresenter.regenerate();
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
