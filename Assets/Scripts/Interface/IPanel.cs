/// <summary>
/// Interface to be attached to each panel
/// </summary>
public interface IPanel
{
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
