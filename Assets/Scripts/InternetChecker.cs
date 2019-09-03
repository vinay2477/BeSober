using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InternetChecker : MonoBehaviour {

	public GameObject popup;
	
	public float waitTime = 1f;

	float timer;

	void Update () 
	{
		timer += Time.deltaTime;
		if (timer > waitTime) { 
			if (Application.internetReachability == NetworkReachability.NotReachable) {
				popup.SetActive (true);
			} else {
				popup.SetActive (false);
			}
			timer = 0f;
		}
	}

}
