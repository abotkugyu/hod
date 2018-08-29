using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMain : MonoBehaviour {

    // Use this for initialization
    public Player player;
    public EnemyControll enemies;
    public GameStatus status;
	void Start () {
        player = (GameObject.Find("Player")).GetComponent<Player>();
        enemies = new EnemyControll();
        enemies.generate();
        status = new GameStatus();
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
            if (status.turn == 1)
            {
                float x = Input.GetAxisRaw("Horizontal") * 200;
                float z = Input.GetAxisRaw("Vertical") * 200;
                if (!player.is_move && (x != 0 || z != 0) && player.status.is_action == false) {
                    player.move(x, z);
                    player.set_position(new Vector3((x != 0 ? Mathf.Sign(x) : 0), 0, (z != 0 ? Mathf.Sign(z) : 0)));
                }

                //自分が動いたら敵のターンにする
                if (player.status.is_action == true) {
                    enemies.turn_reset();
                    enemies.all_action();
                    status.turn = 2;
                }
            }
            else if (status.turn == 2)
            {
                //敵が全部動いていればユーザーのターンにする
                if(enemies.is_all_action() == true)
                {
                    player.status.is_action = false;
                    status.turn = 1;
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
