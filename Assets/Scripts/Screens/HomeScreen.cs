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

	List<string> alcoholConsumed;

	void Awake ()
	{
		alcoholConsumed = new List<string> () {"0",
			"1",
			"2",
			"3",
			"4",
			"5",
			"6",
			"7",
			"8",
			"9",
			"10"
		};

		valueList = new List<int> () {
			5,
			98,
			56,
			45,
			30,
			22,
			17,
			15,
			13,
			17,
			25,
			37,
			40,
			36,
			33,
			5,
			98,
			56,
			45,
			3
		};
	}

	void Start ()
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

		for (int i = (graphNode.Count - 1); i >= 0; i--) {
			graphNode [i].UpdateNode (valueList [i], graphDateList [i].ToString ());
		}

		StartCoroutine ("GetDoseData");
	}

	public void GoToSettings ()
	{
		ScreenManager.Instance.Activate<SettingScreen> ();
	}

	//https://www.zhfbaa.cn/api/addict/input?sign=""&access_token=""&uid=1234567890&appid=addict&info={"user_dose":"30"}

	public void AddDose ()
	{		
		dozeValue = alcoholConsumed [dropdown.value];
	}

	public void AddDoseButton ()
	{
		StartCoroutine (UpdateDose (dozeValue));
	}


	private IEnumerator UpdateDose (string value)
	{
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
			} else {
				string responseText = www.downloadHandler.text;

				DoseCallBack callback = new DoseCallBack ();
				callback = JsonUtility.FromJson<DoseCallBack> (responseText);

				if (callback.errmsg == "OK") {
					
				} else {
					
				}
			}
		}
	}

	private IEnumerator GetDoseData ()
	{
		DateTime today = DateTime.Now;
		DateTime endday = today.AddDays (-21);

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
			} else {
				string responseText = www.downloadHandler.text;

				DoseDataCallBack callback = new DoseDataCallBack ();
				callback = JsonUtility.FromJson<DoseDataCallBack> (responseText);
				calldose = callback;
				if (callback.errmsg == "OK") {
					//Debug.Log (callback.List.Count);
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
	public string _id;
	public int uid;
	public string appid;
	public string user_dose;
	public int _ctime;
	public int _mtime;
}
