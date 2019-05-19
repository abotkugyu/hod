using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransFormUtil{
	public static int GetChangeRotate(Vector3 d){
        
		int rotate = 0;
		if (d.x == -1)
		{
			rotate += 180;
			if (d.z == 1)
			{
				rotate += 135;
			}
			else if (d.z == 0)
			{
				rotate += 90;
			}
			else if (d.z == -1)
			{
				rotate += 45;
			}
		}
		else if (d.x == 0)
		{
			if (d.z == -1)
			{
				rotate += 180;
			}
		}
		else if (d.x == 1)
		{
			if (d.z == 1)
			{
				rotate += 45;
			}
			else if (d.z == 0)
			{
				rotate += 90;
			}
			else if (d.z == -1)
			{
				rotate += 135;
			}
		}
		return rotate;
	}
}
