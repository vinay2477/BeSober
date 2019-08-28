using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

/// <summary>
/// Base class for all panels
/// </summary>
public abstract class PanelBase : MonoBehaviour, IPanel
{
	/// <summary>
	/// Activate the Panel
	/// </summary>
	public virtual void Activate ()
	{
		//			Debug.Log ("Activate - " + GetType ().Name);
		gameObject.SetActive (true);
	}

	/// <summary>
	/// Deactivate the Panel
	/// </summary>
	public virtual void Deactivate ()
	{
		//			Debug.Log ("Deactivate - " + GetType ().Name);
		gameObject.SetActive (false);
	}

	/// <summary>
	/// Called after panel is activated
	/// </summary>
	public virtual void OnShow ()
	{
		Activate ();
	}

	/// <summary>
	/// Called before panel is deactivated
	/// </summary>
	public virtual void OnHide ()
	{
		Deactivate ();
	}

	/// <summary>
	/// Called on back or escape key pressed
	/// </summary>
	public virtual void OnBackKeyPressed ()
	{
		Debug.Log ("On Back Key - " + GetType ().Name);
	}
}