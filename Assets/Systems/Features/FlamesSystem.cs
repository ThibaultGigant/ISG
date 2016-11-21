using UnityEngine;
using FYFY;
using DigitalRuby.PyroParticles;

public class FlamesSystem : FSystem {
	// Use this to update member variables when system pause. 
	// Advice: avoid to update your families inside this function.

	private Family flames = FamilyManager.getFamily(new AllOfComponents(typeof(Flames)));

	protected override void onPause(int currentFrame) {
	}

	// Use this to update member variables when system resume.
	// Advice: avoid to update your families inside this function.
	protected override void onResume(int currentFrame){
	}

	// Use to process your families.
	protected override void onProcess(int familiesUpdateCount) {

		foreach(GameObject go in flames){
			Flames f = go.GetComponent<Flames> ();
			foreach (GameObject f2 in f.flames){
				if (f2 != null) {
					FireBaseScript fbs = f2.GetComponent<FireBaseScript> ();
					if (fbs.ManualParticleSystems.Length > 0) {
						if (f.isOn) {
							foreach (ParticleSystem p in fbs.ManualParticleSystems) {
								Propulseur prop = go.GetComponent<Propulseur> ();
								float sizeFactor = prop.currentThrust / prop.maxThrust;
								p.startSize = 3 * sizeFactor;
								p.Play ();
							}
						} else {
							foreach (ParticleSystem p in fbs.ManualParticleSystems) {
								p.Stop ();
							}
						}
					}
				}

			}
		}

	}
}