using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;

public class HomeScreen : PanelBase
{
	public List<GraphNode> graphNode;
	public List<string> graphDateList;
	List<int> valueList;
	 
	public Dropdown dropdown;
	public string dozeValue;
	public Text userGraphText;

	List<string> alcoholConsumed;

	public GameObject popup;
	public Text popupText;

	public GameObject graph;

	void Awake ()
	{
		graph.SetActive (false);
		userGraphText.gameObject.SetActive (true);
		userGraphText.text = "Fetching Data...";
		alcoholConsumed = new List<string> () {"0",
			"2",
			"4",
			"6",
			"8",
			"10",
			"12",
			"14",
			"16",
			"18",
			"20",
			"20"
		};

	}


	void Start()
	{
		StartCoroutine ("GetDoseData");
	}

	public void ActivatePopup(string data)
	{
		popup.SetActive (true);
		popupText.text = data;
	}

	public void DeactivatePopup()
	{
		popup.SetActive (false);
	}

	void InitializeGraph ()
	{		
		graphDateList = new List<string> ();
		DateTime currentday = DateTime.Now;
		graphDateList.Add (String.Format ("{0:dd/MM}", currentday));

		DateTime date = currentday;
		for (int i = 0; i < 19; i++) {
			DateTime temp = date.AddDays (-1);
			graphDateList.Add (String.Format ("{0:dd/MM}", temp));
			date = temp;
		}

		graphDateList.Reverse ();

		for (int i = (graphNode.Count-1); i >= 0; i--) {
			graphNode [i].UpdateNode (valueList [i], graphDateList [i].ToString ());
		}
		userGraphText.gameObject.SetActive (false);
		graph.SetActive (true);
	}

	public void GoToSettings ()
	{
		ScreenManager.Instance.Activate<SettingScreen> ();
	}

	public void AddDose ()
	{		
		dozeValue = alcoholConsumed [dropdown.value];
	}

	public void AddDoseButton ()
	{
		if (dozeValue == "0") {
			
		} else {
			StartCoroutine (UpdateDose (dozeValue));
			dropdown.value = 0;
		}
	}


	private IEnumerator UpdateDose (string value)
	{
		AppManager.Instance.loading.SetActive (true);
		DoseClass dose = new DoseClass ();
		dose.user_dose = value;

		string infoText = JsonUtility.ToJson (dose).ToString ();

		WWWForm form = new WWWForm ();
		form.AddField ("uid", AppManager.Instance.phoneNumber);
		form.AddField ("info", infoText);

		using (UnityWebRequest www = UnityWebRequest.Post ("https://www.zhfbaa.cn/api/addict/input?appid=addict&access_token=0000&sign=12345", form)) {
			yield return www.SendWebRequest ();

			if (www.isNetworkError || www.isHttpError) {
				Debug.Log (www.error);
				AppManager.Instance.loading.SetActive (false);
				ActivatePopup ("Network Error!");
			} else {
				string responseText = www.downloadHandler.text;
				Debug.Log (responseText);
				DoseCallBack callback = new DoseCallBack ();
				callback = JsonUtility.FromJson<DoseCallBack> (responseText);

				if (callback.errmsg == "OK") {
					AppManager.Instance.loading.SetActive (false);
					graph.SetActive (false);
					userGraphText.gameObject.SetActive (true);
					StartCoroutine ("GetDoseData");
					ActivatePopup ("Succcessfully Updated!");
				} else {
					AppManager.Instance.loading.SetActive (false);
					ActivatePopup ("Network Error!");
					ActivatePopup ("Network Error. Please Try again!");
				}
			}
		}
	}

	private IEnumerator GetDoseData ()
	{
		DateTime today = DateTime.Now;
		DateTime endday = today.AddDays (-20);

		DoseData dose = new DoseData ();
		dose.end_time = String.Format ("{0:dd-MM-yyyy}", today);
		dose.start_time = String.Format ("{0:dd-MM-yyyy}", endday);

		string infoText = JsonUtility.ToJson (dose).ToString ();
		WWWForm form = new WWWForm ();
		form.AddField ("uid", AppManager.Instance.phoneNumber);
		form.AddField ("info", infoText);

		using (UnityWebRequest www = UnityWebRequest.Post ("https://www.zhfbaa.cn/api/addict/getdose?appid=addict&access_token=0000&sign=12345", form)) {
			yield return www.SendWebRequest ();

			if (www.isNetworkError || www.isHttpError) {
				Debug.Log (www.error);
				GetDoseData ();
			} else {
				string responseText = www.downloadHandler.text;
				Debug.Log (responseText);
				DoseDataCallBack callback = new DoseDataCallBack ();
				callback = JsonUtility.FromJson<DoseDataCallBack> (responseText);
				calldose = callback;
				if (callback.errmsg == "OK") {
					if (callback.List.Count != 0) {
						List<int> temp = new List<int> ();
						valueList = new List<int> ();
						for (int i = 0; i < callback.List.Count; i++) {
							temp.Add(callback.List [i].dose); 
						}
						temp.Reverse ();
						for (int i = 0; i < 20; i++) {
							valueList.Add (temp[i]);
						}
						valueList.Reverse ();
						InitializeGraph ();
					}
				} else {

				}
			}
		}
		
	}

	public DoseDataCallBack calldose;
}

public class DoseClass
{
	public string user_dose;
}

public class DoseData
{
	public string start_time;
	public string end_time;
}

public class DoseCallBack
{
	public int errno;
	public string errmsg;
	public double logid;
}

[Serializable]
public class DoseDataCallBack
{
	public int errno;
	public string errmsg;
	public List<List> List;
	public long logid;
}

[Serializable]
public class List
{
	public int time;
	public int dose;
}
