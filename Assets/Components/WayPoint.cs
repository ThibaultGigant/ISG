using UnityEngine;

public class WayPoint : MonoBehaviour {
	// Advice: FYFY component aims to contain only public members (according to Entity-Component-System paradigm).
	public int id;
	public bool last = false;
	public float maxSpeed;
	public float minSpeed;
}