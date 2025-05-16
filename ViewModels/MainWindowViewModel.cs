using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;

namespace Cash_Flow_Management.ViewModels;

public partial class MainWindowViewModel : ObservableObject, INotifyPropertyChanged
{
	private readonly ICategoryService _categoryService;
	private readonly IWindowService _windowService;
	[ObservableProperty]
	DateTime _date;
	[ObservableProperty]
	private string? _amount;
	[ObservableProperty]
	private string? _description;
	[ObservableProperty]
	private Category _category;

	public ObservableCollection<Category> Categories { get; set; } = [];
	//public ObservableCollection<Transaction> Transactions => _transactionService.Transactions;
	public ICommand AddTransactionCommand { get; set; }
	public ICommand AddCategoryCommand { get; set; }
	public MainWindowViewModel(ICategoryService categoryService, IWindowService windowService)
	{
		_categoryService = categoryService;
		_windowService = windowService;
		SubscribeToEvents();
		PopulateCategoriesWithGeneratedTestData();
		InitializeCommands();
	}

	private void InitializeCommands()
	{
		AddTransactionCommand = new RelayCommand(AddTransaction_Click);
		AddCategoryCommand = new RelayCommand(AddCategory_Click);
	}

	private void AddTransaction_Click()
	{

	}

	private void AddCategory_Click()
	{
		var addCategoryWindow = new AddCategoryWindow(_windowService);

		if (_windowService.Open(addCategoryWindow) == true)
		{
			_categoryService.AddCategory(addCategoryWindow.Result);
		}
	}

	private void SubscribeToEvents()
	{
		_categoryService.CategoryAdded += AddCategory;
		_categoryService.CategoryRemoved += RemoveCategory;
	}
	private void AddCategory(Category category)
	{
		Categories.Add(category);
	}

	private void RemoveCategory(Category category)
	{
		Categories.Remove(category);
	}

	private void PopulateCategoriesWithGeneratedTestData()
	{
		foreach (var category in _categoryService.Categories)
		{
			AddCategory(category);
		}
	}	
}
