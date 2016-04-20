using System;
using System.Collections;
using System.Collections.Generic;

public class CLevelData 
{
	public int goal { get; private set; }
	public float speed_ratio { get; private set; }
	public float gap { get; private set; }

	public CLevelData(int goal, float speed_ratio, float gap)
	{
		this.goal = goal;
		this.speed_ratio = speed_ratio;
		this.gap = gap;
	}
}
