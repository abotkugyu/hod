using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ApplicationConfigs : ScriptableObject
{
	//例としてPhoto設定,サーバ設定、NCMBの設定を扱う
	public ApiConfig ApiConfig;

}

[System.Serializable]
public class ApiConfig
{
	public string BaseUrl = "test";
}
