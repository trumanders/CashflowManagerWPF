using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections;
using System.ComponentModel;

namespace Cash_Flow_Management.ViewModels;

public partial class MainWindowViewModel : ObservableObject, INotifyPropertyChanged
{
	private readonly ICategoryService _categoryService;
	private readonly IWindowService _windowService;
	private readonly ITransactionService _transactionService;

	[ObservableProperty]
	DateTime? _date;
	[ObservableProperty]
	private string? _amount;
	[ObservableProperty]
	private string? _description;
	[ObservableProperty]
	private Category? _category;
	[ObservableProperty]
	public decimal? _totalRevenues;
	[ObservableProperty]
	public decimal? _totalExpenses;
	[ObservableProperty]
	public decimal? _totalCashFlow;

	public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

	public ObservableCollection<Category> Categories { get; set; } = [];
	public ObservableCollection<Transaction> Transactions => _transactionService.Transactions;
	public ICommand AddTransactionCommand { get; set; }
	public ICommand AddCategoryCommand { get; set; }

	public MainWindowViewModel(ICategoryService categoryService, IWindowService windowService, ITransactionService transactionService)
	{
		_categoryService = categoryService;
		_windowService = windowService;
		_transactionService = transactionService;
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
		if (!ValidateAllFieldsFilled())
		{
			MessageBox.Show("Please enter all fields to add transaction.", "Input error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
			return;
		}
		
		if (ValidateInputs())
		{
			_transactionService.AddTransaction(new Transaction()
			{
				Date = Date!.Value,
				Amount = decimal.Parse(Amount!),
				Description = Description,
				Category = Category!
			});
		}

	}

	private void AddCategory_Click()
	{
		var addCategoryWindow = new AddCategoryWindow(_windowService);

		if (_windowService.Open(addCategoryWindow) == true)
		{
			_categoryService.AddCategory(addCategoryWindow.Result);
			Category = addCategoryWindow.Result;
		}
	}

	private void SubscribeToEvents()
	{
		_categoryService.CategoryAdded += OnAddCategory;
		_categoryService.CategoryRemoved += OnRemoveCategory;
		_transactionService.TransactionAdded += OnTransactionAdded;
	}

	private void OnAddCategory(Category category)
	{
		Categories.Add(category);
	}

	private void OnRemoveCategory(Category category)
	{
		Categories.Remove(category);
	}

	private void PopulateCategoriesWithGeneratedTestData()
	{
		foreach (var category in _categoryService.Categories)
		{
			OnAddCategory(category);
		}
	}

	private bool ValidateAllFieldsFilled()
	{
		return
			Date != null &&
			Amount != null &&
			Category != null &&
			!string.IsNullOrEmpty(Description);
	}

	private bool ValidateInputs()
	{
		if (!decimal.TryParse(Amount, out decimal value))
		{
			MessageBox.Show("Invalid amount value!", "Amount", MessageBoxButton.OK, MessageBoxImage.Exclamation);
			return false;
		}

		if (value < 0)
		{
			MessageBox.Show("You entered a negative amount.\nPlease use the correct category type, or create a new category instead.", "Negative amount", MessageBoxButton.OK, MessageBoxImage.Information);
			return false;
		}

		return true;
	}

	private void OnTransactionAdded()
	{
		TotalExpenses = Transactions
			.Where(x => x.Category.Type == CategoryType.Expense)
			.Sum(x => x.Amount);

		TotalRevenues = Transactions
			.Where(x => x.Category.Type == CategoryType.Revenue)
			.Sum(x => x.Amount);

		TotalCashFlow = TotalRevenues - TotalExpenses;
	}

	public IEnumerable GetErrors(string? propertyName)
	{
		throw new NotImplementedException();
	}
}
