using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMap : MonoBehaviour {

	public int x = 1000;
	public int y = 1000;
	public int[,] map;

	// Use this for initialization
	void Start () {
		map = new int[x,y];
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void create() {
		generate_map ();

		GameObject original = GameObject.Find("Tile");
		GameObject copied = Object.Instantiate(original) as GameObject;
		copied.transform.Translate(1, 0, 0);
		copied.transform.Translate(2, 0, 0);
	}

	void generate_map(){
		split_map(new List<int>{1000});

	}
	void split_map(List<int> data){
		Random.Range (0, 2);
		show_list_log (data);
		for (x = 0; x < data.Count; x++) {
			int is_split = Random.Range (0, 10);
			if (data [x] < 10) {
				continue;
			}
			if (is_split <= 8) {
				int ins = data [x] / 2;
				data [x] = data [x] / 2;
				data.Insert (x, ins);
				x--;
				continue;
			} 
		}
		show_list_log (data);
	}

	void show_list_log(List<int> data){
		string log = "";
		for (x = 0; x < data.Count; x++) {
			log += data[x].ToString ()+",";
		}
		Debug.Log (log);
	}
}
