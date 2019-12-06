using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.WSA;

public class CharacterListPresenter : MonoBehaviour {

    public Dictionary<int, GameObject> characterListObject = new Dictionary<int, GameObject>();
    public Dictionary<int, CharacterPresenter> characterListPresenter = new Dictionary<int, CharacterPresenter>();
    public int serialGuid = 1;

    public void Generate(MapPresenter mapPresenter, UserModel model)
    {        
        GameObject res = Resources.Load("Object/"+model.modelName) as GameObject;
        List<int> pos = mapPresenter.GetPopPoint();
        int posX = pos[0];
        int posZ = pos[1];
        GameObject obj = Object.Instantiate(res, new Vector3(posX, 0, posZ), Quaternion.identity) as GameObject;
        obj.layer = 9;

        CharacterPresenter characterPresenter = obj.GetComponent<CharacterPresenter>();
        characterPresenter.Initialize(model, serialGuid);

        characterPresenter.SetMapData(
            mapPresenter.map[posX, posZ].floorId,
            new Vector3(posX, 0, posZ),
            new Vector3(0, 0, -1)
            );
        
        characterListPresenter[serialGuid] = characterPresenter;
        characterListObject[serialGuid] = obj;
        serialGuid++;

        //mapに配置
        mapPresenter.SetUserModel(posX, posZ, characterPresenter.status);

        Debug.Log(characterPresenter.status.position.x + ":"+ characterPresenter.status.position.z + "," + characterPresenter.characterView.trans.position.x + ":" + characterPresenter.characterView.trans.position.z);
    }
    
    /*
    /// <summary>
    /// 階段場所直接指定
    /// </summary>
    /// <param name="mapPresenter"></param>
    /// <param name="pos"></param>
    public void DummyGenerate(MapPresenter mapPresenter, List<int> pos)
    {
        if (pos != null)
        {
            GameObject res = Resources.Load("Object/Enemy") as GameObject;
            for (int x = 0; x < 1; x++)
            {
                int posX = pos[0];
                int posZ = pos[1];
                GameObject obj = Object.Instantiate(res, new Vector3(posX, 0, posZ), Quaternion.identity) as GameObject;
                obj.layer = 9;

                CharacterPresenter characterPresenter = obj.GetComponent<CharacterPresenter>();
                characterPresenter.Initialize(EnemyData.GetRandom(), serialGuid);
                
                characterPresenter.SetMapData(
                    mapPresenter.map[posX, posZ].floorId,
                    new Vector3(posX, 0, posZ),
                    new Vector3(0, 0, -1)
                );
                                
                characterListPresenter[serialGuid] = characterPresenter;
                characterListObject[serialGuid] = obj;
                serialGuid++;
                
                //mapに配置
                mapPresenter.SetUserModel(pos[0], pos[1], characterPresenter.status);
            }
        }
    }
    */

    //敵が全部動いたかどうか
    public bool IsAllAction()
    {
        bool is_end = true;
        foreach (KeyValuePair<int, CharacterPresenter> enemy in characterListPresenter.Where(presenter => presenter.Value.status.type == TileModel.CharaType.Enemy))
        {
            CharacterPresenter enemyPresenter = enemy.Value;
            if (enemyPresenter.status.isAction == false && enemyPresenter.isMove == true)
            {
                is_end = !is_end;
                break;
            }
        }

        return is_end;
    }

    //全ての敵に行動させる
    public void AllAction(MapPresenter mapPresenter)
    {
        // Dictionary<int, EnemyPresenter> plauerInSideFloorEnemy = enemyListPresenter.Select(enemy =>
        // enemy.Value.status.floorId == 0).ToDictionary(enemy => );
        
        Dictionary<int, CharacterPresenter> plauerInSideFloorEnemy = new Dictionary<int, CharacterPresenter>();
        Dictionary<int, CharacterPresenter> plauerOutSideFloorEnemy  = new Dictionary<int, CharacterPresenter>();

        /// TODO全プレイヤーで検索する
        CharacterPresenter player = characterListPresenter.FirstOrDefault(presenter => presenter.Value.status.type == TileModel.CharaType.Player).Value;

        //プレイヤーと同じフロアにいるかで分ける
        foreach (KeyValuePair<int, CharacterPresenter> enemy in characterListPresenter.Where(presenter => presenter.Value.status.type == TileModel.CharaType.Enemy))
        {
            if (enemy.Value.status.floorId == player.status.floorId)
            {
                plauerInSideFloorEnemy.Add(enemy.Key, enemy.Value);
            }
            else
            {
                plauerOutSideFloorEnemy.Add(enemy.Key, enemy.Value);
            }
        }

        //プレイヤーに近い順に並べて行動させるようにソートする。
        foreach (KeyValuePair<int, CharacterPresenter> enemy in plauerInSideFloorEnemy)
        {
            //GetFirstPositionAStar(enemy.Value.status.position, playerPresenter.status.position, mapPresenter);
        }        

        foreach (KeyValuePair<int, CharacterPresenter> enemy in characterListPresenter.Where(presenter => presenter.Value.status.type == TileModel.CharaType.Enemy))
        {
            CharacterPresenter characterPresenter = enemy.Value;
            int actionType = characterPresenter.GetAction();
            //とりあえず1を移動
            if (actionType == 1)
            {
                float x = Mathf.Sign(Random.Range(-1.0f, 1.0f)) * 200;
                float z = Mathf.Sign(Random.Range(-1.0f, 1.0f)) * 200;
                int nX = (x != 0 ? (int)Mathf.Sign(x) : 0);
                int nZ = (z != 0 ? (int)Mathf.Sign(z) : 0);
                
                int beforePositionX = (int) characterPresenter.status.position.x;
                int beforePositionZ = (int) characterPresenter.status.position.z;
                int afterPositionX = beforePositionX + nX;
                int afterPositionZ = beforePositionZ + nZ;
                if (mapPresenter.IsCanMove(afterPositionX, afterPositionZ, TileModel.CharaType.Enemy))
                {
                    
                    //Debug.Log(mapPresenter.map[afterPositionX, afterPositionZ].tileType);
                    //Debug.Log(enemyPresenter.status.position.x + ":"+ enemyPresenter.status.position.z + "," + enemyPresenter.enemyView.trans.position.x + ":" + enemyPresenter.enemyView.trans.position.z);
                    
                    characterPresenter.Move(x, z);
                    
                    //移動元と移動先にキャラクター情報を設定
                    mapPresenter.SetUserModel(beforePositionX, beforePositionZ, null);
                    mapPresenter.SetUserModel(afterPositionX, afterPositionZ, characterPresenter.status);
                    
                    //StartMoveしてからSetPositionをする。
                    
                    characterPresenter.SetMapData(
                        mapPresenter.map[afterPositionX, afterPositionZ].floorId,
                        new Vector3(afterPositionX, 0, afterPositionZ),
                        new Vector3(nX, 0, nZ)
                    );
                }else{
                    //移動先がなければ行動済みにする。
                    characterPresenter.SetIsAction(true);
                }
            }else{
                characterPresenter.SetIsAction(true);
            }
        }
    }

    private List<Vector3> directions = new List<Vector3>
    {
        new Vector3(-1, 0, -1),
        new Vector3(-1, 0, 0),
        new Vector3(-1, 0, 1),
        new Vector3(0, 0, -1),
//        new List<Vector3> {0, 0, 0},
        new Vector3(0, 0, 1),
        new Vector3(1, 0, -1),
        new Vector3(1, 0, 0),
        new Vector3(1, 0, 1)
    };

    private class AStarCost
    {
        public Vector3 position = new Vector3(0,0,0);
        public int status = 0; // 0=none,1=open,2=close
        public int cost = 0;
        public int estimateCost = 0;
        public int score = 0;

    }
    
    private Vector3 GetFirstPositionAStar(Vector3 from, Vector3 to, MapPresenter mapPresenter)
    {
        AStarCost nowNode = new AStarCost();       
        
        nowNode.position = from;
        
        var ecX = (int)to.x - (int)from.x;
        var ecY = (int)to.y - (int)from.y;
        
        nowNode.cost = ecX > ecY ? ecX : ecY;
        nowNode.estimateCost = 0;
        nowNode.score = nowNode.cost + nowNode.estimateCost;

        AStarCost aStarCost = GetPositionAStar(nowNode, to, new List<AStarCost>(), new List<AStarCost>(), mapPresenter);

        return aStarCost.position;
    }

    private AStarCost GetPositionAStar(AStarCost aStar, Vector3 to, List<AStarCost> routeAStarList, List<AStarCost> cacheAStarCostList, MapPresenter mapPresenter)
    {
        aStar.status = 2;
        cacheAStarCostList.Add(aStar);
        foreach (Vector3 direction in directions)
        {
            AStarCost nowNode = new AStarCost
            {
                position = aStar.position + direction,
                estimateCost = aStar.estimateCost + 1, 
                status = 1
            };

            var ecX = (int)to.x - (int)nowNode.position.x;
            var ecY = (int)to.y - (int)nowNode.position.y;
            
            nowNode.cost = ecX > ecY ? ecX : ecY;
            nowNode.score = nowNode.cost + nowNode.estimateCost;
            
            var cache = cacheAStarCostList.FirstOrDefault(aster => aster.position == to);
            if (cache == null)
            {
                if (mapPresenter.IsCanMove((int) nowNode.position.x, (int) nowNode.position.z, TileModel.CharaType.Player))
                {
                    routeAStarList.Add(nowNode);
                    GetPositionAStar(nowNode, to, routeAStarList, cacheAStarCostList, mapPresenter);
                }
                else
                {
                    //移動できない場所だったらcloseしてcacheに入れる
                    nowNode.status = 2;
                    cacheAStarCostList.Add(nowNode);
                    return new AStarCost();
                }
            }
            else
            {
                //cacheにある
                return new AStarCost();
            }
        }
        return new AStarCost();
    }


    //敵の行動フラグをリセットする
    public void TurnReset()
    {    
        foreach (KeyValuePair<int, CharacterPresenter> enemy in characterListPresenter.Where(presenter => presenter.Value.status.type == TileModel.CharaType.Enemy))
        {
            enemy.Value.SetIsAction(false);
        }
    }
    
	public void Delete(CharacterPresenter characterPresenter)
	{
        int guid = characterPresenter.status.guid;
        if (characterPresenter != null)
        {
            Destroy(characterListObject[guid]);
            characterListPresenter.Remove(guid);
        }
        Debug.Log(characterListPresenter.Count);
	}
		
    public CharacterPresenter GetCharacter(int guid)
    {
        return characterListPresenter.FirstOrDefault(enemy =>
            enemy.Key == guid).Value;
    }
	
    public void ShowLog()
    {

        foreach (KeyValuePair<int, CharacterPresenter> character in characterListPresenter)
        {
            //Debug.Log(enemy.Key);
            CharacterPresenter characterPresenter = character.Value;
            //Debug.Log(enemyPresenter.status.position.x + ":"+ enemyPresenter.status.position.z + "," + enemyPresenter.enemyView.trans.position.x + ":" + enemyPresenter.enemyView.trans.position.z);
        }
    }

}
