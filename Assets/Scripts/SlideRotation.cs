using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SlideRotation : MonoBehaviour {

	public GameObject target;
	Slider slider;

	void Start () {
		slider = GetComponent<Slider> ();
	}
	
	// Update is called once per frame
	void Update () {
		target.transform.rotation = Quaternion.Euler (new Vector3 (slider.value, 0, 0));
	}
}
