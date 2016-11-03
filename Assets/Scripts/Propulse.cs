using UnityEngine;
using System.Collections;

public class Propulse : MonoBehaviour {

	public Rigidbody toEmpty;
	public GameObject point;

	public float thrust;
	public float consomption;

	Rigidbody rb;
	Rigidbody parentRb;

	FixedJoint joint;

	// Use this for initialization
	void Start () {
		rb =  GetComponent<Rigidbody>();
		joint = GetComponent<FixedJoint>();
		parentRb = transform.parent.gameObject.GetComponent<Rigidbody> ();
		//rb = transform.parent.Find("Tank").gameObject.GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {

	
		if (toEmpty.mass > consomption) {

			if (joint == null) {
				toEmpty.mass -= consomption * Time.deltaTime;
				//rb.AddForce (300, thrust*Time.deltaTime, 0);
				rb.AddForce (0, thrust*Time.deltaTime, 0);
			} else {
				parentRb.AddForce (0, thrust*Time.deltaTime, 0);
			}

			//Debug.Log (transform.up);
		}
	}
}
