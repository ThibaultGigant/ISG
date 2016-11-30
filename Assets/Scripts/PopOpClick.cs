using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using FYFY;

public class PopOpClick : MonoBehaviour
{

	PopUpComponent pop;

	void Start ()
	{
		pop = GetComponentInParent<PopUpComponent> ();
	}


	public void Ok ()
	{
		pop.display = false;
	}

	public void Menu(){
		Debug.Log ("Menu");
	}

	public void Replay(){
		Debug.Log ("Replay");
		GameObjectManager.loadScene(SceneManager.GetActiveScene().buildIndex);
	}
}
