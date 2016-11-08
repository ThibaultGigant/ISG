using UnityEngine;
using System.Collections.Generic;

public class Capcom : MonoBehaviour {


	public MonitorText m;
	private float timer;
	private Queue<string> q;

	// Use this for initialization
	void Start () {
		timer = 0f;
		q = new Queue<string> ();
		q.Enqueue ("Test 1");
		q.Enqueue ("Test 2");
		q.Enqueue ("Test 3");
		q.Enqueue ("Test 4");
		q.Enqueue ("Test 5");
		q.Enqueue ("Test 6");
		q.Enqueue ("Test 7");
	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;
		if (timer > 2f) {
			if (q.Count > 0) {
			m.messageQueue.Enqueue (q.Dequeue ());
			timer = 0f;
		}

		}
			

	}
}
