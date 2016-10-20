using UnityEngine;
using FYFY;

public class GravitySystem : FSystem {
	// Récupération des familles sur lesquelles agit le système
	private Family orbiteurs = FamilyManager.getFamily(new AllOfComponents(typeof(Orbiteur), typeof(Rigidbody)));
	private Family attracteurs = FamilyManager.getFamily(new AllOfComponents(typeof(Attracteur), typeof(Rigidbody)));

	// Constante gravitationnelle pour les calculs des forces
	public static float GRAVITY_CNST = 6.67384e-8f; // ATTENTION : constante modifiée pour masses en tonnes

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
		float masseOrbiteur; // Masse de l'orbiteur courant dans la boucle
		float masseAttracteur; // Masse de l'attracteur courant dans la boucle
		Rigidbody rb; // Rigidbody de l'attracteur courant dans la boucle
		float force; // Force que l'on ajoutera à celles qui s'appliquent déjà sur le Rigidbody de l'orbiteur
		Vector3 direction; // Direction de la force qui s'appliquera

		// Pour chaque orbiteur
		foreach (GameObject orbiteur in orbiteurs) {
			rb = orbiteur.GetComponent<Rigidbody>();
			masseOrbiteur = rb.mass;
			// On lui applique la force de gravité de chaque attracteur
			foreach (GameObject attracteur in attracteurs) {
				masseAttracteur = attracteur.GetComponent<Rigidbody> ().mass;
				direction = ( attracteur.transform.position - orbiteur.transform.position ).normalized ;

				force = (((float)(GRAVITY_CNST) * masseAttracteur * masseOrbiteur) / Mathf.Pow (Vector3.Distance (orbiteur.transform.position, attracteur.transform.position), 2f)) * Time.deltaTime;

				rb.AddForce (direction * force);
			}
		}
	}
}