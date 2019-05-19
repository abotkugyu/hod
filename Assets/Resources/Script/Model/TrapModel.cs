using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
//item情報
public class TrapModel
{
    //1:罠 type
    public int type = 0;
    //効果値
    public int value = 0;
    //座標
    public Vector3 position = new Vector3(0, 0, 0);
}
