using UnityEngine;
using FYFY;

public class AtmoSphereGeneratorSystem : FSystem
{


	Family generator = FamilyManager.getFamily (new AllOfComponents (typeof(AtmoSphereGeneratorComponent)));



	public AtmoSphereGeneratorSystem ()
	{
		foreach (GameObject go in generator) {
			AtmoSphereGeneratorComponent atmo = go.GetComponent<AtmoSphereGeneratorComponent> ();

			for (int i = 0; i < 100/*PhysicsConstants.atmosphereEnd / atmo.precision*/; i++) {

				GameObject sphere = GameObjectManager.instantiatePrefab ("AtmoshereUnit");
				sphere.transform.localScale = Vector3.one * 100f * PhysicsConstants.altitudeOffset + Vector3.one * 10000f * i;//* PhysicsConstants.atmosphereEnd / atmo.precision;
				sphere.transform.Rotate (Vector3.one * Random.Range (0, 360));
			}


		}
	}

	protected override void onPause (int currentFrame)
	{
	}

	// Use this to update member variables when system resume.
	// Advice: avoid to update your families inside this function.
	protected override void onResume (int currentFrame)
	{
	}

	// Use to process your families.
	protected override void onProcess (int familiesUpdateCount)
	{
	}
}