using UnityEngine;
using FYFY;

public class OrientationSystem : FSystem {

	private Family orbiteurs = FamilyManager.getFamily(new AllOfComponents(typeof(Rigidbody)));
	private float speed = 5;

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
		foreach (GameObject go in orbiteurs) {
			if (Input.GetKey (KeyCode.Z))
				go.GetComponent<Rigidbody> ().AddRelativeTorque (Vector3.right, ForceMode.Acceleration);
			if (Input.GetKey (KeyCode.S))
				go.GetComponent<Rigidbody> ().AddRelativeTorque (-Vector3.right, ForceMode.Acceleration);
		}
	}
}