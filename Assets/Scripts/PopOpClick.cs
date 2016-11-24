using UnityEngine;
using System.Collections;

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
}
