using UnityEngine;

public class SoundComponent : MonoBehaviour {
	// Advice: FYFY component aims to contain only public members (according to Entity-Component-System paradigm).
	public AudioSource fireSound;
	public AudioSource jettisonSound;

	public bool fireOn;
}