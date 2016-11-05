using UnityEngine;
using FYFY;
using UnityEngine.UI;

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

	// Use to process your families.
	protected override void onProcess(int familiesUpdateCount) {

		foreach (GameObject go in monitors) {
			MonitorText m = go.GetComponent<MonitorText> ();
			Text text = go.GetComponent<Text> ();
			m.timer += Time.deltaTime;

			m.frameCount += 1;



			float speed = Vector3.Distance (m.target.transform.position, m.lastPosition) / Time.deltaTime * 3.6f;
			float g = Time.deltaTime * (speed - m.lastSpeed) * 3.6f * 9.81f * 2f;
			float alt = Vector3.Distance (new Vector3 (0f, -6371000, 0f), m.target.transform.position);
			alt -= 6371000;
			m.lastPosition = m.target.transform.position;
			m.lastSpeed = speed;

			if (m.frameCount == m.fps) {
				string tex = "T : +" + (int)m.timer + "\n";
				tex += "Speed : " + (int)speed + " km/h" + "\n"; 
				tex += "Acceleration : " + (int)g + " G" + "\n";
				tex += "Altitude : " + (int)alt + " m" + "\n";

				text.text = tex;
				m.frameCount = 0;
			}
		}

	}
}