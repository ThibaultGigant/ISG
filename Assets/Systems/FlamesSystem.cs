using UnityEngine;
using FYFY;

public class FlamesSystem : FSystem {
	// Use this to update member variables when system pause. 
	// Advice: avoid to update your families inside this function.

	private Family flames = FamilyManager.getFamily(new AllOfComponents(typeof(Flames)));

	protected override void onPause(int currentFrame) {
	}

	// Use this to update member variables when system resume.
	// Advice: avoid to update your families inside this function.
	protected override void onResume(int currentFrame){
	}

	// Use to process your families.
	protected override void onProcess(int familiesUpdateCount) {

		foreach(GameObject go in flames){
			Flames f = go.GetComponent<Flames> ();
			foreach (GameObject f2 in f.flames){
				f2.SetActive (f.isOn);

			}
		}

	}
}