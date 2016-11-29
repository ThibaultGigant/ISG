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
			Slider slider = go.GetComponentInChildren<Slider> ();
			if (slider.value == 1) {
				/*
				foreach (Largable checkIfDropped in go.GetComponent<IgniteHUDComponent>().dropBeforeIgnite) {
					if (checkIfDropped.toDrop == false) {
						go.tag = "Explosive";
						GameObjectManager.addComponent<Rigidbody> (go.GetComponent<IgniteHUDComponent>().propulseurs[0].gameObject);
						checkIfDropped.toDrop = true;
					}
				}
				*/
				foreach (Propulseur prop in go.GetComponent<IgniteHUDComponent>().propulseurs) {
					prop.isOn = true;
				}
				slider.interactable = false;
				GameObjectManager.removeComponent (go.GetComponent<IgniteHUDComponent> ());
			}
		}

	}
}