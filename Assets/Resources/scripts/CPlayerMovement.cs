using UnityEngine;
using System.Collections;

public class CPlayerMovement : MonoBehaviour {

	bool flag;
	float speed = 20.0f;
	float speed_ratio;


	void Awake()
	{
		reset();
	}


	void reset()
	{
		this.flag = true;

		Vector3 pos = transform.position;
		pos.x = -3.3f;
		transform.position = pos;
	}


	IEnumerator input_loop()
	{
		while (true)
		{
			if (Input.GetMouseButtonDown(0))
			{
				StopCoroutine("move");
				StartCoroutine("move");
			}

			yield return 0;
		}
	}


	IEnumerator move()
	{
		Vector3 from = transform.position;

		Vector3 to;
		if (flag)
		{
			to = new Vector3(2.6f, from.y, from.z);
		}
		else
		{
			to = new Vector3(-3.3f, from.y, from.z);
		}
		flag = !flag;

		Vector3 direction = (to - from).normalized;
		float duration = (to - from).magnitude / (this.speed * this.speed_ratio);
		float begin = Time.time;
		while (Time.time - begin < duration)
		{
			transform.position += direction * (this.speed * this.speed_ratio) * Time.smoothDeltaTime;
			yield return 0;
		}

		transform.position = to;
	}


	public void update_speed_ratio(float speed_ratio)
	{
		this.speed_ratio = speed_ratio;
	}


	public void pause()
	{
		StopAllCoroutines();
	}


	public void restart()
	{
		reset();
		StartCoroutine(input_loop());
	}
}
