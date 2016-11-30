using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using FYFY;
using System;

public class TrackingPanelSystem : FSystem
{
	private Family panels = FamilyManager.getFamily (new AllOfComponents (typeof(TrackingPanel)));
	private Family gameControl = FamilyManager.getFamily (new AllOfComponents (typeof(GameController)));

	public TrackingPanelSystem ()
	{
		foreach (GameObject go in panels) {
			foreach (GameObject go2 in gameControl) {
				GameController gc = go2.GetComponent<GameController> ();
			
				TrackingPanel tp = go.GetComponent<TrackingPanel> ();

				Slider sliderG = go.transform.Find ("PanelG/GSlider").GetComponent<Slider> ();
				Slider dragSlider = go.transform.Find ("PanelDrag/DragSlider").GetComponent<Slider> ();

				tp.GQueue = new LimitedQueue<float> (tp.memory);

				sliderG.maxValue = tp.GRange;

				dragSlider.maxValue = tp.dragRange;
			}

		}

	}


	protected override void onProcess (int familiesUpdateCount)
	{
	
		foreach (GameObject go in panels) {
			foreach (GameObject go2 in gameControl) {
				GameController gc = go2.GetComponent<GameController> ();

				//@@@@@@@@@@@@@@@@@@
				// Components
				//@@@@@@@@@@@@@@@@@@

				TrackingPanel tp = go.GetComponent<TrackingPanel> ();
				Slider sliderG = go.transform.Find ("PanelG/GSlider").GetComponent<Slider> ();
				Slider dragSlider = go.transform.Find ("PanelDrag/DragSlider").GetComponent<Slider> ();
				Slider orientationSlider = go.transform.Find ("PanelOrientation/OrientationSlider").GetComponent<Slider> ();

				Image GAlert = go.transform.Find ("PanelG/Panel").GetComponent<Image> ();
				Image DragAlert = go.transform.Find ("PanelDrag/Panel").GetComponent<Image> ();
				Image orientationAlert = go.transform.Find ("PanelOrientation/Panel").GetComponent<Image> ();

				Text affichageDegres = go.transform.Find ("PanelOrientation/Affichage/Text").GetComponent<Text> ();
				Text affichageG = go.transform.Find ("PanelG/Affichage/Text").GetComponent<Text> ();
				Text affichageDrag = go.transform.Find ("PanelDrag/Affichage/Text").GetComponent<Text> ();

				tp.GQueue.Enqueue (gc.acceleration);
				sliderG.value = Mathf.Abs(getQueueMean (tp.GQueue));
				affichageG.text =  Mathf.Abs(getQueueMean (tp.GQueue)).ToString ("F2")+" G";


				dragSlider.value = Mathf.Sqrt (gc.drag);
				affichageDrag.text = dragSlider.value.ToString ("F2")+" G";

				orientationSlider.value = gc.earthOrientation;
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

				if (sliderG.value > gc.GAlertThreshold) {
					Color c = Color.red;
					c.a = tp.alphaAlertVal;
					GAlert.color = c;
				} else {
					Color c = Color.white;
					c.a = 100;
					GAlert.color = c;
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

				if (dragSlider.value > gc.DragAlertThreshold) {
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
