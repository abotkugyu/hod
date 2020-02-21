using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LogType
{
    AttackLog = 0
}

public class ActionLogModel
{
    
    public static ActionLogModel Instance { get; } = new ActionLogModel();
    public List<String> logs { get; }

    static string[] LogFormat =
    {
        "{0}の攻撃、{1}に{2}のダメージ",
    }; 

    private ActionLogModel()
    {
        logs = new List<string>();
    }
    
    public void AddLog(LogType type, string[] args)
    {
        lock(logs){
            logs.Add(String.Format(LogFormat[(int) type], args));
        }
    }
}
