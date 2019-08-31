using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GraphNode : MonoBehaviour
{

	const int factor = 6;

	public Text date;

	public	RectTransform rect;

	public void UpdateNode (int value, string data)
	{
		rect.sizeDelta = new Vector2 (50, value * factor);
		date.text = data;
	}
}
