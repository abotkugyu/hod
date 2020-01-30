using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DirectionUtil
{        
    public List<Vector2Int> GetAroundDirection(int distance)
    {
        List<Vector2Int> directions = new List<Vector2Int>();
        for (var x = -distance;x <= distance; x++)
        {
            for (var y = -distance; y <= distance; y++)
            {
                if (x == 0 && y == 0) continue;
                directions.Add(new Vector2Int(x, y));
            }
        }

        return directions.OrderBy(_ => Mathf.Abs(_.x) >= Mathf.Abs(_.y) ? Mathf.Abs(_.x) : Mathf.Abs(_.y)).ToList();
    }
}
