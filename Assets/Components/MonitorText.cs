using UnityEngine;
using System.Collections;

public class MonitorText : MonoBehaviour {

	public int nbLines = 8;
	public ArrayList nextLines;

	public float timer = 0f;

	public GameObject target;
	public Vector3 lastPosition;
	public float lastSpeed;

	public int frameCount = 0;

	public int fps = 10;

	//public FixedSizedQueue queue;

}

