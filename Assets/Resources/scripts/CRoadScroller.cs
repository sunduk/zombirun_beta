using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CRoadScroller : MonoBehaviour {

	[SerializeField]
	Vector3 direction = new Vector3(0, 0, -1);

	[SerializeField]
	float speed = 5;

	float speed_ratio;
	List<Transform> roads;


	void Awake () {
		this.roads = new List<Transform>();
		Transform[] children = GetComponentsInChildren<Transform>();
		for (int i = 0; i < children.Length; ++i)
		{
			if (children[i] == transform)
			{
				continue;
			}
			this.roads.Add(children[i]);
		}
	}


	public void update_speed_ratio(float speed_ratio)
	{
		this.speed_ratio = speed_ratio;
	}


	// Update is called once per frame
	IEnumerator scroll_loop()
	{
		while (true)
		{
			for (int i = 0; i < this.roads.Count; ++i)
			{
				this.roads[i].localPosition += this.direction * (this.speed * this.speed_ratio) * Time.smoothDeltaTime;
			}

			for (int i = 0; i < this.roads.Count; ++i)
			{
				if (this.roads[i].localPosition.z <= -10.0f)
				{
					add_tail_from_head();
					break;
				}
			}

			yield return 0;
		}
	}


	void add_tail_from_head()
	{
		this.roads[0].localPosition = this.roads[this.roads.Count - 1].localPosition + new Vector3(0, 0, 10);
		this.roads.Add(this.roads[0]);
		this.roads.RemoveAt(0);
	}


	public void pause()
	{
		StopAllCoroutines();
	}


	public void restart()
	{
		StartCoroutine(scroll_loop());
	}
}
