using UnityEngine;
using FYFY;
using FYFY_plugins.TriggerManager;
using UnityEngine.UI;

public class ActivatorSystem : FSystem {

	Family activators = FamilyManager.getFamily (new AllOfComponents (typeof(Activator)));
	
	protected override void onPause(int currentFrame) {
	}

	// Use this to update member variables when system resume.
	// Advice: avoid to update your families inside this function.
	protected override void onResume(int currentFrame){
	}

	// Use to process your families.
	protected override void onProcess(int familiesUpdateCount) {

		foreach (GameObject go in activators) {
			Activator ac = go.GetComponent<Activator> ();
			if (PhysicsConstants.GetAltitude(ac.target.position) > ac.altitude)
				ac.toActivate.GetComponent<Slider>().interactable = true;
		}
	
	}
}