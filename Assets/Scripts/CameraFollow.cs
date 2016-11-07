using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;

public class CameraFollow : MonoBehaviour {


	private MouseLook m_MouseLook;
	// Use this for initialization
	public GameObject toFollow;
	private Camera m_Camera;

	Vector3 offset;
	void Start () {
		m_MouseLook = new MouseLook ();
		m_Camera = transform.Find ("Main Camera").gameObject.GetComponent<Camera> ();
		offset = transform.position - toFollow.transform.position;
		m_MouseLook.Init(transform , m_Camera.transform);
	}
	
	// Update is called once per frame

	void FixedUpdate(){
		m_MouseLook.UpdateCursorLock();
	}

	void Update () {
		RotateView();
		transform.position = toFollow.transform.position + offset;
	}

	private void RotateView()
	{
		Debug.Log (m_MouseLook);
		m_MouseLook.LookRotation (transform, m_Camera.transform);
	}
}
