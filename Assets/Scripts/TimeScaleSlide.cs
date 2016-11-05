using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TimeScaleSlide : MonoBehaviour {


	Slider slide;

	void Start () {
		slide = GetComponent<Slider> ();
	}
	
	// Update is called once per frame
	void Update () {
		Time.timeScale = slide.value;
	}
}
