using UnityEngine;

public class Orbiteur : MonoBehaviour {
	// Advice: FYFY component aims to contain only public members (according to Entity-Component-System paradigm).
	public Vector3 vitesse;
	public GameObject target; // GameObject sur lequel on applique la force
}