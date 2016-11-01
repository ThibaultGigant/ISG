using UnityEngine;
using UnityEngine.UI;

public class Propulseur : MonoBehaviour {

	public bool isOn;
	public float carburant;
	public float carburantMax;
	public float currentThrust;
	public float maxThrust;
	public float consoMax;
	public Quaternion orientation;
	public Slider fuelSlider;
	public Slider thrust;
}