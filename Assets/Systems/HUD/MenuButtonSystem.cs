using UnityEngine;
using FYFY;
using FYFY_plugins.MouseManager;

public class MenuButtonSystem : FSystem {
	Family buttons = FamilyManager.getFamily(new AllOfComponents(typeof(MenuButtonComponent), typeof(MouseOver)));

	// Use this to update member variables when system pause. 
	// Advice: avoid to update your families inside this function.
	protected override void onPause(int currentFrame) {
	}

	// Use this to update member variables when system resume.
	// Advice: avoid to update your families inside this function.
	protected override void onResume(int currentFrame){
	}

	// Use to process your families.
	protected override void onProcess(int familiesUpdateCount) {
		foreach (GameObject go in buttons) {
			Debug.Log ("Button Active");
			if (Input.GetMouseButtonDown (0))
				GameObjectManager.loadScene (go.GetComponent<MenuButtonComponent> ().scene);
		}
	}
}