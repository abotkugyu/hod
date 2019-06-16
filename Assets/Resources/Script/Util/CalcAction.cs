using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalcAction{
    /// <summary>
    /// ダメージ計算
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    public static int CalcAttack(UserModel from, UserModel to)
    {
        int damage = from.attack - to.defence;
        if (damage < 0)
        {
            damage = 0;
        }
        return damage;
    }

}
