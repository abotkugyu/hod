using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour {

    // Use this for initialization
    public UserModel status = new UserModel();
    Animator _animator;
    
    public float speed = 4.0f;
    public bool is_move = false;
    public Vector3 target_position;
    
    //所持アイテム
    public List<Item> items = new List<Item>();
    
    void Start () {
        _animator = GetComponent<Animator>();
        _animator.Play("Wait");
        
        Item item = new Item();
        item.generate();
        for (int x = 0; x < 10; x++)
        {
            item.re_random();
            items.Add(item);
        }

    }

	// Update is called once per frame
	void Update () {
        if (is_move && status.is_action == false){
            moving();
        }
	}

    //攻撃処理
    public void attack(GameMap m,EnemyController e)
    {
        Vector3 pos = status.position + status.direction;
        if (pos.x < 0 || pos.z < 0){
            return;
        }
        if (m.map[(int)pos.x, (int)pos.z].chara_type != 1){
            Debug.Log("c_type:" + m.map[(int)pos.x, (int)pos.z].chara_type);
            Debug.Log("c_id:" + m.map[(int)pos.x, (int)pos.z].chara_id);
            for (int x = 0; x < e.enemy_list.Count; x++) {
                Enemy com = e.enemy_list[x].GetComponent<Enemy>();
                Debug.Log("attack:e_id=" + com.status.id);
                if (com.status.id == m.map[(int)pos.x, (int)pos.z].chara_id){
                    Debug.Log("delete:id="+com.status.id);
                    com.status.hp = 0;
                    e.delete(x);
                    break;
                }
            }
            //GameObject obj = GameObject.FindWithTag();
            //Destroy(obj);
            //GameObject obj = GameObjecta.Find("Player");
            //find
        }
        status.is_action = true;

        GetComponent<Animator>().SetTrigger("attack");
    }

    //移動処理
	public void move (float x,float z) {
        is_move = true;
		Rigidbody transform = this.GetComponent<Rigidbody>();
		Vector3 now_position = transform.position;

        target_position.x = x/200 + now_position.x;
        target_position.z = z/200 + now_position.z;

		//Force	その質量を使用して、rigidbodyへの継続的な力を追加します。
		//Acceleration	その質量を無視して、rigidbodyへの継続的な加速を追加します。
		//Impulse	その質量を使用して、rigidbodyに瞬時に速度変化を追加します。
		//VelocityChange	その質量を無視して、rigidbodyに瞬時に速度変化を追加します。
        //transform.AddForce(x, 0, z, ForceMode.Acceleration);
        
        _animator.SetBool("is_run", true);

	}

    void moving(){
        Rigidbody transform = this.GetComponent<Rigidbody>();
        Vector3 now_position = transform.position;

        float step = 0.04f;//speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(target_position.x, 0, target_position.z), step);
        if (target_position.x == now_position.x && target_position.z == now_position.z)
        {
            transform.velocity = Vector3.zero;
            is_move = false;
            status.is_action = true;
            _animator.SetBool("is_run", false);
        }
    }


    //ダメージ計算
    void set_hp(int value)
    {
        Hud hud = (GameObject.Find("Hud")).GetComponent<Hud>();
        hud.update_health(value);
    }
    //mp計算
    void set_mp(int value)
    {
        Hud hud = (GameObject.Find("Hud")).GetComponent<Hud>();
        hud.update_magic(value);
    }
    //ep計算
    void set_ep(int value)
    {
        Hud hud = (GameObject.Find("Hud")).GetComponent<Hud>();
        hud.update_energy(value);
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
        int n_direction = get_rotate(status.direction);
        //Debug.Log(n_direction);
        int s_direction = get_rotate(position);
        //Debug.Log(s_direction);

        /*
        if(n_direction > s_direction){
           
            int rd = n_direction - s_direction;
            if(rd >= 180){
                this.transform.Rotate(0.0f, 360-rd, 0.0f);
            }else{
                this.transform.Rotate(0.0f, -rd, 0.0f);
            }
        }
        else if (s_direction > n_direction)
        {
            int rd = s_direction - n_direction;
            if (rd >= 180)
            {
                this.transform.Rotate(0.0f, -360+rd, 0.0f);
            }
            else
            {
                this.transform.Rotate(0.0f, rd, 0.0f);
            }

        }
        */
        float angle = Mathf.LerpAngle(n_direction, s_direction, Time.time);
        this.transform.eulerAngles = new Vector3(0, angle, 0);


        status.direction = new Vector3(position.x, position.y, position.z);
    }

    int get_rotate(Vector3 d){
        
        int rotate = 0;
        if (d.x == -1)
        {
            rotate += 180;
            if (d.z == 1)
            {
                rotate += 135;
            }
            else if (d.z == 0)
            {
                rotate += 90;
            }
            else if (d.z == -1)
            {
                rotate += 45;
            }
        }
        else if (d.x == 0)
        {
            if (d.z == -1)
            {
                rotate += 180;
            }
        }
        else if (d.x == 1)
        {
            if (d.z == 1)
            {
                rotate += 45;
            }
            else if (d.z == 0)
            {
                rotate += 90;
            }
            else if (d.z == -1)
            {
                rotate += 135;
            }
        }
        return rotate;
    }

}
