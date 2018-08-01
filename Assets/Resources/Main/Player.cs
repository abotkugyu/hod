using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	// Use this for initialization
	void Start () {
        
	}
   
	public float speed = 4.0f;
	public bool is_move = false;
	public Vector3 target_position;

	// Update is called once per frame
	void Update () {

		float x = Input.GetAxisRaw ("Horizontal")*200;
		float z = Input.GetAxisRaw ("Vertical")*200;
		//Time.time - last_key_pressed > 1.00f && 

		if (!is_move && (x != 0 || z != 0)) {
			is_move = true;
			move (x, z);
		} else {
            moving();
		}
	}

    //移動処理
	void move (float x,float z) {
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
            }
            else if (target_position.x - 1 > now_position.x)
            {
                rigidbody.velocity = Vector3.zero;
                is_move = false;
            }
        }
        else if (target_position.z != now_position.z)
        {
            if (target_position.z + 1 < now_position.z)
            {
                rigidbody.velocity = Vector3.zero;
                is_move = false;
            }
            else if (target_position.z - 1 > now_position.z)
            {
                rigidbody.velocity = Vector3.zero;
                is_move = false;
            }
        }

        use_hp(50);
        use_mp(40);
        use_ep(20);
    }

    //ダメージ計算
    void use_hp(int value)
    {
        Hud hud = (GameObject.Find("Hud")).GetComponent<Hud>();
        hud.update_health(value);
    }
    //mp計算
    void use_mp(int value)
    {
        Hud hud = (GameObject.Find("Hud")).GetComponent<Hud>();
        hud.update_magic(value);
    }
    //ep計算
    void use_ep(int value)
    {
        Hud hud = (GameObject.Find("Hud")).GetComponent<Hud>();
        hud.update_energy(value);
    }
}
