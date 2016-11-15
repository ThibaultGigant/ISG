using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Characters.FirstPerson;

namespace UnityStandardAssets.Characters.FirstPerson
{
	[Serializable]
	public class MouseLook
	{
		public float XSensitivity = 2f;
		public float YSensitivity = 2f;
		public bool clampVerticalRotation = true;
		public float MinimumX = -180F;
		public float MaximumX = 180F;
		public bool smooth;
		public float smoothTime = 5f;

		public float maxFOV = 80f;
		public float minFOV = 8f;

		public float speed = 0.01f;

		public bool lockCursor = true;


		private Quaternion m_CharacterTargetRot;
		private Quaternion m_CameraTargetRot;
		private bool m_cursorIsLocked = false;

		bool needReset;
		bool tracking = false;

		int memory = 30;
		LimitedQueue<Vector3> queue;

		public void Init (Transform character, Transform camera)
		{
			m_CharacterTargetRot = character.localRotation;
			m_CameraTargetRot = camera.localRotation;
			needReset = false;

			if (queue == null) {
				queue = new LimitedQueue<Vector3> (memory);
			}
		}



		public void LookRotation (Transform character, Camera camera, Transform target, Vector3 up)
		{

			queue.Enqueue (target.transform.position);

			camera.fieldOfView += 2 * Input.GetAxis ("Mouse ScrollWheel");
			camera.fieldOfView = Mathf.Min (Mathf.Max (camera.fieldOfView, minFOV), maxFOV);

			if (Input.GetKeyDown (KeyCode.T)) {
				tracking = !tracking;
				if (!tracking) {
					Init (character, camera.transform);
				}
			}

			if (tracking) {
				
				float lerpSpeed = Mathf.Max (Mathf.Min (speed / Time.deltaTime, speed), 0.001f);

				Vector3 pos = Vector3.zero;

				foreach (Vector3 v3 in queue) {
					pos += v3;
				}
				pos /= queue.Count;
				Vector3 z = Vector3.zero;

				pos = Vector3.SmoothDamp (camera.transform.position, target.transform.position, ref z, .3f / Time.deltaTime);
			
				//
				//camera.transform.rotation = Quaternion.Lerp (camera.transform.rotation, Quaternion.LookRotation (target.transform.position - camera.transform.position), lerpSpeed);
				character.transform.LookAt (target);
				return;
			}

	
			if (!Input.GetKey (KeyCode.Mouse1)) {
				return;
			}
	
			needReset = true;

			float yRot = CrossPlatformInputManager.GetAxis ("Mouse X") * XSensitivity;
			float xRot = CrossPlatformInputManager.GetAxis ("Mouse Y") * YSensitivity;

			m_CharacterTargetRot *= Quaternion.Euler (0f, yRot, 0f);
			m_CameraTargetRot *= Quaternion.Euler (-xRot, 0f, 0f);

			if (clampVerticalRotation)
				m_CameraTargetRot = ClampRotationAroundXAxis (m_CameraTargetRot);

			if (smooth) {
				character.localRotation = Quaternion.Slerp (character.localRotation, m_CharacterTargetRot,
					smoothTime * Time.deltaTime);
				camera.transform.localRotation = Quaternion.Slerp (camera.transform.localRotation, m_CameraTargetRot,
					smoothTime * Time.deltaTime);
			} else {
				character.localRotation = m_CharacterTargetRot;
				camera.transform.localRotation = m_CameraTargetRot;
			}

			UpdateCursorLock ();
		}

		public void SetCursorLock (bool value)
		{
			lockCursor = value;
			if (!lockCursor) {
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;
			}
		}

		public void UpdateCursorLock ()
		{
			InternalLockUpdate ();
		}

		private void InternalLockUpdate ()
		{
			if (m_cursorIsLocked) {
				Cursor.lockState = CursorLockMode.Locked;
				Cursor.visible = false;
			} else if (!m_cursorIsLocked) {
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;
			}
		}

		Quaternion ClampRotationAroundXAxis (Quaternion q)
		{
			q.x /= q.w;
			q.y /= q.w;
			q.z /= q.w;
			q.w = 1.0f;

			float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan (q.x);

			angleX = Mathf.Clamp (angleX, MinimumX, MaximumX);

			q.x = Mathf.Tan (0.5f * Mathf.Deg2Rad * angleX);

			return q;
		}

	}
}
