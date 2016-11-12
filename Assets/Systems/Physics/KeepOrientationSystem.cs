using UnityEngine;
using FYFY;
using System.Collections.Generic;

public class KeepOrientationSystem : FSystem {

	private Family orbiteurs = FamilyManager.getFamily(new AllOfComponents(typeof(Rigidbody)));

	/**
	 * Dictionnaire servant a recueillir les dernieres positions connues des Rigidbody
	 */
	private IDictionary<GameObject, Vector3> previousPosition;
	private int framesBetweenUpdates = 5;
	private int framesElapsed = 0;

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
		float angle;
		framesElapsed += 1;

		// Creation du dictionnaire si ce n'est deja fait
		if (previousPosition == null)
			previousPosition = new Dictionary<GameObject, Vector3> ();

		// Pour chaque GameObject, on effectue une rotation sur l'axe x de l'angle effectue entre les "framesBetweenUpdates" derniere frames
		foreach (GameObject go in orbiteurs) {
			if (previousPosition.ContainsKey (go) && framesElapsed > framesBetweenUpdates) {
				// Recuperation de l'angle effectue entre les frames
				angle = Vector3.Angle (
					previousPosition [go] - Vector3.zero,
					go.transform.position - Vector3.zero
				);

				// Effectue la rotation appropriee
				go.transform.Rotate(Vector3.right * angle);
				framesElapsed = 0;
			} else if (!previousPosition.ContainsKey(go)) {
				previousPosition.Add (go, go.transform.position);
			}

			// Mise a jour du dictionnaire
			previousPosition [go] = go.transform.position;
		}
	}
}