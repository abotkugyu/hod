using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ExtensionMethods
{
    public static class VectorExtensions
    {
        public static Vector2Int AddX(this Vector2Int v2,int x)
        {
            return new Vector2Int(v2.x + x, v2.y);
        }
        
        public static Vector2Int AddY(this Vector2Int v2,int y)
        {
            return new Vector2Int(v2.x,v2.y + y);
        }
                
        public static Vector2Int AddXY(this Vector2Int v2,int x, int y)
        {
            return new Vector2Int(v2.x + x,v2.y + y);
        }
        public static Vector2Int GetVector2Int(this Vector3 v3)
        {
            return new Vector2Int((int)v3.x,(int)v3.z);
        }
        public static Vector3 GetVector2Int(this Vector2Int v2)
        {
            return new Vector3(v2.x,0,v2.y);
        }
    }
}