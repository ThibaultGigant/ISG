using UnityEngine;
using FYFY;
using UnityEngine.UI;

public class JettisonHUDSystem : FSystem {
	private Family jettisonButtons = FamilyManager.getFamily(new AllOfComponents(typeof(JettisonHUDComponent)));
	
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
		foreach (GameObject go in jettisonButtons) {
			if (((Slider)go).value == 1) {
				go.GetComponent<JettisonHUDComponent> ().largable.toDrop = true;
				((Slider)go).interactable = false;
				GameObjectManager.removeComponent (JettisonHUDComponent);
			}
		}

	}
}