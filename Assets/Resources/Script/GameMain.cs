using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class GameMain : MonoBehaviour {

    public GameStatusModel gameStatus;
    
    // Use this for 
    [SerializeField]
    private MapPresenter mapPresenter;
    [SerializeField]    
    public ItemListPresenter itemsListPresenter;
    
    public CharacterListPresenter characterListPresenter;
    
    [SerializeField]    
    public StairsListPresenter stairsListPresenter;
    [SerializeField]    
    public MenuPresenter menuPresenter;

	void Start () {
        ItemData.Load();
        EnemyData.Load();
        PlayerData.Load();
        
        //map 生成
        mapPresenter.Generate();
                    
        //enemy生成
        List<UserModel> enemies = EnemyData.GetRandoms(10);
        enemies.ForEach(enemy => characterListPresenter.Generate(mapPresenter, enemy));

        //player生成
        UserModel player = PlayerData.GetRandom();
        player.isOwn = true;
        characterListPresenter.Generate(mapPresenter, player);
        
        
        gameStatus = new GameStatusModel();
        
        //item 配置
        itemsListPresenter.Generate(mapPresenter);
        
        //階段配置
        stairsListPresenter.Generate(mapPresenter);
        
        //dummy エネミー配置
        //enemiesListPresenter.DummyGenerate(mapPresenter, mapPresenter.CanSetObject(pos));
        
        //dummy 階段配置
        List<int> pos = mapPresenter.GetPopPoint();
        stairsListPresenter.DummyGenerate(mapPresenter, mapPresenter.CanSetObject(pos));

        menuPresenter.itemMenuPresenter.Initialize(characterListPresenter.characterListPresenter.FirstOrDefault(presenter => presenter.Value.status.isOwn).Value.itemModels);
                
        //dummy エネミー配置

    }
	
    public int phase = 2;
    // Update is called once per frame
	void Update ()
    {
        var characterPresenter = characterListPresenter.characterListPresenter.FirstOrDefault(presenter => presenter.Value.status.isOwn).Value;
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
                    if (Input.GetKeyDown(KeyCode.X) && characterPresenter.status.isAction == false && !characterPresenter.isMove)
                    {
                        characterPresenter.Attack(mapPresenter, characterListPresenter);
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

                        int beforePositionX = (int) characterPresenter.status.position.x;
                        int beforePositionZ = (int) characterPresenter.status.position.z;
                        int afterPositionX = beforePositionX + n_x;
                        int afterPositionZ = beforePositionZ + n_z;
                        if (mapPresenter.IsCanMove(afterPositionX, afterPositionZ, TileModel.CharaType.Player))
                        {
                            if (!characterPresenter.isMove && characterPresenter.status.isAction == false)
                            {
                                if (!is_lshift)
                                {
                                    Debug.Log("player move:x=" + x);
                                    Debug.Log("player move:z=" + z);
                                    characterPresenter.Move(x, z);
                                    
                                    //移動元と移動先にキャラクター情報を設定
                                    mapPresenter.SetUserModel(beforePositionX, beforePositionZ, null);
                                    mapPresenter.SetUserModel(afterPositionX, afterPositionZ, characterPresenter.status);
                                                                        
                                    //アイテムがあれば取得
                                    if (mapPresenter.map[afterPositionX, afterPositionZ].itemType == 1)
                                    {
                                        Debug.Log("get item");
                                        
                                        ItemModel itemModel = itemsListPresenter.Find(mapPresenter.map[afterPositionX, afterPositionZ].itemGuid);                                                                                
                                        if (itemModel != null)
                                        {
                                            // itemを取得出来たらマップ上のitemを削除
                                            if (characterPresenter.SetItem(itemModel))
                                            {
                                                itemsListPresenter.Delete(mapPresenter.map[afterPositionX, afterPositionZ].itemGuid);
                                            }
                                        }
                                        else
                                        {                                            
                                            Debug.Log(String.Format("FATAL ERROR:Get Item By GUID {0}",mapPresenter.map[afterPositionX, afterPositionZ].itemGuid.ToString()));
                                        }                                        
                                    }
                                                                        
                                    characterPresenter.SetPosition(new Vector3(afterPositionX, 0, afterPositionZ));
                                    characterPresenter.SetDirection(new Vector3(n_x, 0, n_z));
                                    
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
                        else
                        {
                            if (!characterPresenter.isMove){
                                characterPresenter.SetDirection(new Vector3(n_x, 0, n_z));
                            }
                        }
                    

                        if (!characterPresenter.isMove && is_lshift)
                        {
                            characterPresenter.SetDirection(new Vector3(n_x, 0, n_z));
                        }
                    }

                    //自分が動いたら敵のターンにする
                    if (characterPresenter.status.isAction)
                    {
                        characterListPresenter.TurnReset();
                        characterListPresenter.AllAction(mapPresenter);
                        gameStatus.turn = 2;
                    }
                }
            }
            else if (gameStatus.turn == 2)
            {
                //敵が全部動いていればユーザーのターンにする
                if(characterListPresenter.IsAllAction())
                {
                    characterPresenter.status.isAction = false;
                    gameStatus.turn = 1;
                }
            }
        }
	}		
}
