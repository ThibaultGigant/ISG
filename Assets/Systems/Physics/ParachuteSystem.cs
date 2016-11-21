using UnityEngine;
using FYFY;

public class ParachuteSystem : FSystem {


	Family parachutes = FamilyManager.getFamily (new AllOfComponents(typeof(Parachute)));

	protected override void onPause(int currentFrame) {
	}

	// Use this to update member variables when system resume.
	// Advice: avoid to update your families inside this function.
	protected override void onResume(int currentFrame){
	}

	// Use to process your families.
	protected override void onProcess(int familiesUpdateCount) {
		foreach(GameObject go in parachutes){
			Parachute p = go.GetComponent<Parachute> ();
			Rigidbody rb = p.target;

			if(p.on){

				rb.drag += p.drag * Mathf.Sqrt (Mathf.Max (0, PhysicsConstants.atmosphereEnd - PhysicsConstants.GetAltitude (rb.position)));

				if(rb.drag > p.breakForce){
					GameObjectManager.removeComponent<Parachute> (go);
				}

			}

		}
	}
}