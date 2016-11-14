using UnityEngine;
using FYFY;

public class GravitySystem : FSystem {
	// Récupération des familles sur lesquelles agit le système
	private Family orbiteurs = FamilyManager.getFamily(new AllOfComponents(typeof(Rigidbody)));
	private Family attracteurs = FamilyManager.getFamily(new AllOfComponents(typeof(Attracteur), typeof(Masse)));

	// Constante gravitationnelle pour les calculs des forces
	public static float GRAVITY_CNST = 6.67384e-11f;

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
		double masseOrbiteur; // Masse de l'orbiteur courant dans la boucle
		double masseAttracteur; // Masse de l'attracteur courant dans la boucle
		Rigidbody rb; // Rigidbody de l'attracteur courant dans la boucle
		double force; // Force que l'on ajoutera à celles qui s'appliquent déjà sur le Rigidbody de l'orbiteur
		Vector3 direction; // Direction de la force qui s'appliquera

		// Pour chaque orbiteur
	
		foreach (GameObject orbiteur in orbiteurs) {
			rb = orbiteur.GetComponent<Rigidbody> ();
			masseOrbiteur = rb.mass;
			// On lui applique la force de gravité de chaque attracteur
			foreach (GameObject attracteur in attracteurs) {
				masseAttracteur = attracteur.GetComponent<Masse>().mass * Mathf.Pow(10, attracteur.GetComponent<Masse>().exposant);
				direction = ( attracteur.transform.position - orbiteur.transform.position ).normalized ; // 50 * Time.fixedDeltaTime *
				force =  ((GRAVITY_CNST) * masseAttracteur * masseOrbiteur) / Mathf.Pow (Vector3.Distance (orbiteur.transform.position, attracteur.transform.position) * 100, 2f); // *100 car on a divise les distances par 100
				//force *= .5555555f;
				rb.mass = (float) masseOrbiteur;
				rb.AddForce (direction * (float) force);
			}
		}
	}
}