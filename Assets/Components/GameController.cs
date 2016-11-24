using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public GameObject target;
	public TrajectoryGenerator generator;

	public float maxTrajectoryDistance;


	public float GAlertThreshold;
	public float GFailThreshold;
	public float GFailThresholdDuration;

	public float DragAlertThreshold;
	public float DragFailThreshold;

	public float MaxCollisionSpeed;

	public float acceleration;
	public float speed;
	public float groundSpeed;
	public float altitude;
	public float drag;


	public float orientation;
	public float earthOrientation;


}
