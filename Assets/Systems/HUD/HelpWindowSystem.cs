using UnityEngine;
using FYFY;

public class HelpWindowSystem : FSystem {
	Family help = FamilyManager.getFamily (new AnyOfTags("HelpWindow"), new AllOfComponents(typeof(PopUpComponent)));
	Family timeScale = FamilyManager.getFamily (new AllOfComponents (typeof(TimeScaleHandler)));

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

		if (Input.GetKeyDown (KeyCode.H) || Input.GetKeyDown (KeyCode.M) || Input.GetKeyDown (KeyCode.Escape)) {
			foreach (GameObject go in help)
				go.GetComponent<PopUpComponent> ().display = true;

			foreach (GameObject go in timeScale) {

				TimeScaleHandler tsh = go.GetComponent<TimeScaleHandler> ();
				tsh.currentTimeScale = 0;
				Time.timeScale = tsh.timeScales[tsh.currentTimeScale];

			}
		}
	}
}