using UnityEngine;
using System.Collections;

public class SingletonMonoBehaviour<T> : MonoBehaviour where T : SingletonMonoBehaviour<T>
{
	protected static T instance;
	public static T Instance {
		get {
			if (instance == null) {
				instance = (T)FindObjectOfType (typeof(T));

				if (instance == null) {
					Debug.LogWarning (typeof(T) + "is nothing");
				}
			}

			return instance;
		}
	}

	protected void Awake()
	{
		CheckInstance();
	}

	protected bool CheckInstance()
	{
		if( instance == null)
		{
			instance = (T)this;
			return true;
		}else if( Instance == this )
		{
			return true;
		}

		Destroy(this);
		return false;
	}
}

public class ConfigComponent : SingletonMonoBehaviour<ConfigComponent>
{
	private readonly string basePath = "Config/";

	[SerializeField] private ConfigEnvironment targetEnv = ConfigEnvironment.Development;
	private ApplicationConfigs config;

	void Awake()
	{
		//シーンをまたいでも消さない
		DontDestroyOnLoad(gameObject);
	}

	/// <summary>
	/// Conf値
	/// </summary>
	public ApplicationConfigs Config
	{
		//configがnullならロードしてキャッシュする
		get { return config ?? (config = LoadConfig()); }
	}

	/// <summary>
	/// 環境別設定値読み込み
	/// </summary>
	/// <returns></returns>
	private ApplicationConfigs LoadConfig()
	{
		// 愚直にswitchで
		// 他にもっといい方法あるかも
		switch (targetEnv)
		{
		case ConfigEnvironment.Development:
			Debug.Log (basePath);
			return Resources.Load<ApplicationConfigs> (basePath + "Development");
		case ConfigEnvironment.Staging:
			Debug.Log("Load 'Staging' conf");
			return Resources.Load<ApplicationConfigs>(basePath + "Staging");
		case ConfigEnvironment.Production:
			return Resources.Load<ApplicationConfigs>(basePath + "Production");
		default:
			return null;
			//throw new ArgumentOutOfRangeException();
		}
	}
}