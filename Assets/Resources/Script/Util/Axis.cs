using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public struct InputAxis 
{
    public InputAxis(int iX, int iY, float fX, float fY)
    {
        this.I = new Vector2Int(iX,iY);
        this.F = new Vector2(fX,fY);
    }
    
    /// <summary>
    ///   <para>X component of the vector.</para>
    /// </summary>
    public Vector2Int I { get; set; }

    /// <summary>
    ///   <para>Y component of the vector.</para>
    /// </summary>
    public Vector2 F { get; set; }
        
    public static InputAxis GetInputAxis()
    {         
        float x = Input.GetAxisRaw("Horizontal") * 200;
        float z = Input.GetAxisRaw("Vertical") * 200;
        //方向入力
        int xA = (x != 0 ? (int) Mathf.Sign(x) : 0);
        int zA = (z != 0 ? (int) Mathf.Sign(z) : 0); 
        
        return new InputAxis(xA ,zA, x, z);
    }
        
    public static InputAxis GetRandomAxis()
    {         
        float x = Mathf.Sign(Random.Range(-1.0f, 1.0f)) * 200;
        float z = Mathf.Sign(Random.Range(-1.0f, 1.0f)) * 200;
        int xA = (x != 0 ? (int)Mathf.Sign(x) : 0);
        int zA = (z != 0 ? (int)Mathf.Sign(z) : 0);
        
        return new InputAxis(xA ,zA, x, z);
    }
}
