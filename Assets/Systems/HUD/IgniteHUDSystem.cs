using UnityEngine;
using FYFY;
using UnityEngine.UI;

public class IgniteHUDSystem : FSystem {
	private Family igniteButtons = FamilyManager.getFamily(new AllOfComponents(typeof(IgniteHUDComponent)));

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
		foreach (GameObject go in igniteButtons) {
			if (((Slider)go).value == 1) {
				go.GetComponent<IgniteHUDComponent> ().propulseur.isOn = true;
				((Slider)go).interactable = false;
				GameObjectManager.removeComponent (IgniteHUDComponent);
			}
		}

	}
}