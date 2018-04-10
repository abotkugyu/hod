using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		float x = Input.GetAxisRaw ("Horizontal");
		float y = Input.GetAxisRaw ("Vertical");
		Vector2 direction = new Vector2 (x, y).normalized;

		// 移動する向きとスピードを代入する
		GetComponent<Rigidbody2D>().velocity = direction;

	}
}
