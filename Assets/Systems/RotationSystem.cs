using UnityEngine;
using FYFY;
using UnityEngine.UI;

public class RotationSystem : FSystem {

	private Family propulseurs = FamilyManager.getFamily(new AllOfComponents(typeof(Propulseur), typeof(Masse)));

	// Use this to update member variables when system pause. 
	// Advice: avoid to update your families inside this function.
	protected override void onPause(int currentFrame) {
	}

	// Use this to update member variables when system resume.
	// Advice: avoid to update your families inside this function.
	protected override void onResume(int currentFrame){
	}

	// Use to process your families.
	protected override void onProcess(int familiesUpdateCount) {

		foreach (GameObject go in propulseurs) {
			
			Propulseur prop = go.GetComponent<Propulseur> ();
			Slider orientationSlider = (Slider) prop.sliders.GetComponentsInChildren<Slider>()[2];

			Quaternion rot = go.GetComponent<Rigidbody> ().rotation;

			go.GetComponent<Rigidbody> ().rotation = Quaternion.Euler(orientationSlider.value, 180, -90);
		}

	}
}