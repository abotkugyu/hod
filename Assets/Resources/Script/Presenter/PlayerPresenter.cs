using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerPresenter : MonoBehaviour {

    // Use this for initialization
    public UserModel status = new UserModel();
    
    public float speed = 4.0f;
    public bool isMove = false;
    public Vector3 targetPosition;
    
    [SerializeField]
    public PlayerView playerView;
    
    public List<ItemModel> itemModels;

    void Update () {
        if (isMove && status.isAction == false){
            Moving();
        }
    }
    
    public void Initialize()
    {
        playerView.Initialize();
        DummyItem();
    }
    
    public void DummyItem()
    {        
        itemModels.Add(ItemData.GetRandom());
    }
    
    public void DummyInitialize(List<int> pos)
    {        
        int posx = pos[0];
        int posz = pos[1];
        playerView.player.transform.position = new Vector3(posx, 0, posz);
        SetPosition(new Vector3(posx, 0, posz));
        SetDirection(new Vector3(0, 0, -1));
    }

    //攻撃処理
    public void Attack(MapPresenter m,EnemyListPresenter e)
    {
        Vector3 pos = status.position + status.direction;
        if (pos.x < 0 || pos.z < 0){
            return;
        }
        if (m.map[(int)pos.x, (int)pos.z].charaType != TileModel.CharaType.Player){
            Debug.Log("c_type:" + m.map[(int)pos.x, (int)pos.z].charaType);
            Debug.Log("c_id:" + m.map[(int)pos.x, (int)pos.z].charaId);
            for (int x = 0; x < e.enemyList.Count; x++) {
                EnemyPresenter enemyPresenter = e.enemyList[x].GetComponent<EnemyPresenter>();
                Debug.Log("attack:e_id=" + enemyPresenter.status.id);
                if (enemyPresenter.status.id == m.map[(int)pos.x, (int)pos.z].charaId){
                    Debug.Log("delete:id="+enemyPresenter.status.id);
                    enemyPresenter.status.hp = 0;
                    e.Delete(x);
                    break;
                }
            }
        }
        status.isAction = true;

        playerView.Attack();
    }

    /// <summary>
	/// Force	その質量を使用して、rigidbodyへの継続的な力を追加します。
	/// Acceleration	その質量を無視して、rigidbodyへの継続的な加速を追加します。
	/// Impulse	その質量を使用して、rigidbodyに瞬時に速度変化を追加します。
	/// VelocityChange	その質量を無視して、rigidbodyに瞬時に速度変化を追加します。
    /// transform.AddForce(x, 0, z, ForceMode.Acceleration);
    /// </summary>
    /// <param name="x"></param>
    /// <param name="z"></param>
	public void Move (float x,float z) {
        isMove = true;
		Rigidbody transform = playerView.GetTransForm();
		Vector3 nowPosition = transform.position;

        targetPosition.x = x/200 + nowPosition.x;
        targetPosition.z = z/200 + nowPosition.z;
        
        playerView.Run(true);

	}

    void Moving(){
        Rigidbody transform = playerView.GetTransForm();
        Vector3 nowPosition = transform.position;

        float step = 0.04f;//speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(targetPosition.x, 0, targetPosition.z), step);
        if (targetPosition.x == nowPosition.x && targetPosition.z == nowPosition.z)
        {
            transform.velocity = Vector3.zero;
            isMove = false;
            status.isAction = true;
            playerView.Run(false);
        }
    }

    /// user.status.position に座標保存
    public Vector3 SetPosition(Vector3 position)
    {
        status.position = new Vector3(status.position.x + position.x, status.position.y + position.y, status.position.z + position.z);
        return status.position;
    }

    /// <summary>
    /// <code>
    /// transform.eulerAnglesで向きを変更
    /// user.status.directionに向き保存
    /// </code>
    /// </summary>
    public void SetDirection(Vector3 position)
    {
        int nowDirection = TransFormUtil.GetChangeRotate(status.direction);
        int targetDirection = TransFormUtil.GetChangeRotate(position);

        float angle = Mathf.LerpAngle(nowDirection, targetDirection, Time.time);
        
        playerView.SetAngles(new Vector3(0, angle, 0));

        status.direction = new Vector3(position.x, position.y, position.z);
    }
    
    public bool SetItem(ItemModel itemModel)
    {
        if (GameConfig.ItemMaxLimit <= itemModels.Count)
        {
            Debug.Log("NOTICE : Over Has Item Limit");
            return false;
        }
            
        itemModels.Add(itemModel);
        return true;
    }
}
