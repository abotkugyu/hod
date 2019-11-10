using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//敵情報管理
public class EnemyListPresenter : MonoBehaviour
{
    public Dictionary<int, GameObject> enemyListObject = new Dictionary<int, GameObject>();
    public Dictionary<int, EnemyPresenter> enemyListPresenter = new Dictionary<int, EnemyPresenter>();
    public int serialGuid = 1;
    public int num = 1;

    public void Generate(MapPresenter mapPresenter)
    {
        GameObject res = Resources.Load("Object/Enemy") as GameObject;
        for (int x = 0; x < num; x++)
        {
            List<int> pos = mapPresenter.GetPopPoint();
            int posX = pos[0];
            int posZ = pos[1];
            GameObject obj = Object.Instantiate(res, new Vector3(posX, 0, posZ), Quaternion.identity) as GameObject;
            obj.layer = 9;

            EnemyPresenter enemyPresenter = obj.GetComponent<EnemyPresenter>();
            enemyPresenter.Initialize(EnemyData.GetRandom(), serialGuid);

            enemyPresenter.SetMapData(
                mapPresenter.map[posX, posZ].floorId,
                new Vector3(posX, 0, posZ),
                new Vector3(0, 0, -1)
                );
            
            enemyListPresenter[serialGuid] = enemyPresenter;
            enemyListObject[serialGuid] = obj;
            serialGuid++;

            //mapに配置
            mapPresenter.SetUserModel(posX, posZ, enemyPresenter.status);
    
            Debug.Log(enemyPresenter.status.position.x + ":"+ enemyPresenter.status.position.z + "," + enemyPresenter.enemyView.trans.position.x + ":" + enemyPresenter.enemyView.trans.position.z);
        }
        Debug.Log("***********");
    }
    
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

                EnemyPresenter enemyPresenter = obj.GetComponent<EnemyPresenter>();
                enemyPresenter.Initialize(EnemyData.GetRandom(), serialGuid);
                
                enemyPresenter.SetMapData(
                    mapPresenter.map[posX, posZ].floorId,
                    new Vector3(posX, 0, posZ),
                    new Vector3(0, 0, -1)
                );
                                
                enemyListPresenter[serialGuid] = enemyPresenter;
                enemyListObject[serialGuid] = obj;
                serialGuid++;
                
                //mapに配置
                mapPresenter.SetUserModel(pos[0], pos[1], enemyPresenter.status);
            }
        }
    }

    //敵が全部動いたかどうか
    public bool IsAllAction()
    {
        bool is_end = true;
        foreach (KeyValuePair<int, EnemyPresenter> enemy in enemyListPresenter)
        {
            EnemyPresenter enemyPresenter = enemy.Value;
            if (enemyPresenter.status.isAction == false && enemyPresenter.isMove == true)
            {
                is_end = !is_end;
                break;
            }
        }

        return is_end;
    }

    //全ての敵に行動させる
    public void AllAction(MapPresenter mapPresenter, PlayerPresenter playerPresenter)
    {
        // Dictionary<int, EnemyPresenter> plauerInSideFloorEnemy = enemyListPresenter.Select(enemy =>
        // enemy.Value.status.floorId == 0).ToDictionary(enemy => );
        
        Dictionary<int, EnemyPresenter> plauerInSideFloorEnemy = new Dictionary<int, EnemyPresenter>();
        Dictionary<int, EnemyPresenter> plauerOutSideFloorEnemy  = new Dictionary<int, EnemyPresenter>();

        //プレイヤーと同じフロアにいるかで分ける
        foreach (KeyValuePair<int, EnemyPresenter> enemy in enemyListPresenter)
        {
            if (enemy.Value.status.floorId == playerPresenter.status.floorId)
            {
                plauerInSideFloorEnemy.Add(enemy.Key, enemy.Value);
            }
            else
            {
                plauerOutSideFloorEnemy.Add(enemy.Key, enemy.Value);
            }
        }

        //プレイヤーに近い順に並べて行動させるようにソートする。
        foreach (KeyValuePair<int, EnemyPresenter> enemy in plauerInSideFloorEnemy)
        {
            //GetFirstPositionAStar(enemy.Value.status.position, playerPresenter.status.position, mapPresenter);
        }        

        foreach (KeyValuePair<int, EnemyPresenter> enemy in enemyListPresenter)
        {
            EnemyPresenter enemyPresenter = enemy.Value;
            int actionType = enemyPresenter.GetAction();
            //とりあえず1を移動
            if (actionType == 1)
            {
                float x = Mathf.Sign(Random.Range(-1.0f, 1.0f)) * 200;
                float z = Mathf.Sign(Random.Range(-1.0f, 1.0f)) * 200;
                int nX = (x != 0 ? (int)Mathf.Sign(x) : 0);
                int nZ = (z != 0 ? (int)Mathf.Sign(z) : 0);
                
                int beforePositionX = (int) enemyPresenter.status.position.x;
                int beforePositionZ = (int) enemyPresenter.status.position.z;
                int afterPositionX = beforePositionX + nX;
                int afterPositionZ = beforePositionZ + nZ;
                if (mapPresenter.IsCanMove(afterPositionX, afterPositionZ, TileModel.CharaType.Enemy))
                {
                    
                    //Debug.Log(mapPresenter.map[afterPositionX, afterPositionZ].tileType);
                    //Debug.Log(enemyPresenter.status.position.x + ":"+ enemyPresenter.status.position.z + "," + enemyPresenter.enemyView.trans.position.x + ":" + enemyPresenter.enemyView.trans.position.z);
                    
                    enemyPresenter.Move(x, z);
                    
                    //移動元と移動先にキャラクター情報を設定
                    mapPresenter.SetUserModel(beforePositionX, beforePositionZ, null);
                    mapPresenter.SetUserModel(afterPositionX, afterPositionZ, enemyPresenter.status);
                    
                    //StartMoveしてからSetPositionをする。
                    
                    enemyPresenter.SetMapData(
                        mapPresenter.map[afterPositionX, afterPositionZ].floorId,
                        new Vector3(afterPositionX, 0, afterPositionZ),
                        new Vector3(nX, 0, nZ)
                    );
                }else{
                    //移動先がなければ行動済みにする。
                    enemyPresenter.SetIsAction(true);
                }
            }else{
                enemyPresenter.SetIsAction(true);
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
        foreach (KeyValuePair<int, EnemyPresenter> enemy in enemyListPresenter)
        {
            enemy.Value.SetIsAction(false);
        }
    }
    
	public void Delete(EnemyPresenter enemyPresenter)
	{
        int guid = enemyPresenter.status.guid;
        if (enemyPresenter != null)
        {
            Destroy(enemyListObject[guid]);
            enemyListPresenter.Remove(guid);
        }
        Debug.Log(enemyListPresenter.Count);
	}
		
    public EnemyPresenter GetEnemy(int guid)
    {
        return enemyListPresenter.FirstOrDefault(enemy =>
            enemy.Key == guid).Value;
    }
	
    public void ShowLog()
    {

        foreach (KeyValuePair<int, EnemyPresenter> enemy in enemyListPresenter)
        {
            //Debug.Log(enemy.Key);
            EnemyPresenter enemyPresenter = enemy.Value;
            //Debug.Log(enemyPresenter.status.position.x + ":"+ enemyPresenter.status.position.z + "," + enemyPresenter.enemyView.trans.position.x + ":" + enemyPresenter.enemyView.trans.position.z);
        }
    }

    
}
