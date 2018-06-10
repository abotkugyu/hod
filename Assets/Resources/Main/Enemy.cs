using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	// Use this for initialization
	void Start () {
  
	}

    public float speed = 4.0f;
    public bool is_move = false;
    public Vector3 target_position;
	// Update is called once per frame

	void Update () {
        if (is_move){
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
        }	
	}

    void move(float x, float z) {
        is_move = true;
        Rigidbody transform = GetComponent<Rigidbody>();
        Vector3 now_position = transform.position;

        Rigidbody rigidbody = GetComponent<Rigidbody>();
        rigidbody.AddForce(x, 0, z, ForceMode.Acceleration);
        target_position.x = now_position.x;
        target_position.z = now_position.z;

    }
}
