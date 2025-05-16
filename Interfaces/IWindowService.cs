namespace Cash_Flow_Management.Interfaces;

public interface IWindowService
{
	bool? Open(Window window);
	void CloseWithResult(bool result);
}
