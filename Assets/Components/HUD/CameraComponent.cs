using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;

public class CameraComponent : MonoBehaviour
{

	public float distance;
	public float angleX, angleY;
	public float speed;

	public GameObject target;
	public GameObject reference;

	public MouseLook mouseLook;
	public Camera theCamera;

}
