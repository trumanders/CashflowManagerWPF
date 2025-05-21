namespace Cash_Flow_Management.Services;

public class WindowService : IWindowService
{
	/// <summary>
	/// Opens a window
	/// </summary>
	/// <param name="window">The Window instance to open</param>
	/// <returns>True if </returns>
	public bool? Open(Window window)
	{
		return window.ShowDialog();
	}

	/// <summary>
	/// Closes the active window and sets the passed in result.
	/// </summary>
	/// <param name="result">The passed in result.</param>
	public void CloseWithResult(bool result)
	{
		var window = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);

		if (window != null)
		{
			window.DialogResult = result;
			window.Close();
		}
	}
}
