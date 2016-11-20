using UnityEngine;
using System.Collections;

public class TrackingPanel : MonoBehaviour
{

	public int lastCheckPointIndex;
	public GameObject target;
	public TrajectoryGenerator trajectory;

	public Vector3 lastVelocity;
	public int memory;
	public LimitedQueue<float> GQueue;
}
