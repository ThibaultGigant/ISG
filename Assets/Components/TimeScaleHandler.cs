using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TimeScaleHandler : MonoBehaviour {

	[HideInInspector]
	public int currentTimeScale = 0;
	public List<float> timeScales = new List<float>(){1.0f,2.0f,5.0f,10.0f,20.0f,50.0f};

}
