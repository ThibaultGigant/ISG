using UnityEngine;
using FYFY;
using DigitalRuby.PyroParticles;

public class FlamesSystem : FSystem
{
	// Use this to update member variables when system pause.
	// Advice: avoid to update your families inside this function.

	private Family flames = FamilyManager.getFamily (new AllOfComponents (typeof(Flames)));

	protected override void onPause (int currentFrame)
	{
	}

	// Use this to update member variables when system resume.
	// Advice: avoid to update your families inside this function.
	protected override void onResume (int currentFrame)
	{
	}

	void setFlameSize (FireBaseScript fbs, float size, bool on)
	{
		if (fbs == null || fbs.ManualParticleSystems.Length <= 0) {
			return;
		}
		if (on) {
			foreach (ParticleSystem p in fbs.ManualParticleSystems) {
				p.startSize = 3 * size;
				p.Play ();
			}
		} else {
			foreach (ParticleSystem p in fbs.ManualParticleSystems) {
				p.Stop ();
			}
		}

	}

	// Use to process your families.
	protected override void onProcess (int familiesUpdateCount)
	{
		foreach (GameObject go in flames) {
			Propulseur prop = go.GetComponent<Propulseur> ();
			float sizeFactor = prop.currentThrust / prop.maxThrust;
			Flames f = go.GetComponent<Flames> ();

			foreach (GameObject f2 in f.flames) {
				FireBaseScript fbs = f2.GetComponent<FireBaseScript> ();
				FireBaseScript[] cfbs = f2.GetComponentsInChildren<FireBaseScript> ();
				setFlameSize (fbs, sizeFactor, f.isOn);
				foreach (FireBaseScript fbss in cfbs) {
					setFlameSize (fbss, sizeFactor, f.isOn);
				}
			}
		}
	}
}