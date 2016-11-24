using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Mission1GeneratorSystem : TrajectoryGeneratorSystem
{

	// Only ascension in first mission

	protected override void CalculateAscension (GameObject generatorGo)
	{
		TrajectoryGenerator generator = generatorGo.GetComponent<TrajectoryGenerator> ();
		generator.checkPoints = new List<CheckPointInfos> ();

		CheckPointInfos last = GenerateCheckPoint (generatorGo.transform.position, 0f, 0f, 0f);
		generator.checkPoints.Add (last);

		int max = 10000;
		int count = 0;


		while (PhysicsConstants.GetAltitude (last.position) < generator.targetAscensionHeight && count < max) {

			float newAcceleration = last.acceleration >= generator.targetG ? generator.targetG : (last.acceleration + generator.targetG * generator.stepDuration / 10);
			float newSpeed = last.speed + generator.stepDuration * newAcceleration;
			Vector3 newPosition = last.position + Vector3.up * newSpeed * generator.stepDuration;



			last = GenerateCheckPoint (newPosition, newSpeed, newAcceleration, 0f);
			generator.checkPoints.Add (last);
			count++;
		}

		Debug.Log ("Ascension - Count : " + count + " Max : " + max);

	}


}
