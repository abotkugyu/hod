using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StatusConfig
{
    public int hp;
    public int mp;
    //１ターンの行動回数
    public int speed;
    //行動順
    public int speed_priority;
    //ability
    public int ability;
    //行動パターン
    public int[] action_pattern;
    //種族
    public int type;
    //名前
    public string name;
    //攻撃力
    public int attack;
    //防御力
    public int defence;
    //命中力
    public int base_hit_rate;
    //回避力
    public int base_avoid_rate;
    //
}
