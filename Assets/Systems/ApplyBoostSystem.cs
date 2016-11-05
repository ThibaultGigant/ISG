using UnityEngine;
using FYFY;
using UnityEngine.UI;

public class ApplyBoostSystem : FSystem {

	private Family propulseurs = FamilyManager.getFamily(new AllOfComponents(typeof(Propulseur),typeof(Masse)));

	protected override void onPause(int currentFrame) {
	}
		
	protected override void onResume(int currentFrame){
	}

	protected override void onProcess(int familiesUpdateCount) {
		 
		foreach (GameObject go in propulseurs) {
			
			Propulseur prop = go.GetComponent<Propulseur> ();
			Flames flames = go.GetComponent<Flames> ();
			Masse masse = prop.GetComponent<Masse> ();

<<<<<<< HEAD
			if (prop.isOn && prop.currentFuel > 0f) {
=======
			Slider thrustSlider = (Slider) prop.sliders.GetComponentsInChildren<Slider>()[0];
			Slider fuelSlider = (Slider) prop.sliders.GetComponentsInChildren<Slider>()[1];

			if (prop.isOn && prop.carburant>0){
				// on applique la force sur la fusée
				Rigidbody rb = go.GetComponent<Rigidbody>();
				currentThrust = prop.maxThrust * thrustSlider.value * 0.01f; // on lit le pourcentage de poussée à appliquer 
>>>>>>> origin/master

				Vector3 force = prop.target.transform.up * prop.currentThrust * 1000f * 50f; //On est en tonne
				Rigidbody rb = prop.target.GetComponent<Rigidbody> ();
				rb.AddForce (force);

<<<<<<< HEAD
				prop.currentFuel = Mathf.Max(prop.currentFuel - prop.currentThrust / prop.maxThrust * prop.consumption * Time.fixedDeltaTime, 0f);
			}
=======
				//go.transform.rotation = Quaternion.AngleAxis(orientationSlider.value, Vector3.forward);
				Vector3 force = currentThrust * Time.fixedDeltaTime * Vector3.up;

				//force = go.transform.rotation * force;
				Debug.Log (force);
>>>>>>> origin/master

			masse.mass = prop.emptyMass + prop.currentFuel;

			if (flames != null) {
				flames.isOn = prop.isOn && prop.currentFuel > 0;
			}
		}
	}
}