using UnityEngine;
using FYFY;
using UnityEngine.UI;

public class ApplyBoostSystem : FSystem {

	private Family propulseurs = FamilyManager.getFamily(new AllOfComponents(typeof(Propulseur),typeof(Masse)));

	protected override void onPause(int currentFrame) {
	}
		
	protected override void onResume(int currentFrame){
	}

	protected override void onProcess(int familiesUpdateCount) {
		 
		foreach (GameObject go in propulseurs) {
			
			Propulseur prop = go.GetComponent<Propulseur> ();
			Flames flames = go.GetComponent<Flames> ();
			Masse masse = prop.GetComponent<Masse> ();

			if (prop.isOn && prop.currentFuel > 0f) {

				Vector3 force = prop.target.transform.up * prop.currentThrust * 1000f * 50f; //On est en tonne
				Rigidbody rb = prop.target.GetComponent<Rigidbody> ();
				rb.AddForce (force);

				prop.currentFuel = Mathf.Max(prop.currentFuel - prop.currentThrust / prop.maxThrust * prop.consumption * Time.fixedDeltaTime, 0f);
			}

			masse.mass = prop.emptyMass + prop.currentFuel;

			if (flames != null) {
				flames.isOn = prop.isOn && prop.currentFuel > 0;
			}
		}
	}
}