using UnityEngine;
using FYFY;
using UnityEngine.UI;

public class OnOffHUDSystem : FSystem {
	private Family onOffButtons = FamilyManager.getFamily(new AllOfComponents(typeof(ToggleHUDComponent)));

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
		foreach (GameObject go in onOffButtons) {
			Slider slider = go.GetComponentInChildren<Slider> ();
			if (slider.value == 1) {

				foreach (Largable checkIfDropped in go.GetComponent<ToggleHUDComponent>().dropBeforeIgnite) {
					if (checkIfDropped.toDrop == false) {
						checkIfDropped.GetComponentInParent<Rigidbody>().gameObject.tag = "Explosive";
						checkIfDropped.toDrop = true;
						GameControllerSystem.Explode(checkIfDropped.GetComponentInParent<Rigidbody>().gameObject,
							"You have engaged " + go.GetComponent<ToggleHUDComponent> ().propulseur.gameObject.name + 
							" before having jettisoned " + checkIfDropped.gameObject.name);
					}
				}

				go.GetComponent<ToggleHUDComponent> ().propulseur.isOn = true;

			}
			else
				go.GetComponent<ToggleHUDComponent> ().propulseur.isOn = false;
		}

	}
}