using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

	// Use this for initialization
	public GameObject toFollow;

	Vector3 offset;
	void Start () {
		offset = transform.position - toFollow.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = toFollow.transform.position + offset;
	}
}
