using UnityEngine;
using System.Collections;

public class Gravity : MonoBehaviour {

	public static float GRAVITY_CNST_EARTH_MASS = 4e+17f;

	Vector3 earthCenter = new Vector3(0f,-6371000f,0f);
	Rigidbody rb;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {
	
		Vector3 direction = ( earthCenter - transform.position ).normalized ;

		float force = ( ((float)(GRAVITY_CNST_EARTH_MASS) * rb.mass) / Mathf.Pow(Vector3.Distance (earthCenter, transform.position),2f) ) * Time.deltaTime;

		rb.AddForce (direction * force);

	}
}
