using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMain : MonoBehaviour {

	// Use this for initialization
	void Start () {
        

	}
    public int phase = 2;
    public bool is_create_map = false;
	// Update is called once per frame
	void Update () {
        
		//var conf = ConfigProvider.Api;

        //rest_map=1,dungen_map=1
        if (phase == 1) {
            
        } else if (phase == 2) {
            

        }
        if (is_create_map == false)
        {
            GameMap map = GetComponent<GameMap>();
            map.create();
            is_create_map = !is_create_map;
        }
	}
		
}
