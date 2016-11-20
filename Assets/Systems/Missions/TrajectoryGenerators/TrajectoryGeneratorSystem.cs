using UnityEngine;
using FYFY;
using UnityEngine.UI;
using System;

public abstract class TrajectoryGeneratorSystem : FSystem
{

	private Family generators = FamilyManager.getFamily (new AllOfComponents (typeof(TrajectoryGenerator)));

	private Family checkPoints = FamilyManager.getFamily (new AnyOfTags ("CheckPoint"));

	public TrajectoryGeneratorSystem ()
	{
		foreach (GameObject go in generators) {
			GenerateTrajectory (go);
			//GameObjectManager.destroyGameObject (go);
		}

	}

	protected virtual void GenerateTrajectory (GameObject go)
	{
		CalculateAscension (go);
		CalculateAlignement (go);
		CalculateOrbitalTrajectory (go);
		CalculateReEntry (go);

		GeneratePrefabs (go);

	}

	protected override void onProcess (int countFrame)
	{
		int i = 0;
		TrajectoryGenerator generator = null;

		foreach (GameObject go in generators) {
			generator = go.GetComponent<TrajectoryGenerator> ();
		}

		foreach (GameObject cube in checkPoints) {
			CheckPoint check = cube.GetComponent<CheckPoint> ();

			CheckPointInfos ck = generator.checkPoints [i];

			cube.transform.position = ck.position;
			check.speed = ck.speed;
			check.orientation = ck.orientation;
			check.acceleration = ck.acceleration;
			i++;
		}

		this.Pause = true;
	}

	public void GeneratePrefabs (GameObject go)
	{

		TrajectoryGenerator trajectory = go.GetComponent<TrajectoryGenerator> ();

		foreach (CheckPointInfos ck in trajectory.checkPoints) {
			GameObject cube = GameObjectManager.instantiatePrefab (trajectory.checkPointPrefab.name);
		}

	}

	protected virtual void CalculateAscension (GameObject go)
	{
		
	}

	protected virtual void CalculateAlignement (GameObject go)
	{
		
	}

	protected virtual void CalculateOrbitalTrajectory (GameObject go)
	{
		
	}

	protected virtual void CalculateReEntry (GameObject go)
	{
		
	}

	public CheckPointInfos GenerateCheckPoint (Vector3 position, float speed, float acceleration, float orientation)
	{
		//GameObject checkPoint = GameObjectManager.instantiatePrefab (prefab);

		CheckPointInfos check = new CheckPointInfos ();
		check.position = position;
		check.acceleration = acceleration;
		check.speed = speed;
		check.orientation = orientation;
		return check;
	}

	public float GetCheckPointHeight (Vector3 position)
	{
		float alt = Vector3.Distance (Vector3.zero, position);
		//alt -= 63710;
		return alt;
	}
}
