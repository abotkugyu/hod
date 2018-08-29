using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	// Use this for initialization

    public UserStatus status;
    void Start()
    {
        status = new UserStatus();
    }

    public float speed = 4.0f;
    public bool is_move = false;
    public Vector3 target_position;

    public void action (){
        float x = Mathf.Sign(Random.Range(-1.0f, 1.0f)) * 200;
        float z = Mathf.Sign(Random.Range(-1.0f, 1.0f)) * 200;
        is_move = true;
        move(x, z);
    }

    // Update is called once per frame
    void Update()
    {
        if (is_move && status.is_action == false)
        {
            moving();
        }
    }

    //移動処理
    void move(float x, float z)
    {
        Rigidbody transform = GetComponent<Rigidbody>();
        Vector3 now_position = transform.position;

        Rigidbody rigidbody = GetComponent<Rigidbody>();
        //Force その質量を使用して、rigidbodyへの継続的な力を追加します。
        //Acceleration  その質量を無視して、rigidbodyへの継続的な加速を追加します。
        //Impulse   その質量を使用して、rigidbodyに瞬時に速度変化を追加します。
        //VelocityChange    その質量を無視して、rigidbodyに瞬時に速度変化を追加します。
        rigidbody.AddForce(x, 0, z, ForceMode.Acceleration);
        target_position.x = now_position.x;
        target_position.z = now_position.z;
        Debug.Log(target_position.x);
    }

    void moving()
    {
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
}
