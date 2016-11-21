using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using FYFY;

public class TrackingPanelSystem : FSystem
{


	private Family panels = FamilyManager.getFamily (new AllOfComponents (typeof(TrackingPanel)));

	public TrackingPanelSystem ()
	{

		foreach (GameObject go in panels) {
			TrackingPanel tp = go.GetComponent<TrackingPanel> ();

			Slider sliderG = go.transform.Find ("PanelG/GSlider").GetComponent<Slider> ();
			Slider sliderOrientaion = go.transform.Find ("PanelOrientation/OrientationSlider").GetComponent<Slider> ();
			Image GAlert = go.transform.Find ("PanelG/Panel/Image").GetComponent<Image>();
			Image RotAlert = go.transform.Find ("PanelOrientation/Panel/Image").GetComponent<Image>();

			tp.lastVelocity = Vector3.zero;
			tp.lastCheckPointIndex = 0;

			tp.GQueue = new LimitedQueue<float> (tp.memory);

			sliderG.maxValue = tp.GRange;

			sliderOrientaion.minValue = -tp.rotRange/2;
			sliderOrientaion.maxValue = tp.rotRange/2;

			GAlert.enabled = false;
			RotAlert.enabled = false;
		}

	}


	protected override void onProcess (int familiesUpdateCount)
	{
	
		foreach (GameObject go in panels) {
			TrackingPanel tp = go.GetComponent<TrackingPanel> ();
			Slider sliderG = go.transform.Find ("PanelG/GSlider").GetComponent<Slider> ();
			Slider sliderOrientaion = go.transform.Find ("PanelOrientation/OrientationSlider").GetComponent<Slider> ();

			Image GAlert = go.transform.Find ("PanelG/Panel/Image").GetComponent<Image>();
			Image RotAlert = go.transform.Find ("PanelOrientation/Panel/Image").GetComponent<Image>();

			GameObject rocket = tp.target;

		
			int bestIndex = tp.lastCheckPointIndex;
			float bestDistance = Vector3.Distance (tp.trajectory.checkPoints [bestIndex].position, tp.target.transform.position);
			int temp = Mathf.Min (bestIndex + 1, tp.trajectory.checkPoints.Count - 1);
			float tempDistance = Vector3.Distance (tp.trajectory.checkPoints [temp].position, tp.target.transform.position);

			while (temp < tp.trajectory.checkPoints.Count - 1 && tempDistance < bestDistance) {
				bestIndex = temp;
				bestDistance = tempDistance;
				temp++;
				tempDistance = Vector3.Distance (tp.trajectory.checkPoints [temp].position, tp.target.transform.position);
			}

			temp = (int)Mathf.Max (bestIndex - 1, 0);
			tempDistance = Vector3.Distance (tp.trajectory.checkPoints [temp].position, tp.target.transform.position);
			while (temp > 0 && tempDistance < bestDistance) {
				bestIndex = temp;
				bestDistance = tempDistance;
				temp++;
				tempDistance = Vector3.Distance (tp.trajectory.checkPoints [temp].position, tp.target.transform.position);
			}

			tp.lastCheckPointIndex = bestIndex;
			Vector3 lastVelocity = tp.target.GetComponent<Rigidbody> ().velocity;
			float G = (lastVelocity - tp.lastVelocity).magnitude * 9.81f;
			tp.GQueue.Enqueue (G);
			sliderG.value = getQueueMean (tp.GQueue);
			tp.lastVelocity = lastVelocity;
			sliderOrientaion.value = tp.target.transform.rotation.x * 180 - tp.trajectory.checkPoints [bestIndex].orientation;

			if(tp.growing){
				tp.alphaAlertVal += tp.blinkingSpeed * Time.deltaTime;
				if (tp.alphaAlertVal > 1f){
					tp.growing = false;
				}
			}else{
				tp.alphaAlertVal -= tp.blinkingSpeed * Time.deltaTime;
				if (tp.alphaAlertVal < 0f){
					tp.growing = true;
				}
			}

			if (sliderG.value > tp.GAlertThreshold){
				GAlert.enabled = true;
				Color c = GAlert.color;
				c.a = tp.alphaAlertVal;
				GAlert.color = c;
			}else{
				GAlert.enabled = false;
			}

			if (Mathf.Abs (sliderOrientaion.value) > tp.RotAlertThreshold){
				RotAlert.enabled = true;
				Color c = RotAlert.color;
				c.a = tp.alphaAlertVal;
				RotAlert.color = c;
			}else{
				RotAlert.enabled = false;
			}

		}
	
	}

	public float getQueueMean (LimitedQueue<float> q)
	{
		if (q.Count == 0)
			return 0;

		float res = 0f;
		foreach (float o in q) {
			res += (float)o;
		}

		return res /= q.Count;
	}
}
