using UnityEngine;
using FYFY;
using UnityEngine.UI;

public class ApplyBoostSystem : FSystem {

	private float rotationValue = 0;

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
		float currentThrust;

		foreach (GameObject go in propulseurs) {

			Propulseur prop = go.GetComponent<Propulseur> ();
			Flames flames = go.GetComponent<Flames> ();

			Slider thrustSlider = (Slider) prop.sliders.GetComponentsInChildren<Slider>()[0];
			Slider fuelSlider = (Slider) prop.sliders.GetComponentsInChildren<Slider>()[1];

			if (prop.isOn && prop.carburant>0){
				// on applique la force sur la fusée
				Rigidbody rb = go.GetComponent<Rigidbody>();
				currentThrust = prop.maxThrust * thrustSlider.value * 0.01f; // on lit le pourcentage de poussée à appliquer 

				//prop.orientation = go.transform.rotation;

				//go.transform.rotation = Quaternion.AngleAxis(orientationSlider.value, Vector3.forward);
				Vector3 force = currentThrust * Time.fixedDeltaTime * Vector3.up;

				//force = go.transform.rotation * force;
				Debug.Log (force);

				//rb.AddForce (force);
				rb.AddRelativeForce (force);

				// consommation de la propultion
				float consoReel = currentThrust * prop.consoMax / prop.maxThrust;
				prop.carburant -= consoReel * Time.fixedDeltaTime;
				fuelSlider.value = 100 * prop.carburant / prop.carburantMax;
				//Debug.Log ();
			}
			flames.isOn = prop.isOn && prop.carburant > 0 && thrustSlider.value > 0;
		}
	}
}