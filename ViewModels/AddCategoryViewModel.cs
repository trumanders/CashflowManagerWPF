namespace Cash_Flow_Management.ViewModels;

public class AddCategoryViewModel
{
	private readonly IWindowService _windowService;
	public string? CategoryName { get; set; }
	public CategoryType CategoryType { get; set; }
	public Category Result { get; private set; } = new();
	public ICommand? OkClickedCommand { get; set; }
	public ICommand? CancelClickedCommand { get; set; }

	public AddCategoryViewModel(IWindowService windowService)
	{
		_windowService = windowService;
		InitializeCommands();
	}

	private void Ok_Click()
	{
		Result.Name = CategoryName;
		Result.Type = CategoryType;
		_windowService.CloseWithResult(true);
	}

	private void Cancel_Click()
	{
		_windowService.CloseWithResult(false);
	}

	private void InitializeCommands()
	{
		OkClickedCommand = new RelayCommand(Ok_Click);
		CancelClickedCommand = new RelayCommand(Cancel_Click);
	}
}
