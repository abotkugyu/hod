using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ExtensionMethods;
using UnityEngine;
using UnityEngine.WSA;

public class CharacterListPresenter : MonoBehaviour {

    public Dictionary<int, GameObject> characterListObject = new Dictionary<int, GameObject>();
    public Dictionary<int, CharacterPresenter> characterListPresenter = new Dictionary<int, CharacterPresenter>();
    public int serialGuid = 1;

    public void Generate(MapPresenter mapPresenter, UserModel model)
    {        
        GameObject res = Resources.Load("Object/"+model.modelName.Replace('_','/')) as GameObject;
        Vector2Int pos = mapPresenter.GetPopPoint();
        GameObject obj = Object.Instantiate(res, new Vector3(pos.x, 0, pos.y), Quaternion.identity) as GameObject;
        obj.layer = 9;
        obj.transform.parent = transform;

        CharacterPresenter characterPresenter = obj.GetComponent<CharacterPresenter>();
        characterPresenter.Initialize(model, serialGuid);

        characterPresenter.SetMapData(
            mapPresenter.GetTileModel(pos.x, pos.y).floorId,
            new Vector3(pos.x, 0, pos.y),
            new Vector3(0, 0, -1)
            );
        
        characterListPresenter[serialGuid] = characterPresenter;
        characterListObject[serialGuid] = obj;
        serialGuid++;

        //mapに配置
        mapPresenter.SetUserModel(pos, characterPresenter.status);
   }
    
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
            enemy.Value.beforeStatus = enemy.Value.status;
            if (enemy.Value.status.floorId == player.status.floorId)
            {
                plauerInSideFloorEnemy.Add(enemy.Key, enemy.Value);
            }
            else
            {
                plauerOutSideFloorEnemy.Add(enemy.Key, enemy.Value);
            }
        }

        // プレイヤーに近い順に並べて行動させるようにソートする。
        foreach (KeyValuePair<int, CharacterPresenter> enemy in plauerInSideFloorEnemy)
        {
            //GetFirstPositionAStar(enemy.Value.status.position, playerPresenter.status.position, mapPresenter);
        }

        var around1 = new DirectionUtil().GetAroundDirection(1);
        var around100 = new DirectionUtil().GetAroundDirection(100);

        foreach (KeyValuePair<int, CharacterPresenter> enemy in characterListPresenter.Where(presenter => presenter.Value.status.type == TileModel.CharaType.Enemy))
        {
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            CharacterPresenter characterPresenter = enemy.Value;
            
            // 周りにプレイヤーがいれば攻撃
            var searchDirection1 = around1.Select(i => i + characterPresenter.status.position.GetVector2Int());
            var hitEnemyDirection = searchDirection1.Where(i => mapPresenter.SearchCharaType(i, TileModel.CharaType.Player));
                        
            if (hitEnemyDirection.Any())
            {
                Vector2Int direction = hitEnemyDirection.First() - characterPresenter.status.position.GetVector2Int();
                characterPresenter.SetDirection(direction.GetVector2Int());
                characterPresenter.Attack(mapPresenter, characterListPresenter);
                characterPresenter.SetIsAction(true);
                continue;
            }
            
            //通路を検索
            //var hitPathDirection = around100.FirstOrDefault(i => mapPresenter.SearchTileType(i + characterPresenter.status.position.GetVector2Int(), TileModel.TileType.Path));
            var pathModel = mapPresenter.GetFloorModel(characterPresenter.status.floorId).innerPath;

            var to = pathModel.up != Vector2Int.zero ? pathModel.up : Vector2Int.zero;
            to = pathModel.down != Vector2Int.zero ? pathModel.down : to;
            to = pathModel.left != Vector2Int.zero ? pathModel.left : to;
            to = pathModel.right != Vector2Int.zero ? pathModel.right : to;

            // 攻撃できなければランダムアクション
            int actionType = characterPresenter.GetAction();
            if (actionType == 1)
            {
//                var distance = hitPathDirection - characterPresenter.status.position.GetVector2Int();
                InputAxis axis = InputAxis.GetRandomAxis();
                if (to != Vector2Int.zero && to != characterPresenter.status.position.GetVector2Int())
                {
                    var distance = GetFirstPositionAStar(characterPresenter.status.position.GetVector2Int(), to, mapPresenter, characterPresenter);
                    axis = new InputAxis(distance - characterPresenter.status.position.GetVector2Int());
                }
                sw.Stop();
                Debug.Log(characterPresenter.status.guid+",from:"+characterPresenter.status.position.GetVector2Int()+",to:"+to+",time:"+sw.ElapsedMilliseconds);
                
                if (axis.I == new Vector2Int(0, 0))
                {
                    //移動先がなければ行動済みにする。
                    characterPresenter.SetIsAction(true);
                    continue;
                }

                if (!mapPresenter.IsCanMove(axis.I, characterPresenter.status.position.GetVector2Int(), characterPresenter.status.type))
                {                    
                    // TODO 移動先に邪魔なものがあれば縦か横移動をする。
                    characterPresenter.SetIsAction(true);
                    continue;
                }

                Vector2Int beforePosition = new Vector2Int((int) characterPresenter.status.position.x, (int) characterPresenter.status.position.z);
                Vector2Int afterPosition = new Vector2Int(beforePosition.x + axis.I.x, beforePosition.y + axis.I.y);

                characterPresenter.Move(axis.F.x, axis.F.y);
                
                //移動元と移動先にキャラクター情報を設定
                mapPresenter.SetUserModel(beforePosition, null);
                mapPresenter.SetUserModel(afterPosition, characterPresenter.status);
                
                characterPresenter.SetMapData(
                    mapPresenter.GetTileModel(afterPosition).floorId,
                    new Vector3(afterPosition.x, 0, afterPosition.y),
                    new Vector3(axis.I.x, 0, axis.I.y)
                );
                
            }else
            {
                //GetNearCharacterPosition(GameConfig.SearchType.Around8, TileModel.CharaType.Player, mapPresenter.map);
                characterPresenter.SetIsAction(true);
            }
        }
    }
    
    private List<Vector2Int> _directions = new List<Vector2Int>
    {
        new Vector2Int(-1, -1),
        new Vector2Int(-1, 0),
        new Vector2Int(-1, 1),
        new Vector2Int(0,  -1),
//        new List<Vector3> {0, 0, 0},
        new Vector2Int(0,  1),
        new Vector2Int(1,  -1),
        new Vector2Int(1,  0),
        new Vector2Int(1,  1)
    };

    private class AStarCost
    {
        public Vector2Int position = new Vector2Int(0,0);
        public int status = 1; // 1=open,2=close
        public int cost = 0;
        public int estimateCost = 0;
        public int score = 0;
        public int distance = 0;
    }
    
    private Vector2Int GetFirstPositionAStar(Vector2Int from, Vector2Int to, MapPresenter mapPresenter, CharacterPresenter characterPresenter)
    {
        AStarCost nowNode = new AStarCost();       
        
        nowNode.position = from;
        
        nowNode.cost = 0;
        nowNode.estimateCost = EstimateCost(nowNode.position, to);
        nowNode.score = nowNode.cost + nowNode.estimateCost;

        var cacheAStarCostList = new List<AStarCost>(){nowNode};
        var routeAStarList = new List<AStarCost>();
        
        GetPositionAStar(nowNode, to, ref routeAStarList, ref cacheAStarCostList, mapPresenter, characterPresenter);
        if (routeAStarList.Count > 0)
        {
            routeAStarList.Reverse();
            return routeAStarList.First().position;
        }
        return new Vector2Int(0, 0);
    }

    private bool GetPositionAStar(AStarCost aStar, Vector2Int to, ref List<AStarCost> routeAStarList, ref List<AStarCost> cacheAStarCostList, MapPresenter mapPresenter, CharacterPresenter characterPresenter)
    {
        List<AStarCost> aroundAStarList = new List<AStarCost>();
        foreach (Vector2Int direction in _directions)
        {
            var p = aStar.position + direction;
            //cacheにあればOpenしない
            if (cacheAStarCostList.Any(aster => aster.position == p) || !mapPresenter.IsCanMoveAStar(direction, aStar.position, characterPresenter.status.type))
            {
                continue;
            }
            
            AStarCost nowNode = new AStarCost
            {
                position = p,
                cost = aStar.cost + 1
            };
            nowNode.estimateCost = EstimateCost(nowNode.position, to);
            nowNode.distance = DistanceCost(nowNode.position, to);
            nowNode.score = nowNode.cost + nowNode.estimateCost;    
                        
            cacheAStarCostList.Add(nowNode);
            aroundAStarList.Add(nowNode);
            //目的地についたら
            if (nowNode.position == to)
            {
                routeAStarList.Add(nowNode);
                return true;
            }
        }
        
        if (aroundAStarList.Count < 1)
        {
            return false;
        }
        
        cacheAStarCostList.First(_ => _.position == aStar.position).status = 2;
        
        var nextAroundAStar = aroundAStarList.OrderBy(a => a.score).ThenBy(a => a.distance);

        foreach (var nextAStar in nextAroundAStar)
        {
            if (GetPositionAStar(nextAStar, to, ref routeAStarList, ref cacheAStarCostList, mapPresenter,
                characterPresenter))
            {
                routeAStarList.Add(nextAStar);
                return true;
            }
        }
        return false;
    }

    public int EstimateCost(Vector2Int from, Vector2Int to)
    {        
        var ecX = Mathf.Abs((int) to.x - (int) from.x);
        var ecY = Mathf.Abs((int) to.y - (int) from.y);

        return ecX > ecY ? ecX : ecY;
    }
    
    public int DistanceCost(Vector2Int from, Vector2Int to)
    {        
        var ecX = Mathf.Abs((int) to.x - (int) from.x);
        var ecY = Mathf.Abs((int) to.y - (int) from.y);

        return ecX + ecY;
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
	
    public void Delete()
    {
        // hp0以下を削除
        var deleteCharacterListPresenter = characterListPresenter.Where(p => p.Value.status.hp < 0).ToArray();
        foreach (var character in deleteCharacterListPresenter)
        {
            character.Value.Death();
            int guid = character.Value.status.guid;
            Destroy(characterListObject[guid]);
            characterListPresenter.Remove(guid);
        }
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

    public CharacterPresenter GetOwnCharacterPresenter()
    {
        return characterListPresenter.FirstOrDefault(presenter => presenter.Value.status.isOwn).Value;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="search">サーチ範囲 1=周囲8マス</param>
    public void GetNearCharacterPosition(GameConfig.SearchType searchType, TileModel.CharaType type, TileModel[,] map)
    {
        if (searchType == GameConfig.SearchType.Around8)
        {
            
        }
    }
}
