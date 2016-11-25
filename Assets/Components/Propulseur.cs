using UnityEngine;
using UnityEngine.UI;

public class Propulseur : MonoBehaviour
{

	public GameObject target;

	public bool isOn;

	public float initialFuel;
	public float currentFuel;

	public float maxThrust;
	public float currentThrust;

	public float consumption;
	// How much fuel is consumed per second when the thrust is at its maximum

	public bool backward;

	public float emptyMass;

}