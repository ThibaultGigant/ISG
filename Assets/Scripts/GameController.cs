using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
	public float releaseTime;
	private float elapsedTime;

	// Use this for initialization
	void Start () {
		elapsedTime = 0f;
	}
	
	// Update is called once per frame
	void Update () {
		elapsedTime += Time.deltaTime;
		if (elapsedTime > releaseTime) {
			GameObject go1 = GameObject.Find ("side1");
			GameObject go2 = GameObject.Find ("side2");
			go1.GetComponent<Largable> ().toDrop = true;
			go2.GetComponent<Largable> ().toDrop = true;
		}
	}
}
