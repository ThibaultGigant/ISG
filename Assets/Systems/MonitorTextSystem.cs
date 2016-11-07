using UnityEngine;
using FYFY;
using UnityEngine.UI;
using System;

public class MonitorTextSystem : FSystem {

	private Family monitors = FamilyManager.getFamily(new AllOfComponents(typeof(MonitorText)));

	protected override void onPause(int currentFrame) {
	}

	// Use this to update member variables when system resume.
	// Advice: avoid to update your families inside this function.
	protected override void onResume(int currentFrame){

		foreach (GameObject go in monitors){
			MonitorText m = go.GetComponent<MonitorText> ();
			m.lastPosition = m.target.transform.position;
			m.lastSpeed = 0f;
		}
	}

	protected override void onProcess(int familiesUpdateCount) {

		foreach (GameObject go in monitors) {
			MonitorText m = go.GetComponent<MonitorText> ();
			Text text = go.GetComponent<Text> ();
			m.timer += Time.fixedDeltaTime;
			m.frameCount++;

			float speed = m.lastSpeed;
			//if (Vector3.Distance (m.target.transform.position, m.lastPosition) > 0) {
			speed = Vector3.Distance (m.target.transform.position, m.lastPosition) * 3.6f / Time.fixedDeltaTime;
			//}

			float g = (speed - m.lastSpeed)  / Time.fixedDeltaTime / 9.81f / 2f;
			string strG = String.Format("{0:0.00}", g);

			float alt = Vector3.Distance (new Vector3 (0f, -6371000, 0f), m.target.transform.position);
			alt -= 6371000;
			alt /= 1000f;

			string strAlt = String.Format("{0:0.000}", alt);

			m.lastPosition = m.target.transform.position;
			m.lastSpeed = speed;


			if (m.frameCount == m.fps) {
				string tex = "T : + " + (int)m.timer + "\n";
				tex += "Speed : " + (int)speed + " km/h" + "\n"; 
				tex += "Acceleration : " + strG + " G" + "\n";
				tex += "Altitude : " + strAlt + " km" + "\n";

				text.text = tex;
				m.frameCount = 0;
			}
		}

	}
}