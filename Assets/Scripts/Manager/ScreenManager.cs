using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ScreenManager : MonoBehaviour
{

	public static ScreenManager Instance;

	public PanelBase[] UIPanels;

	[HideInInspector]
	public PanelBase currentPanel;

	void Awake ()
	{
		//Check if instance already exists
		if (Instance == null) {
			//if not, set instance to this
			Instance = this;
		}
		//If instance already exists and it's not this:
		else if (Instance != this) {
			//Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
			Destroy (gameObject);    
		}
		//Sets this to not be destroyed when reloading scene
		DontDestroyOnLoad (gameObject);
	}

	// Use this for initialization
	void Start ()
	{
		ScreenManager.Instance.Activate<SplashScreen> ();
	}

	public void Activate<T> () where T : PanelBase
	{
		for (int i = 0; i < UIPanels.Length; i++) {
			UIPanels [i].Deactivate ();
		}
		T popup = GetPanel<T> ();
		popup.OnShow ();
		if (currentPanel != null)
			currentPanel.OnHide ();
		currentPanel = popup;
	}

	public void DeActivate<T> () where T : PanelBase
	{
		T popup = GetPanel<T> ();
		popup.OnHide ();
	}

	/// <summary>
	/// Gets the panel object given the type of panel
	/// </summary>
	/// <returns>The panel.</returns>
	/// <typeparam name="T">Panel Type.</typeparam>
	public T GetPanel<T> () where T : PanelBase
	{
		return System.Array.Find (UIPanels, t => t.GetType ().Name == typeof(T).Name) as T;
	}


}
