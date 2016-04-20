using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CMovableObject : MonoBehaviour {

	[SerializeField]
	Vector3 direction;

	[SerializeField]
	float speed;

	float speed_ratio;

	
	// Update is called once per frame
	void Update () {
		transform.localPosition += this.direction * (this.speed * this.speed_ratio) * Time.smoothDeltaTime;
	}


	public void update_speed_ratio(float speed_ratio)
	{
		this.speed_ratio = speed_ratio;
	}
}
