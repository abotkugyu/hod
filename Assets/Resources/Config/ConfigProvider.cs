using UnityEngine;
using System.Collections;

public static class ConfigProvider
{
	private const string path = "Resources/Utilities/ConfigProvider";

	private static ConfigComponent _confgiComponent;

	private static ConfigComponent ConfigComponent
	{
		get
		{
			//ConfigComponentが存在しないなら新しく生成する
			if (_confgiComponent != null) return _confgiComponent;
			if (ConfigComponent.Instance == null)
			{
				var resource = Resources.Load(path);
				Object.Instantiate(resource);
			}
			_confgiComponent = ConfigComponent.Instance;
			return _confgiComponent;
		}
	}

	/// <summary>
	/// APIサーバ設定
	/// </summary>
	public static ApiConfig Api
	{
		get { return ConfigComponent.Config.ApiConfig; }
	}
}