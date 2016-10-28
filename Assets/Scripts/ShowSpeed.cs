using UnityEngine;
using System.Collections;

public class ShowSpeed : MonoBehaviour {

	Vector3 lastPos;
	float lastSpeed;
	float time;


	// Use this for initialization
	void Start () {
		lastPos = gameObject.transform.position;
		lastSpeed = 0;
		time = 0f;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		time += Time.fixedDeltaTime;

		float speed = Vector3.Distance (gameObject.transform.position, lastPos) / Time.fixedDeltaTime * 3.6f;

		float g = Time.fixedDeltaTime * (speed - lastSpeed) * 3.6f * 9.81f * 2f;// Vas savoir pourquoi on doit mettre x2

		Debug.Log ("T = "+((int)time)+" "+gameObject.name+" speed : "+((int)speed)+ "km/h\nAcceleration : "+g+" G");

		lastPos = gameObject.transform.position;

		lastSpeed = speed;
	}
}
