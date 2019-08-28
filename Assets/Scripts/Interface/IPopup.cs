using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface  IPopup {

	/// <summary>
	/// Activate the Panel
	/// </summary>
	void Activate ();

	/// <summary>
	/// Deactivate the Panel
	/// </summary>
	void Deactivate ();

	/// <summary>
	/// OnBackButton functionality
	/// </summary>
	void OnBackKeyPressed ();
}
