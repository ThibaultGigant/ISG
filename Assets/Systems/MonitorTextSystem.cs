using UnityEngine;
using FYFY;
using UnityEngine.UI;

public class MonitorTextSystem : FSystem {

	private Family monitors = FamilyManager.getFamily(new AllOfComponents(typeof(MonitorText)));

	protected override void onPause(int currentFrame) {
	}

	// Use this to update member variables when system resume.
	// Advice: avoid to update your families inside this function.
	protected override void onResume(int currentFrame){
	}

	// Use to process your families.
	protected override void onProcess(int familiesUpdateCount) {

		foreach (GameObject go in monitors){
			MonitorText m = go.GetComponent<MonitorText> ();
			Text text = go.GetComponent<Text> ();
			m.timer += Time.deltaTime;



		}

	}
}