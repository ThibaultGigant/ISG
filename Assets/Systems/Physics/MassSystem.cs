using UnityEngine;
using FYFY;

public class MassSystem : FSystem {

	private Family rigidbodies = FamilyManager.getFamily (new AllOfComponents(typeof(Rigidbody)));
	private Family masses = FamilyManager.getFamily(new AllOfComponents(typeof(Masse)));
	
	protected override void onPause(int currentFrame) {
	}


	protected override void onResume(int currentFrame){
	}


	protected override void onProcess(int familiesUpdateCount) {

		foreach(GameObject go in rigidbodies){
			Rigidbody rb = go.GetComponent<Rigidbody> ();
			rb.mass = 0f;
		}

		foreach(GameObject go in masses){
			Masse mass = go.GetComponent<Masse> ();
			if (mass.target != null) {
				Rigidbody rb = mass.target.GetComponent<Rigidbody> ();
				//Masse m2 = mass.target.GetComponent<Masse> ();
				rb.mass += mass.mass * Mathf.Pow (10, mass.exposant);
			}
		}
	}
}