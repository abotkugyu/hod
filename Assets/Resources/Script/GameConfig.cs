using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GameConfig : MonoBehaviour {
    /// item最大所持数
    public static int ItemMaxLimit = 20;
    
    public enum SearchType
    {
        Around4 = 1,        
        Around8 = 2,
        Around12 = 3,
        Around16 = 4,
        AroundFloor = 5,
        AroundMap = 6,
    }

    public enum Around4Type
    {
        Up = 1,
        Down = 2,
        Left = 3,
        Right = 4
    }
}
