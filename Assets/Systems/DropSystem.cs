using UnityEngine;
using FYFY;

public class DropSystem : FSystem {
	// Récupération des familles sur lesquelles agit le système
	private Family largables = FamilyManager.getFamily(new AllOfComponents(typeof(FixedJoint), typeof(Largable), typeof(Orbiteur)));

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
		foreach (GameObject go in largables)
		{
			if (go.GetComponent<Largable> ().toDrop)
			{
				go.GetComponent<Orbiteur> ().target = go;
				GameObjectManager.removeComponent<FixedJoint>(go);
			}
		}
	}
}