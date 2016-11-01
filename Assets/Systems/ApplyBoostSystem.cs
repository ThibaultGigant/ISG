using UnityEngine;
using FYFY;

public class ApplyBoostSystem : FSystem {

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
			Flames flames = go.GetComponent<Flames> ();

			if (prop.isOn && prop.carburant>0){
				// on applique la force sur la fusée
				Rigidbody rb = go.GetComponent<Rigidbody>();
				prop.currentThrust = prop.maxThrust * prop.thrust.value * 0.01f; // on lit le pourcentage de poussée à appliquer 
				Vector3 force = prop.currentThrust * prop.orientation.eulerAngles * Time.fixedDeltaTime;
				rb.AddForce (force);
				// consommation de la propultion
				float consoReel = prop.currentThrust * prop.consoMax / prop.maxThrust;
				prop.carburant -= consoReel * Time.fixedDeltaTime;
				prop.fuelSlider.value = 100 * prop.carburant / prop.carburantMax;
				//Debug.Log ();
			}
			flames.isOn = prop.isOn && prop.carburant > 0;
		}
	}
}