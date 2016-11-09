using UnityEngine;
using FYFY;
using UnityEngine.UI;

// Famille mettant à jour l'orientation de la fusée
public class OrientationHUDSystem : FSystem {
	private Family orientationSliders = FamilyManager.getFamily(new AllOfComponents(typeof(OrientationHUDComponent)));

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
		foreach (GameObject go in orientationSliders) {
			Slider slider = go.GetComponent<Slider> ();
			GameObject shuttle = go.GetComponent<OrientationHUDComponent> ().shuttle;
			GameObject earth = go.GetComponent<OrientationHUDComponent> ().earth;

			Vector3 dirGravity = ( earth.transform.position - shuttle.transform.position ).normalized ;
			Vector3 dirShuttle = shuttle.transform.up.normalized;
			float angle = Vector3.Angle (dirShuttle,dirGravity) * Mathf.Sign(Vector3.Cross(dirGravity,dirShuttle).x) + 180f;
			Debug.Log ("Angle: " + angle);
		}
	}
}
