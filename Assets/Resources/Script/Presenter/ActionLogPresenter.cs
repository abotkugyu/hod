using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class ActionLogPresenter : GameSingleton<ActionLogPresenter>
{    
    public ActionLogView actionLogView;
    public List<String> logs { get; } = new List<string>();
    
    static string[] LogFormat =
    {
        "{0}の攻撃、{1}に{2}のダメージ",
    };    
    protected override void OnCreated()
    {
        GameObject res = Resources.Load("Object/Window/ActionLog") as GameObject;
        GameObject obj = Object.Instantiate(res, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        actionLogView = obj.GetComponent<ActionLogView>();
    }
    
    public void AddLog(LogType type, string[] args)
    {
        lock(logs){
            logs.Add(String.Format(LogFormat[(int) type], args));
        }
        Refresh();
    }
    public void SwitchShowView()
    {        
        if (!actionLogView.view.activeSelf)
        {
            actionLogView.Show();
        }
        else
        {
            actionLogView.Hide();
        }
    }
    
    public void ShowView()
    {
        actionLogView.Show();
    }
    public void Refresh()
    {
        lock (logs)
        {
            actionLogView.Refresh(logs);
        }
    }
}
