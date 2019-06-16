using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UserModel
{
    
    public UserModel(UserModel model)
    {
        id = model.id;
        maxHp = model.hp;
        maxMp = model.mp;
        maxEp = model.ep;
        hp = model.hp;
        ep = model.ep;
        mp = model.mp;
        type = model.type;
        speed = model.speed;
        attack = model.attack;
        defence = model.defence;
        position = model.position;
        direction = model.direction;
        element = model.element;
        speedPriority = model.speedPriority;
        ability = model.ability;
        blood = model.blood;
        hitRate = model.hitRate;
        avoidRate = model.avoidRate;
        jump = model.jump;
        floorId = model.floorId;
    }
    
    public int guid = 0;
    //id
    public int id = 0;

    public int level = 1;

    public int exp = 0;
    
    //type 1=player 2=enemy
    public TileModel.CharaType type = 0;
    //全体マップの分割番部屋番号
    public int floorId = 0;
    
    //座標
    public Vector3 position = new Vector3(0, 0, 0);
    //向き(デフォルト下向き)
    public Vector3 direction = new Vector3(0, 0, -1);
    public int maxHp = 50;
    public int maxMp = 40;
    public int maxEp = 30;
    public int hp = 50;
    public int mp = 40;
    public int ep = 30;
    //１ターンの行動回数
    public int speed;
    //行動順
    public int speedPriority;
    //ability
    public int ability;
    //行動パターン
    public int[] actionPattern;
    //種族
    public int blood;
    //名前
    public string name;
    //攻撃力
    public int attack = 110;
    //防御力
    public int defence = 50;
    //命中力
    public int hitRate;
    //回避力
    public int avoidRate;
    //属性
    public int element;
    //jump
    public int jump;
    //行動したか
    public bool isAction = false;
}
