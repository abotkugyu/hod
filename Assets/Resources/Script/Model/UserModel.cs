using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UserModel
{
    //id
    public int id = 0;
    //type 1=player 2=enemy
    public int type = 1;
    //座標
    public Vector3 position = new Vector3(0, 0, 0);
    //向き(デフォルト下向き)
    public Vector3 direction = new Vector3(0, 0, -1);
    public int hp = 50;
    public int mp = 40;
    public int ep = 30;
    //１ターンの行動回数
    public int speed;
    //行動順
    public int speed_priority;
    //ability
    public int ability;
    //行動パターン
    public int[] action_pattern;
    //種族
    public int blood;
    //名前
    public string name;
    //攻撃力
    public int attack = 100;
    //防御力
    public int defence = 100;
    //命中力
    public int hit_rate;
    //回避力
    public int avoid_rate;
    //属性
    public int element;
    //jump
    public int jump;
    //行動したか
    public bool is_action = false;
}
