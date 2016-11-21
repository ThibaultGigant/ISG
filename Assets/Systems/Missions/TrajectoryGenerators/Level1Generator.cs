using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Level1Generator : TrajectoryGeneratorSystem
{

	protected override void CalculateAscension (GameObject generatorGo)
	{
		TrajectoryGenerator generator = generatorGo.GetComponent<TrajectoryGenerator> ();
		generator.checkPoints = new List<CheckPointInfos> ();

		CheckPointInfos last = GenerateCheckPoint (generatorGo.transform.position, 0f, 0f, 0f);
		generator.checkPoints.Add (last);

		int max = 10000;
		int count = 0;


		while (PhysicsConstants.GetAltitude (last.position) < generator.targetAscensionHeight && count < max) {


			float newAcceleration = generator.targetG;
			float newSpeed = last.speed + generator.stepDuration * newAcceleration;
			Vector3 newPosition = last.position + Vector3.up * newSpeed * generator.stepDuration;



			last = GenerateCheckPoint (newPosition, newSpeed, newAcceleration, 0f);
			generator.checkPoints.Add (last);
			count++;
		}

		Debug.Log ("Ascension - Count : " + count + " Max : " + max);

	}

	protected override void CalculateAlignement (GameObject generatorGo)
	{

		TrajectoryGenerator generator = generatorGo.GetComponent<TrajectoryGenerator> ();


		CheckPointInfos last = generator.checkPoints [generator.checkPoints.Count - 1];

		int max = 10000;
		int count = 1;

		float constante = 1f;
		constante *= generator.stepDuration;

		while (last.orientation < generator.targetAlignmentOrientation && count < max) {


			float newAcceleration = generator.targetG;
			float newSpeed = last.speed + generator.stepDuration * newAcceleration;

			float newOrientation = last.orientation + constante * Mathf.Log (count);

			Vector3 newPosition = last.position + Quaternion.Euler (new Vector3 (newOrientation, 0f, 0f)) * Vector3.up * newSpeed * generator.stepDuration;
			//Vector3 newPosition = last.position + Vector3.up * newSpeed * generator.stepDuration;

			last = GenerateCheckPoint (newPosition, newSpeed, newAcceleration, newOrientation);
			generator.checkPoints.Add (last);
			count++;
		}

		Debug.Log ("Alignement - Count : " + count + " Max : " + max);
		last.speed = last.speed * Mathf.Sin (last.orientation);

	}


	protected override void CalculateReEntry (GameObject generatorGo)
	{

		TrajectoryGenerator generator = generatorGo.GetComponent<TrajectoryGenerator> ();
		CheckPointInfos last = generator.checkPoints [generator.checkPoints.Count - 1];


		int max = 10000;
		int count = 1;

		float G = -9.81f / 10f;

		float xSpeed = last.speed;
		float ySpeed = 0f;

		while (PhysicsConstants.GetAltitude (last.position) > 0f && count < max) {

			Vector3 newPosition = last.position;

			newPosition.z += xSpeed * generator.stepDuration;

			ySpeed += G * generator.stepDuration;
			newPosition += ySpeed * generator.stepDuration * last.position.normalized;

			last = GenerateCheckPoint (newPosition, Mathf.Abs (Vector3.Distance (last.position, newPosition)), -1f, -70);
			generator.checkPoints.Add (last);
			count++;
		}

		Debug.Log ("Reentry - Count : " + count + " Max : " + max);
	}


}
