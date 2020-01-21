using System.Collections.Generic;
using UnityEngine;

public class DirectionUtil
{        
    public List<Vector2Int> GetAroundDirection(int distance)
    {
        List<Vector2Int> directions = new List<Vector2Int>();
        var n = 1;
        for (var x = -1;x <= n; x++)
        {
            for (var y = -1; y <= n; y++)
            {
                directions.Add(new Vector2Int(x, y));
            }
            if (x == n && n < distance)
            {
                n++;
            }
        }        
        return directions;
    }
}
