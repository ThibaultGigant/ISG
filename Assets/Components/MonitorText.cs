using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class MonitorText : MonoBehaviour {

	public int memory = 10;

	public int nbLines = 4;
	public ArrayList nextLines;

	public float timer;

	public GameObject target;
	public Vector3 lastPosition;
	public float lastSpeed;
	public int frameCount;
	public int fps = 10;

	public LimitedQueue<float> accelerationQueue;
	public LimitedQueue<float> speedQueue;
	public LimitedQueue<string> messageQueue;


}

