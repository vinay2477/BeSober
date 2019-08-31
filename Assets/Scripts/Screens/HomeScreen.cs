using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HomeScreen : PanelBase
{
	public List<GraphNode> graphNode;
	public List<int> graphDateList;
	List<int> valueList;

	void Awake ()
	{
		valueList = new List<int> () { 5, 98, 56, 45, 30, 22, 17, 15, 13, 17, 25, 37, 40, 36, 33, 5, 98, 56, 45, 3 };
	}

	void Start ()
	{
		graphDateList = new List<int> ();
		DateTime currentday = DateTime.Now;
		graphDateList.Add (currentday.Day);

		DateTime date = currentday;
		for (int i = 0; i < 19; i++) {
			DateTime temp = date.AddDays (-1);
			graphDateList.Add (temp.Day);
			date = temp;
		}

		graphDateList.Reverse ();

		for (int i = (graphNode.Count - 1); i >= 0; i--) {
			graphNode [i].UpdateNode (valueList [i], graphDateList [i].ToString ());
		}
	}

	public void GoToSettings()
	{
		ScreenManager.Instance.Activate<SettingScreen> ();
	}
}
