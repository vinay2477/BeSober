using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Networking;

public class AppManager : MonoBehaviour
{

	public static AppManager Instance;

	public string phoneNumber;
	public string Name;
	public string email;
	public string password;
	[SerializeField]
	public DateTime lastSeen;
	[SerializeField]
	public DateTime lastUpdated;
	public DateTime lastFeedback;
	public int latestFeedbackScore;

	public UserData userdata;

	public GameObject loading;

	public int[] positiveQues;
	public int[] negativeQues;

	public List<Image> posImg1;
	public List<Image> posImg2;
	public List<Image> posImg3;
	public List<Image> posImg4;
	public List<Image> posImg5;

	public List<Image> negImg1;
	public List<Image> negImg2;
	public List<Image> negImg3;
	public List<Image> negImg4;
	public List<Image> negImg5;

	public void SetUserData(string phone, string name, string mail, string pwd)
	{
		phoneNumber = phone;
		Name = name;
		email = mail;
		password = pwd;
		SetPlayerPrefs();
	}

	void Awake()
	{
		//Check if instance already exists
		if (Instance == null)
			{
				//if not, set instance to this
				Instance = this;
			}
		//If instance already exists and it's not this:
		else if (Instance != this)
			{
				//Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
				Destroy(gameObject);    
			}
		//Sets this to not be destroyed when reloading scene
		DontDestroyOnLoad(gameObject);
		positiveQues = new int[5];
		negativeQues = new int[5];
	}

	public void SetPlayerPrefs()
	{
		UserData data = new UserData();
		data.email = email;
		data.phoneNumber = phoneNumber;
		data.Name = Name;
		data.password = password;
		data.isLoggedIn = true;
		data.lastSeen = lastSeen.ToString();
		data.lastUpdated = lastUpdated.ToString();
		data.latestFeedbackScore = latestFeedbackScore;
		data.lastFeedback = lastFeedback.ToString();
		Debug.Log("set players pref " + JsonUtility.ToJson(data).ToString());
		PlayerPrefs.SetString("SoberAI", JsonUtility.ToJson(data).ToString());
	}

	public void FetchUserData()
	{
		StartCoroutine("GetUserData");
	}


	public GetUserCallBack callback;

	IEnumerator GetUserData()
	{
		GetUserApi userdata = new GetUserApi();
		userdata.user_phone = phoneNumber;
		string infoText = JsonUtility.ToJson(userdata).ToString();

		WWWForm form = new WWWForm();
		form.AddField("uid", phoneNumber);
		form.AddField("info", infoText);

		using (UnityWebRequest www = UnityWebRequest.Post("https://www.zhfbaa.cn/api/addict/signin?appid=addict&access_token=0000&sign=12345&uid=", form))
			{
				yield return www.SendWebRequest();

				if (www.isNetworkError || www.isHttpError)
					{
						Debug.Log(www.error);
					} else
					{
						string responseText = www.downloadHandler.text;
						Debug.Log(responseText);

						callback = new GetUserCallBack();
						callback = JsonUtility.FromJson<GetUserCallBack>(responseText);
						Debug.Log(responseText);
						if (callback.errmsg == "OK")
							{
								Debug.Log(callback.info.user_phone);
								Debug.Log(callback.info.user_name);
								Debug.Log(callback.info.user_email);
								Debug.Log(callback.info.user_pwd);
								SetUserData(callback.info.user_phone, callback.info.user_name, callback.info.user_email, callback.info.user_pwd);

							} else
							{
					
							}
					}
			}
	}

	public void ToggleButton(BTType type, int quesno)
	{
		switch (type)
			{
			case BTType.pos:
				{
					switch (quesno)
						{
						case 1:
							{
								for (int i = 0; i < posImg1.Count; i++)
									{
										WhiteButton(posImg1[i]);					
									}
							}
							break;
						case 2:
							{
								foreach (Image i in posImg2)
									{
										WhiteButton(i);						
									}
							}
							break;
						case 3:
							{
								foreach (Image i in posImg3)
									{
										WhiteButton(i);						
									}
							}
							break;
						case 4:
							{
								foreach (Image i in posImg4)
									{
										WhiteButton(i);						
									}
							}
							break;
						case 5:
							{
								foreach (Image i in posImg5)
									{
										WhiteButton(i);						
									}
							}
							break;
						}
				}
				break;
			case BTType.neg:
				{
					switch (quesno)
						{
						case 1:
							{
								foreach (Image i in negImg1)
									{
										WhiteButton(i);						
									}
							}
							break;
						case 2:
							{
								foreach (Image i in negImg2)
									{
										WhiteButton(i);						
									}
							}
							break;
						case 3:
							{
								foreach (Image i in negImg3)
									{
										WhiteButton(i);						
									}
							}
							break;
						case 4:
							{
								foreach (Image i in negImg4)
									{
										WhiteButton(i);						
									}
							}
							break;
						case 5:
							{
								foreach (Image i in negImg5)
									{
										WhiteButton(i);						
									}
							}
							break;
						}			
				}
				break;
			}	
	}

	public void ColorButton(Image img)
	{
		img.color = new Color(1, (float)(204 / 255), (float)(153 / 255));
	}

	public void WhiteButton(Image img)
	{
		img.color = new Color(1, 1, 1);
	}

	public void CalculateLatestFeedbackScore()
	{
		int sum1 = 0, sum2 = 0;
		for (int i = 0; i < 5; i++)
			{
				sum1 += positiveQues[i];	
				sum2 += negativeQues[i];	
			}	
		latestFeedbackScore = sum1 - sum2 + 50;
		lastFeedback = DateTime.Today;
		SetPlayerPrefs();
	}
}

[Serializable]
public class UserData
{
	public string phoneNumber;
	public string Name;
	public string email;
	public string password;
	public bool isLoggedIn;
	public string lastSeen;
	public string lastUpdated;
	public string lastFeedback;
	public int latestFeedbackScore;
}

public class GetUserApi
{
	public string user_phone;
}

[Serializable]
public class GetUserCallBack
{
	public int errno;
	public string errmsg;
	public double logid;
	public Info info;
}

[Serializable]
public class Info
{
	public string _id;
	public int uid;
	public string appid;
	public string user_name;
	public string user_pwd;
	public string user_email;
	public string user_phone;
	public int _ctime;
	public int _mtime;
}
