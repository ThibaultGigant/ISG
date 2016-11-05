using UnityEngine;
using FYFY;
using UnityEngine.UI;

public class ThrustHUDSystem : FSystem {
	// Famille pouvant régler la poussée de certains propulseurs
	private Family thrusts = FamilyManager.getFamily(new AllOfComponents(typeof(ThrustHUDComponent)));

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

		// Mise à jour de la poussée pour tous les propulseurs qui peuvent être réglés
		foreach (GameObject go in thrusts) {
			Propulseur prop = go.GetComponent<ThrustHUDComponent> ().propulseur;
			currentThrust = prop.maxThrust * ((Slider)go).value / 100;
			prop.currentThrust = currentThrust;
		}
	}
}