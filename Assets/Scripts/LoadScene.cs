using UnityEngine;
using System.Collections;

public class LoadScene : MonoBehaviour
{

	public int scene;

	public void Load ()
	{
		Application.LoadLevel (scene);
	}
}
