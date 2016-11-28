using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using FYFY;

public class PopUpSystem : FSystem
{

	Family pops = FamilyManager.getFamily (new AllOfComponents (typeof(PopUpComponent)));


	protected override void onProcess (int familiesUpdateCount)
	{
		foreach (GameObject go in pops) {
			PopUpComponent pop = go.GetComponent<PopUpComponent> ();
			if (!pop.display) {
				go.SetActive (false);
			} else {
				go.SetActive (true);
				Text title = go.transform.Find ("Title/Text").GetComponent<Text> ();
				Text text = go.transform.Find ("Text/Text").GetComponent<Text> ();

				if (pop.title != "")
				title.text = pop.title;
				if (pop.text != "")
				text.text = pop.text;
			}
		}
	}
}
