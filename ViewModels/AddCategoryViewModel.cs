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

	/// <summary>
	/// Handles the Ok-button click. Passes true to the WindowService close-method.
	/// </summary>
	private void Ok_Click()
	{
		Result.Name = CategoryName;
		Result.Type = CategoryType;
		_windowService.CloseWithResult(true);
	}

	/// <summary>
	/// Handles the Cancel-button click. Passes false to the WindowService close-method
	/// </summary>
	private void Cancel_Click()
	{
		_windowService.CloseWithResult(false);
	}

	/// <summary>
	/// Initializes the commands
	/// </summary>
	private void InitializeCommands()
	{
		OkClickedCommand = new RelayCommand(Ok_Click);
		CancelClickedCommand = new RelayCommand(Cancel_Click);
	}
}
