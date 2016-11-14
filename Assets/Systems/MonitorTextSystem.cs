using UnityEngine;
using FYFY;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

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
			// Récupération des composants nécessaires
			MonitorText m = go.GetComponent<MonitorText> ();
			Text text = go.GetComponent<Text> ();

			m.frameCount++;

			if (m.frameCount == m.fps) {
				// Mise à jour des données temporelles
				m.timer += Time.fixedDeltaTime * m.fps;

				// Calcul de la vitesse générale
				float speed = Vector3.Distance (m.target.transform.position, m.lastPosition) * 3.6f / (Time.fixedDeltaTime * m.fps);
				float g = (speed - m.lastSpeed)  / (Time.fixedDeltaTime * m.fps) / 9.81f / 2f;

				// Calcul de la vitesse au sol
				float angle = Vector3.Angle(m.target.transform.position, m.lastPosition);
				float groundSpeed;
				if (angle < 0.00001)
					groundSpeed = 0f;
				else
					groundSpeed = angle * 63710f * 3.6f * (float)Math.PI / (Time.fixedDeltaTime * m.fps * 180f);

				// Calcul de l'altitude
				float alt = Vector3.Distance (Vector3.zero, m.target.transform.position) * m.scale;
				alt -= 6371000;
				alt /= 1000f;

				// Mise à jour des données pour les calculs suivants
				m.lastPosition = m.target.transform.position;
				m.lastSpeed = speed;

				m.accelerationQueue.Enqueue (g);
				m.speedQueue.Enqueue (speed);
				
				string strG = String.Format("{0:0.00}", getQueueMean (m.accelerationQueue));
				string strAlt = String.Format("{0:0.000}", alt);

				string tex = "T : + " + (int)m.timer + "\n";
				tex += "Speed : " + (int)getQueueMean (m.speedQueue) + " km/h" + "\n"; 
				tex += "Grnd Speed : " + (int) groundSpeed + " km/h\n";
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