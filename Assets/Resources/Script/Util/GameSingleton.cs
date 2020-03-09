using System;
using UnityEngine;
public class GameSingleton<T> : MonoBehaviour, IDisposable where T : GameSingleton<T>
{
    private static T _instance;

    public static T Instance {
        get
        {
            return _instance;
        }
    }

    public static IDisposable CreateInstance()
    {
        if (_instance != null) {
            return _instance;
        }

        GameObject container = new GameObject(typeof(T).Name);
        _instance = container.AddComponent<T>();

        _instance.OnCreated();

        if (Application.isPlaying) {
            DontDestroyOnLoad(_instance.gameObject);
        }

        return _instance;
    }

    private void DeleteInstance()
    {
        if (_instance == null) {
            return;
        }

        _instance.OnDisposed();

        Destroy(_instance);
        _instance = null;

    }
    public void Dispose()
    {
        DeleteInstance();
    }

    private void OnDestroy()
    {            
        Dispose();
    }

    protected virtual void OnCreated()
    {
    }

    protected virtual void OnDisposed()
    {
    }

    public static bool HasInstance {
        get { return (_instance != null); }
    }
}