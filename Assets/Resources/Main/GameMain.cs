using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMain : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GameMap map = GetComponent<GameMap>();
		map.create ();
	}
	
	// Update is called once per frame
	void Update () {
		var conf = ConfigProvider.Api;
	}
		
}
