using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMain : MonoBehaviour {

    // Use this for initialization
    public Player my;
    public EnemyControll enemies;
	void Start () {
        my = (GameObject.Find("Player")).GetComponent<Player>();
        enemies = new EnemyControll();
        enemies.generate();
        Debug.Log("a");
	}
    public int phase = 2;
    public int turn = 1;
    public bool is_create_map = false;
    // Update is called once per frame
	void Update () {
        
		//var conf = ConfigProvider.Api;

        //rest_map=1,dungen_map=2
        if (phase == 1) {
            
        } else if (phase == 2) {
            //user_turn=1,enemy_turn=2
            if (turn == 1)
            {
                if (my.status.is_action == true) {
                    turn = 2;
                    enemies.turn_reset();
                    enemies.all_action();
                }
            }
            else if (turn == 2)
            {
                if(enemies.is_all_action() == true)
                {
                    turn = 1;
                    my.turn_reset();
                }
            }

        }
        if (is_create_map == false)
        {
            GameMap map = GetComponent<GameMap>();
            map.create();
            is_create_map = !is_create_map;
        }
	}
		
}
