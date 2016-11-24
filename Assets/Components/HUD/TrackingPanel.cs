using UnityEngine;
using System.Collections;

public class TrackingPanel : MonoBehaviour
{
	public int memory;
	public float blinkingSpeed;
	[HideInInspector]
	public bool growing;
	public float GRange;
	public float rotRange;
	public float dragRange;
	[HideInInspector]
	public LimitedQueue<float> GQueue;
	[HideInInspector]
	public float alphaAlertVal;
}
