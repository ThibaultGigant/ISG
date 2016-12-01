using UnityEngine;
using FYFY;

public class TimeScaleSystem : FSystem
{
	private Family timeScale = FamilyManager.getFamily (new AllOfComponents (typeof(TimeScaleHandler)));

	// Use to process your families.
	protected override void onProcess (int familiesUpdateCount)
	{
		foreach (GameObject go in timeScale) {
			TimeScaleHandler tsh = go.GetComponent<TimeScaleHandler> ();

			// Reset la timeScale
			if (Input.GetKeyDown (KeyCode.R)) {
				tsh.currentTimeScale = 0;
			}
			// Diminue d'un cran
			if (tsh.currentTimeScale > 0) { 
				if (Input.GetKeyDown (KeyCode.F)) {
					tsh.currentTimeScale--;
				}
			}
			// Augmente d'un cran
			if (tsh.currentTimeScale < tsh.timeScales.Count - 1) { 
				if (Input.GetKeyDown (KeyCode.G)) {
					tsh.currentTimeScale++;
				}
			}

			Time.timeScale = tsh.timeScales [tsh.currentTimeScale];
		}
	}
}