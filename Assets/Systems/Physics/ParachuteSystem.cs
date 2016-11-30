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
				if (PhysicsConstants.GetAltitude(rb.position) < PhysicsConstants.atmosphereEnd)
					go.transform.rotation = Quaternion.Slerp (go.transform.rotation, Quaternion.LookRotation (-rb.velocity), Time.fixedDeltaTime);

				if(rb.drag > p.breakForce ){
					GameObjectManager.removeComponent<Parachute> (go);
					foreach(GameObject para in p.toEnable){
						para.SetActive (false);
					}
				}

				foreach(GameObject para in p.toEnable){
					para.SetActive (true);
				}

			}

			if(p.release){
				GameObjectManager.removeComponent<Parachute> (go);
				foreach(GameObject para in p.toEnable){
					para.SetActive (true);
					para.transform.parent = null;
				}	
			}
		}

	}
}
