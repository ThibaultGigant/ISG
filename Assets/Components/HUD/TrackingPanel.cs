using UnityEngine;
using System.Collections;

public class TrackingPanel : MonoBehaviour
{


	public int memory;
	public GameObject target;

	public float rotRange;
	public float GRange;
	public float dragRange;

	public float GAlertThreshold;
	public float RotAlertThreshold;
	public float DragAlertThreshold;

	public float alphaAlertVal;
	public bool growing;
	public float blinkingSpeed;

	public int lastCheckPointIndex;

	public TrajectoryGenerator trajectory;

	public Vector3 lastVelocity;

	public LimitedQueue<float> GQueue;
}
