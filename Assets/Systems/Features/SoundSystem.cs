using UnityEngine;
using FYFY;
using DigitalRuby.PyroParticles;

public class SoundSystem : FSystem {
	Family sounds = FamilyManager.getFamily (new AllOfComponents(typeof(SoundComponent)));

	Family flames = FamilyManager.getFamily (new AllOfComponents (typeof(Flames)));
	Family largables = FamilyManager.getFamily (new AllOfComponents (typeof(Largable)));

	GameObject temp;

	public SoundSystem() {
		temp = new GameObject ();
	}

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
		foreach (GameObject go in sounds)
			temp = go;

		SoundComponent sound = temp.GetComponent<SoundComponent> ();

		bool flamesFlag = false;
		bool jettisonFlag = false;

		foreach (GameObject go in flames) {
			if (go.GetComponent<Flames> ().isOn) {
				foreach (FireBaseScript f in go.GetComponent<Flames>().flames[0].GetComponentsInChildren<FireBaseScript>()) {
					foreach (ParticleSystem p in f.ManualParticleSystems) {
						if (p.startSize > 0 && p.isPlaying) {
							Debug.Log (p.startSize);
							flamesFlag = true;
							break;
						}
					}
				}
			}
		}

		foreach (GameObject go in largables) {
			if (go.GetComponent<Largable> ().toDrop) {
				jettisonFlag = true;
				break;
			}
		}

		Debug.Log ("son : " + sound.fireOn + " flames : " + flamesFlag);

		if (flamesFlag && !sound.fireOn) {
			sound.fireSound.Play ();
			Debug.Log ("Je suis ici");
			sound.fireOn = true;
		} 
		else if (flamesFlag == false) {
			sound.fireSound.Stop ();
			sound.fireOn = false;
		}

		if (jettisonFlag) {
			sound.jettisonSound.Play ();
		}
	}
}