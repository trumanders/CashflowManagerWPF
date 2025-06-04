namespace Cash_Flow_Management.Services;

public class WindowService : IWindowService
{
	public bool? Open(Window window)
	{
		return window.ShowDialog();
	}

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
