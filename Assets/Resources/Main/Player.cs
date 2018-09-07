using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    // Use this for initialization
    public UserStatus status = new UserStatus();
    void Start () {
        set_hp(status.hp);
        set_mp(status.mp);
        set_ep(status.ep);
	}
   
	public float speed = 4.0f;
	public bool is_move = false;
	public Vector3 target_position;

	// Update is called once per frame
	void Update () {
        if (is_move && status.is_action == false){
            moving();
		}
	}

    //攻撃処理
    public void attack(GameMap m,EnemyControll e)
    {
        Vector3 pos = status.position + status.direction;
        if (pos.x < 0 || pos.z < 0){
            return;
        }
        if (m.map[(int)pos.x, (int)pos.z].chara_type != 1){
            for (int x = 0; x < e.enemy_list.Count; x++) {
                if (e.enemy_list[x].status.id == m.map[(int)pos.x, (int)pos.z].chara_id){
                    e.enemy_list[x].status.hp = 0;
                    Destroy(e.enemy_list[x]);
                }
            }
            //GameObject obj = GameObject.FindWithTag();
            //Destroy(obj);
            //GameObject obj = GameObjecta.Find("Player");
            //find
        }
        status.is_action = true;
    }

    //移動処理
	public void move (float x,float z) {
        is_move = true;
		Rigidbody transform = GetComponent<Rigidbody>();
		Vector3 now_position = transform.position;
		 
		Rigidbody rigidbody = GetComponent<Rigidbody>();
		//Force	その質量を使用して、rigidbodyへの継続的な力を追加します。
		//Acceleration	その質量を無視して、rigidbodyへの継続的な加速を追加します。
		//Impulse	その質量を使用して、rigidbodyに瞬時に速度変化を追加します。
		//VelocityChange	その質量を無視して、rigidbodyに瞬時に速度変化を追加します。
		rigidbody.AddForce(x, 0, z, ForceMode.Acceleration);
        target_position.x = now_position.x;
        target_position.z = now_position.z;
	}

    void moving(){
        Rigidbody transform = GetComponent<Rigidbody>();
        Vector3 now_position = transform.position;
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        if (target_position.x != now_position.x)
        {
            if (target_position.x + 1 < now_position.x)
            {
                rigidbody.velocity = Vector3.zero;
                is_move = false;
                status.is_action = true;
            }
            else if (target_position.x - 1 > now_position.x)
            {
                rigidbody.velocity = Vector3.zero;
                is_move = false;
                status.is_action = true;
            }
        }
        else if (target_position.z != now_position.z)
        {
            if (target_position.z + 1 < now_position.z)
            {
                rigidbody.velocity = Vector3.zero;
                is_move = false;
                status.is_action = true;
            }
            else if (target_position.z - 1 > now_position.z)
            {
                rigidbody.velocity = Vector3.zero;
                is_move = false;
                status.is_action = true;
            }
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

    //position設定
    public void set_position(Vector3 position)
    {
        status.position = new Vector3(status.position.x + position.x, status.position.y + position.y, status.position.z + position.z);
    }

    //向き設定
    public void set_direction(Vector3 position)
    {
        status.direction = new Vector3(position.x, position.y, position.z);
    }


}
