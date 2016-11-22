using UnityEngine;
using System.Collections;

public class PhysicsConstants
{


	public static float atmosphereEnd = 350f;
	public static float altitudeOffset = 63710f;

	public static float GetAltitude (Vector3 position)
	{
		float alt = Vector3.Distance (Vector3.zero, position);
		alt -= altitudeOffset;
		return alt / 10;
	}
}
