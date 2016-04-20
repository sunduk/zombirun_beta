using UnityEngine;
using System.Collections;

public class CRotationObject : MonoBehaviour {

	[SerializeField]
	float speed;

	[SerializeField]
	Vector3 axis;


	// Update is called once per frame
	void Update () {
		transform.Rotate(this.axis, this.speed * Time.smoothDeltaTime);
	}
}
