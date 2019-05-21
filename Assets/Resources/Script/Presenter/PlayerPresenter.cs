using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerPresenter : MonoBehaviour {

    // Use this for initialization
    public UserModel status = new UserModel();
    
    public float speed = 4.0f;
    public bool is_move = false;
    public Vector3 target_position;
    
    [SerializeField]
    public PlayerView playerView;
    
    public List<ItemModel> itemModels;

    public void initialize()
    {
        playerView.initialize();
        dummy_item();
    }
    
    public void dummy_item()
    {        
        itemModels.Add(ItemData.GetRandom());
    }
    
    public void dummy_initialize(List<int> pos)
    {        
        int posx = pos[0];
        int posz = pos[1];
        playerView.player.transform.position = new Vector3(posx, 0, posz);
        set_position(new Vector3(posx, 0, posz));
        set_direction(new Vector3(0, 0, -1));
    }
    
	// Update is called once per frame
	void Update () {
        if (is_move && status.is_action == false){
            moving();
        }
	}

    //攻撃処理
    public void attack(MapPresenter m,EnemyListPresenter e)
    {
        Vector3 pos = status.position + status.direction;
        if (pos.x < 0 || pos.z < 0){
            return;
        }
        if (m.map[(int)pos.x, (int)pos.z].chara_type != 1){
            Debug.Log("c_type:" + m.map[(int)pos.x, (int)pos.z].chara_type);
            Debug.Log("c_id:" + m.map[(int)pos.x, (int)pos.z].chara_id);
            for (int x = 0; x < e.enemy_list.Count; x++) {
                EnemyPresenter com = e.enemy_list[x].GetComponent<EnemyPresenter>();
                Debug.Log("attack:e_id=" + com.status.id);
                if (com.status.id == m.map[(int)pos.x, (int)pos.z].chara_id){
                    Debug.Log("delete:id="+com.status.id);
                    com.status.hp = 0;
                    e.delete(x);
                    break;
                }
            }
        }
        status.is_action = true;

        playerView.attack();
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
	public void move (float x,float z) {
        is_move = true;
		Rigidbody transform = playerView.GetTransForm();
		Vector3 now_position = transform.position;

        target_position.x = x/200 + now_position.x;
        target_position.z = z/200 + now_position.z;
        
        playerView.run(true);

	}

    void moving(){
        Rigidbody transform = playerView.GetTransForm();
        Vector3 now_position = transform.position;

        float step = 0.04f;//speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(target_position.x, 0, target_position.z), step);
        if (target_position.x == now_position.x && target_position.z == now_position.z)
        {
            transform.velocity = Vector3.zero;
            is_move = false;
            status.is_action = true;
            playerView.run(false);
        }
    }

    /// user.status.position に座標保存
    public Vector3 set_position(Vector3 position)
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
    public void set_direction(Vector3 position)
    {
        int n_direction = TransFormUtil.GetChangeRotate(status.direction);
        int s_direction = TransFormUtil.GetChangeRotate(position);

        float angle = Mathf.LerpAngle(n_direction, s_direction, Time.time);
        
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
