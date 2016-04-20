using UnityEngine;
using System.Collections;

public class CEnemy : MonoBehaviour {

	public int pool_index { get; private set; }

	public void update_pool_index(int pool_index)
	{
		this.pool_index = pool_index;
	}


	public void fallingdown()
	{
	}
}
