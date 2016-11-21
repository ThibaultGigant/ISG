using UnityEngine;
using FYFY;
using UnityEngine.UI;
using System;
using UnityStandardAssets.Characters.FirstPerson;

public class CameraSystem : FSystem
{
	
	private Family cameras = FamilyManager.getFamily (new AllOfComponents (typeof(CameraComponent)));

	public CameraSystem ()
	{
		foreach (GameObject go in cameras) {
			CameraComponent cam = go.GetComponentInChildren<CameraComponent> ();
			cam.mouseLook = new MouseLook ();
		}
	}


	protected override void onPause (int currentFrame)
	{
	}


	protected override void onResume (int currentFrame)
	{

		foreach (GameObject go in cameras) {
			CameraComponent cam = go.GetComponentInChildren<CameraComponent> ();
			cam.theCamera = go.transform.Find ("Main Camera").gameObject.GetComponent<Camera> ();
			cam.mouseLook.Init (go.transform, cam.theCamera.transform);
		}
	}

	protected override void onProcess (int familiesUpdateCount)
	{
		foreach (GameObject go in cameras) {
			CameraComponent cam = go.GetComponent<CameraComponent> ();



			if (Input.GetKey (KeyCode.O)) {
				
				cam.angleX += ((Input.GetKey (KeyCode.LeftShift)) ? cam.speed * 5 : cam.speed) / Time.deltaTime;
			}
			if (Input.GetKey (KeyCode.L)) {
				cam.angleX -= ((Input.GetKey (KeyCode.LeftShift)) ? cam.speed * 5 : cam.speed) / Time.deltaTime;
			}


			Vector3 tmp = (cam.reference.transform.position - cam.target.transform.position).normalized;
			tmp = Quaternion.AngleAxis (cam.angleX, Vector3.right) * tmp;


			cam.mouseLook.LookRotation (go.transform, cam.theCamera, cam.target.transform, -tmp);


			//Plante sinon
			if (Time.timeScale == 0) {
				return; 
			}
			go.transform.position = cam.target.transform.position - tmp * cam.distance;

		}
	}
}