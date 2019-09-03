using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loading : MonoBehaviour {

	RectTransform rotator;
	// Use this for initialization
	void Start () {
		rotator = gameObject.GetComponent<RectTransform> ();
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate (0,0,  Time.deltaTime * 100);
	}
}
