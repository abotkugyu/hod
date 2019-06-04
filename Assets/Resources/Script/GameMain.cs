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
        EnemyData.Load();
        
        playerPresenter.Initialize();
        playerPresenter.status.id = 1;
        playerPresenter.status.type = TileModel.CharaType.Player;
        gameStatus = new GameStatusModel();
        
        //map 生成
        mapPresenter.Generate();

        //enemy 配置
        enemiesListPresenter.Generate(mapPresenter);
        enemiesListPresenter.ShowLog();

        //player 配置
        List<int> pos = mapPresenter.GetPopPoint();
        playerPresenter.DummyInitialize(pos);
        
        
        Debug.Log("PP:"+playerPresenter.status.position.x + ":"+ playerPresenter.status.position.z + "," + playerPresenter.playerView.trans.position.x + ":" + playerPresenter.playerView.trans.position.z);
        
        mapPresenter.map[pos[0], pos[1]].charaType = playerPresenter.status.type;
        mapPresenter.map[pos[0], pos[1]].charaId = playerPresenter.status.id;
			
        //item 配置
        itemsListPresenter.Generate(mapPresenter);
        
        //階段配置
        stairsListPresenter.Generate(mapPresenter);
        
        //dummy エネミー配置
        //enemiesListPresenter.DummyGenerate(mapPresenter, mapPresenter.CanSetObject(pos));
        
        //dummy 階段配置
        stairsListPresenter.DummyGenerate(mapPresenter, mapPresenter.CanSetObject(pos));

        menuPresenter.itemMenuPresenter.Initialize(playerPresenter.itemModels);
                
        //dummy エネミー配置

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
                
                if (Input.GetKeyDown(KeyCode.L))
                {
                    mapPresenter.ShowMapInfo();
                }
                
                if (!menuPresenter.itemMenuPresenter.GetIsShowItemMenu())
                {
                    //攻撃
                    if (Input.GetKeyDown(KeyCode.X) && playerPresenter.status.isAction == false && !playerPresenter.isMove)
                    {
                        playerPresenter.Attack(mapPresenter, enemiesListPresenter);
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

                        int beforePositionX = (int) playerPresenter.status.position.x;
                        int beforePositionZ = (int) playerPresenter.status.position.z;
                        int afterPositionX = beforePositionX + n_x;
                        int afterPositionZ = beforePositionZ + n_z;
                        if (mapPresenter.IsCanMove(afterPositionX, afterPositionZ, TileModel.CharaType.Player))
                        {
                            if (!playerPresenter.isMove && playerPresenter.status.isAction == false)
                            {
                                if (!is_lshift)
                                {
                                    Debug.Log("player move:x=" + x);
                                    Debug.Log("player move:z=" + z);
                                    playerPresenter.Move(x, z);
                                    
                                    //移動元と移動先にキャラクター情報を設定
                                    mapPresenter.SetUserModel(beforePositionX, beforePositionZ, null);
                                    mapPresenter.SetUserModel(afterPositionX, afterPositionZ, playerPresenter.status);
                                                                        
                                    //アイテムがあれば取得
                                    if (mapPresenter.map[afterPositionX, afterPositionZ].itemType == 1)
                                    {
                                        Debug.Log("get item");
                                        
                                        ItemModel itemModel = itemsListPresenter.Find(mapPresenter.map[afterPositionX, afterPositionZ].itemGuid);                                                                                
                                        if (itemModel != null)
                                        {
                                            // itemを取得出来たらマップ上のitemを削除
                                            if (playerPresenter.SetItem(itemModel))
                                            {
                                                itemsListPresenter.Delete(mapPresenter.map[afterPositionX, afterPositionZ].itemGuid);
                                            }
                                        }
                                        else
                                        {                                            
                                            Debug.Log(String.Format("FATAL ERROR:Get Item By GUID {0}",mapPresenter.map[afterPositionX, afterPositionZ].itemGuid.ToString()));
                                        }                                        
                                    }
                                                                        
                                    playerPresenter.SetPosition(new Vector3(afterPositionX, 0, afterPositionZ));
                                    playerPresenter.SetDirection(new Vector3(n_x, 0, n_z));
                                    
                                    //階段あれば移動
                                    if(mapPresenter.map[afterPositionX, afterPositionZ]
                                           .tileType == TileModel.TileType.Stairs)
                                    {
                                        Debug.Log("In Stairs");
                                        mapPresenter.Regenerate();
                                    }
                                }
                            }
                        }

                        if (!playerPresenter.isMove && is_lshift)
                        {
                            playerPresenter.SetDirection(new Vector3(n_x, 0, n_z));
                        }
                    }

                    //自分が動いたら敵のターンにする
                    if (playerPresenter.status.isAction)
                    {
                        enemiesListPresenter.TurnReset();
                        enemiesListPresenter.AllAction(mapPresenter);
                        gameStatus.turn = 2;
                    }
                }
            }
            else if (gameStatus.turn == 2)
            {
                //敵が全部動いていればユーザーのターンにする
                if(enemiesListPresenter.IsAllAction())
                {
                    playerPresenter.status.isAction = false;
                    gameStatus.turn = 1;
                }
            }
        }
	}		
}
