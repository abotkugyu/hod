using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObject : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		var conf = ConfigProvider.Api;
		Debug.Log(conf);
		//NCMBSettings.ApplicationKey = conf.ApplicationKey;
		//NCMBSettings.ClientKey = conf.ClientKey;
	}
}
