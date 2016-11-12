using UnityEngine;
using FYFY;
using UnityEngine.UI;
using System;

public class MonitorTextSystem : FSystem {

	private Family monitors = FamilyManager.getFamily(new AllOfComponents(typeof(MonitorText)));

	public MonitorTextSystem(){
		foreach (GameObject go in monitors) {
			MonitorText m = go.GetComponent<MonitorText> ();
			m.timer = 0f;
			m.frameCount = 0;
			m.accelerationQueue = new LimitedQueue<float> (m.memory);
			m.speedQueue = new LimitedQueue<float> (m.memory);
			m.messageQueue = new LimitedQueue<string> (4);
		}

	}


	protected override void onPause(int currentFrame) {
	}


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

			float speed = Vector3.Distance (m.target.transform.position, m.lastPosition) * m.scale * 3.6f / Time.fixedDeltaTime;
			float g = (speed - m.lastSpeed)  / Time.fixedDeltaTime / 9.81f / 2f;



			float alt = Vector3.Distance (Vector3.zero, m.target.transform.position) * m.scale;
			alt -= 6371000;
			alt /= 1000f;




			m.lastPosition = m.target.transform.position;
			m.lastSpeed = speed;

			m.accelerationQueue.Enqueue (g);
			m.speedQueue.Enqueue (speed);


			if (m.frameCount == m.fps) {


				string strG = String.Format("{0:0.00}", getQueueMean (m.accelerationQueue));
				string strAlt = String.Format("{0:0.000}", alt);

				string tex = "T : + " + (int)m.timer + "\n";
				tex += "Speed : " + (int)getQueueMean (m.speedQueue) + " km/h" + "\n"; 
				tex += "Acceleration : " + strG + " G" + "\n";
				tex += "Altitude : " + strAlt + " km" + "\n";

				foreach(string str in m.messageQueue){
					tex+="-: "+str+"\n";
				}

				text.text = tex;
				m.frameCount = 0;
			}
		}

	}

	public float getQueueMean(LimitedQueue<float> q){
		if (q.Count == 0)
			return 0;
		
		float res = 0f;
		foreach (float o in q){
			res += (float)o;
		}

		return res /= q.Count;
	}
}