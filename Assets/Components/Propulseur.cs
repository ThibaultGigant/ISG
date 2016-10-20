using UnityEngine;

public class Propulseur : MonoBehaviour {
	// Advice: FYFY component aims to contain only public members (according to Entity-Component-System paradigm).
	public float carburant;
	public float currentThrust;
	public float maxThrust;
	public Quaternion orientation;
}