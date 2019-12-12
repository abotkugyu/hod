using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
}
