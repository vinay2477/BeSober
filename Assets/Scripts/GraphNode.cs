using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GraphNode : MonoBehaviour
{

	const int factor = 5;

	public Text date;
	public Text graphScore;

	public	RectTransform rect;


	public void UpdateNode (int value, string data)
	{
		rect.sizeDelta = new Vector2 (40, value * factor);
		date.text = data;
		graphScore.color = new Color(0.1568f, 0.6274f, 0.999f, 0.0f);
		string nodeVale = value.ToString () + " oz";
		graphScore.text = nodeVale;
	}

	public void NodeClick()
	{
		graphScore.color = new Color(0.1568f, 0.6274f, 0.999f, 1.0f);
		StartCoroutine ("FadeOff");
	}

	IEnumerator FadeOff()
	{
		yield return new WaitForSeconds (2);
		graphScore.color = new Color(0.1568f, 0.6274f, 0.999f, 0.0f);
	}
}
