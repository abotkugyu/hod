using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ApplicationConfigs : ScriptableObject
{
	public ApiConfig ApiConfig;
}

[System.Serializable]
public class ApiConfig
{
	public int max_map_x = 1000;
	public int max_map_y = 1000;
}
