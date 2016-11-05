using UnityEngine;
using FYFY;
using UnityEngine.UI;

// Famille mettant à jour les jauges de Fuel
public class FuelHUDSystem : FSystem {
	private Family fuelSliders = FamilyManager.getFamily(new AllOfComponents(typeof(FuelHUDComponent)));

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
		foreach (GameObject go in fuelSliders) {
			Propulseur prop = go.GetComponent<FuelHUDComponent> ().propulseur;
			((Slider)go).value = prop.currentFuel / prop.initialFuel * 100;
		}
	}
}