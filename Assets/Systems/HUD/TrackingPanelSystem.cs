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
			Slider dragSlider = go.transform.Find ("PanelDrag/DragSlider").GetComponent<Slider> ();

			tp.lastVelocity = Vector3.zero;
			tp.lastCheckPointIndex = 0;

			tp.GQueue = new LimitedQueue<float> (tp.memory);

			sliderG.maxValue = tp.GRange;

			sliderOrientaion.minValue = -tp.rotRange / 2;
			sliderOrientaion.maxValue = tp.rotRange / 2;

			dragSlider.maxValue = tp.dragRange;

		}

	}


	protected override void onProcess (int familiesUpdateCount)
	{
	
		foreach (GameObject go in panels) {

			//@@@@@@@@@@@@@@@@@@
			// Components
			//@@@@@@@@@@@@@@@@@@

			TrackingPanel tp = go.GetComponent<TrackingPanel> ();
			Slider sliderG = go.transform.Find ("PanelG/GSlider").GetComponent<Slider> ();
			Slider sliderOrientaion = go.transform.Find ("PanelOrientation/OrientationSlider").GetComponent<Slider> ();
			Slider dragSlider = go.transform.Find ("PanelDrag/DragSlider").GetComponent<Slider> ();
			Slider orientationSlider = go.transform.Find ("PanelOrientation2/OrientationSlider").GetComponent<Slider> ();

			Image GAlert = go.transform.Find ("PanelG/Panel").GetComponent<Image> ();
			Image RotAlert = go.transform.Find ("PanelOrientation/Panel").GetComponent<Image> ();
			Image DragAlert = go.transform.Find ("PanelDrag/Panel").GetComponent<Image> ();
			Image orientationAlert = go.transform.Find ("PanelOrientation2/Panel").GetComponent<Image> ();

			Text affichageDegres = go.transform.Find ("PanelOrientation2/Affichage/Text").GetComponent<Text> ();

			GameObject rocket = tp.target;


			//@@@@@@@@@@@@@@@@@@
			// Best Check Point
			//@@@@@@@@@@@@@@@@@@

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

			//@@@@@@@@@@@@@@@@@@
			// Sliders
			//@@@@@@@@@@@@@@@@@@

			Rigidbody rb = tp.target.GetComponent<Rigidbody> ();

			Vector3 lastVelocity = rb.velocity;

			float G = (lastVelocity - tp.lastVelocity).magnitude * 9.81f / Time.timeScale * 10f; // 10 pour la mise à l'echelle

			tp.GQueue.Enqueue (G);
			sliderG.value = getQueueMean (tp.GQueue);
			tp.lastVelocity = lastVelocity;
			sliderOrientaion.value = tp.target.transform.rotation.x * 180 - tp.trajectory.checkPoints [bestIndex].orientation;

			dragSlider.value = rb.drag;

			Vector3 dirGravity = ( tp.earth.transform.position - rocket.transform.position ).normalized ;
			Vector3 dirShuttle = rocket.transform.up.normalized;
			float angle = Vector3.Angle (dirShuttle,dirGravity) * Mathf.Sign(Vector3.Cross(dirGravity,dirShuttle).x) + 180f;
			orientationSlider.value = (angle>180f) ? (angle - 360) : angle;
			affichageDegres.text = orientationSlider.value.ToString ("###0") + "°";


			//@@@@@@@@@@@@@@@@@@
			// 
			//@@@@@@@@@@@@@@@@@@

			if (tp.growing) {
				tp.alphaAlertVal += tp.blinkingSpeed * Time.deltaTime / Time.timeScale;
				if (tp.alphaAlertVal > 1f) {
					tp.growing = false;
				}
			} else {
				tp.alphaAlertVal -= tp.blinkingSpeed * Time.deltaTime / Time.timeScale;
				if (tp.alphaAlertVal < 0f) {
					tp.growing = true;
				}
			}

			//@@@@@@@@@@@@@@@@@@
			//
			//@@@@@@@@@@@@@@@@@@

			if (sliderG.value > tp.GAlertThreshold) {
				Color c = Color.red;
				c.a = tp.alphaAlertVal;
				GAlert.color = c;
			} else {
				Color c = Color.white;
				c.a = 100;
				GAlert.color = c;
			}

			if (Mathf.Abs (sliderOrientaion.value) > tp.RotAlertThreshold) {
				Color c = Color.red;
				c.a = tp.alphaAlertVal;
				RotAlert.color = c;
			} else {
				Color c = Color.white;
				c.a = 100;
				RotAlert.color = c;
			}

			if (orientationSlider.value > 90 || orientationSlider.value < -90) {
				Color c = Color.red;
				c.a = tp.alphaAlertVal;
				orientationAlert.color = c;
			} else {
				Color c = Color.white;
				c.a = 100;
				orientationAlert.color = c;
			}

			if (dragSlider.value > tp.DragAlertThreshold) {
				Color c = Color.red;
				c.a = tp.alphaAlertVal;
				DragAlert.color = c;
			} else {
				Color c = Color.white;
				c.a = 100;
				DragAlert.color = c;
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
