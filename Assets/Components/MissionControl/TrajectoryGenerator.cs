using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TrajectoryGenerator : MonoBehaviour
{

	public bool drawDebug;

	public GameObject checkPointPrefab;

	public List<CheckPointInfos> checkPoints;

	public float stepDuration;
	public float targetG;

	// Ascension Phase
	public float targetAscensionHeight;


	//Alignement Phase
	public float targetAlignmentHeight;
	public float targetAlignmentOrientation;

	//Orbital phase
	public float targetOrbitalHeight;

	//ReEntry phase
	public float targetReEntryHeight;
}
