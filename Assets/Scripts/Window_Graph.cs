using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey.Utils;

public class Window_Graph : MonoBehaviour
{

	private RectTransform graphContainer;
	[SerializeField]private Sprite circleSprite;

	// Use this for initialization
	void Awake ()
	{
		graphContainer = transform.Find ("graphContainer").GetComponent<RectTransform> ();

		List<int> valueList = new List<int> () { 5, 98, 56, 45, 30, 22, 17, 15, 13, 17, 25, 37, 40, 36, 33,5, 98, 56, 45, 30, 22, 17, 15, 13, 17, 25, 37, 40, 36, 33 };
		ShowGraph (valueList);
	}

	private GameObject CreateCircle (Vector2 anchoredPosition)
	{
		GameObject gameObject = new GameObject ("circle", typeof(Image));
		gameObject.transform.SetParent (graphContainer, false);
		gameObject.GetComponent<Image> ().sprite = circleSprite;
		RectTransform rectTransform = gameObject.GetComponent<RectTransform> ();
		rectTransform.anchoredPosition = anchoredPosition;
		rectTransform.sizeDelta = new Vector2 (11, 11);
		rectTransform.anchorMin = new Vector2 (0, 0);
		rectTransform.anchorMax = new Vector2 (0, 0);
		return gameObject;
	}

	private void ShowGraph (List<int> valueList)
	{
		float graphHeight = graphContainer.sizeDelta.y;
		float yMaximum = 100f;
		float xSize = 50f;
		GameObject lastCirc = null;

		for (int i = 0; i < valueList.Count; i++) {
			float xPoistion = xSize + i * xSize;
			float yPosition = (valueList [i] / yMaximum) * graphHeight;
			GameObject circleGameObject = CreateCircle (new Vector2 (xPoistion, yPosition));
			if (lastCirc != null) {
				CreateDotConnection (lastCirc.GetComponent<RectTransform> ().anchoredPosition, circleGameObject.GetComponent<RectTransform> ().anchoredPosition);
			}
			lastCirc = circleGameObject;
		}
	}

	private void CreateDotConnection (Vector2 dotPoistionA, Vector2 dotPoistionB)
	{
		GameObject gameObject = new GameObject ("dotConnection", typeof(Image));
		gameObject.transform.SetParent (graphContainer, false);
		gameObject.GetComponent<Image> ().color = new Color (1, 1, 1, 0.5f);
		RectTransform rectTransform = gameObject.GetComponent<RectTransform> ();
		Vector2 dir = (dotPoistionB - dotPoistionA).normalized;
		float distance = Vector2.Distance (dotPoistionA,dotPoistionB);
		rectTransform.anchorMin = new Vector2 (0, 0);
		rectTransform.anchorMax = new Vector2 (0, 0);
		rectTransform.sizeDelta = new Vector2 (distance, 3f);
		rectTransform.anchoredPosition = dotPoistionA + dir * distance * 0.5f;
		rectTransform.localEulerAngles = new Vector3 (0, 0, UtilsClass.GetAngleFromVectorFloat (dir));
	}
}
