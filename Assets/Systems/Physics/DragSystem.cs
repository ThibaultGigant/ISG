using UnityEngine;
using FYFY;

public class DragSystem : FSystem
{


	Family rigids = FamilyManager.getFamily (new AllOfComponents (typeof(Rigidbody)));

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

		foreach (GameObject go in rigids) {
			Rigidbody rb = go.GetComponent<Rigidbody> ();
			//rb.drag = Mathf.Sqrt (Mathf.Max (0, PhysicsConstants.atmosphereEnd - PhysicsConstants.GetAltitude (rb.position))) * Mathf.Pow (rb.velocity.magnitude, 2);
			rb.drag = Mathf.Sqrt (Mathf.Max (0, PhysicsConstants.atmosphereEnd - PhysicsConstants.GetAltitude (rb.position))) / 2000;
		}

	}
}