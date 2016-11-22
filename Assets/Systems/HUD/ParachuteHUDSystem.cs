using UnityEngine;
using UnityEngine.UI;
using FYFY;

public class ParachuteHUDSystem : FSystem {


	private Family parachuteButtons = FamilyManager.getFamily(new AllOfComponents(typeof(ParachuteHUDComponent)));

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
		foreach (GameObject go in parachuteButtons) {
			Slider slider = go.GetComponentInChildren<Slider> ();
			if (slider.value == 1) {

				ParachuteHUDComponent para = go.GetComponent<ParachuteHUDComponent> ();

				if (para.open){
					foreach (Parachute parachute in para.parachutes) {
						parachute.on = true;
					}
					GameObjectManager.removeComponent (go.GetComponent<ParachuteHUDComponent> ());
				}else{
					foreach (Parachute parachute in para.parachutes) {
						parachute.release = true;
					}
				}
				slider.interactable = false;
			}
		}

	}
}