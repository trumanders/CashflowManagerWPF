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

	// The total revenues in the filtered transactions
	public decimal? TotalRevenues => FilteredTransactions
		.Where(x => x.Category?.Type == CategoryType.Revenue)
		.Sum(x => x.Amount);


	// The total expenses in the filtered transactions
	public decimal? TotalExpenses => FilteredTransactions
		.Where(x => x.Category?.Type == CategoryType.Expense)
		.Sum(x => x.Amount);

	// The total cash flow in the filtered transactions
	public decimal? TotalCashFlow => TotalRevenues - TotalExpenses;

	// The selected month to filter the transaction by
	private string? _selectedMonthFilter;
	public string? SelectedMonthFilter
	{
		get => _selectedMonthFilter;
		set
		{
			if (_selectedMonthFilter != value)
			{
				_selectedMonthFilter = value;
				UpdateUI();
			}
		}
	}

	// The selected year to filter the transactions by
	private string? _selectedYearFilter;
	public string? SelectedYearFilter
	{
		get => _selectedYearFilter;
		set
		{
			if (_selectedYearFilter != value)
			{
				_selectedYearFilter = value;
				UpdateUI();

			}
		}
	}

	// The complete collection of transactions fetched from TransactionService
	public ObservableCollection<Transaction> Transactions => _transactionService.Transactions;


	// Aggreagates the transactions to match the selected filters
	public IEnumerable<Transaction> FilteredTransactions
	{
		get => Transactions
			.Where(x => (string.IsNullOrEmpty(SelectedYearFilter) || x.Date.Year.ToString() == SelectedYearFilter) &&
						(string.IsNullOrEmpty(SelectedMonthFilter) || x.Date.ToString("MMMM", CultureInfo.InvariantCulture) == SelectedMonthFilter));
	}
	
	// Collection of Categories to choose from
	public ObservableCollection<Category> Categories { get; set; } = [];

	// The collection of available years to filter by in the transaction collection
	public ObservableCollection<string> FilterByAvailableYearsInTransactions =>
		[
			..Transactions
			.Select(x => x.Date.Year.ToString())
			.Distinct()
		];


	// The collection of months to choose from for the selected year filter for transactions
	public ObservableCollection<string> FilterByMonthsInSelectedYearFilter =>
		[
			.. Transactions
			.Where(x => string.IsNullOrEmpty(SelectedYearFilter) || x.Date.Year.ToString() == SelectedYearFilter)
			.Select(x => x.Date.ToString("MMMM", CultureInfo.InvariantCulture))
			.Distinct()
		];



	public ICommand? AddTransactionCommand { get; set; }
	public ICommand? AddCategoryCommand { get; set; }
	public ICommand? ClearFiltersCommand { get; set; }

	public MainWindowViewModel(ICategoryService categoryService, IWindowService windowService, ITransactionService transactionService)
	{
		_categoryService = categoryService;
		_windowService = windowService;
		_transactionService = transactionService;
		SubscribeToEvents();
		_transactionService.AddTestData();
		PopulateCategoriesWithGeneratedTestData();

		InitializeCommands();
	}

	/// <summary>
	/// Initializes the commands
	/// </summary>
	private void InitializeCommands()
	{
		AddTransactionCommand = new RelayCommand(AddTransaction_Click);
		AddCategoryCommand = new RelayCommand(AddCategory_Click);
		ClearFiltersCommand = new RelayCommand(ClearFilter_Click);
	}

	/// <summary>
	/// Handle add transaction button click. Validates input and updates the UI
	/// </summary>
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
			UpdateUI();	
		}
	}

	/// <summary>
	/// Handles add category button click. Calls window service to open new window of type AddCategoryWindow
	/// </summary>
	private void AddCategory_Click()
	{
		var addCategoryWindow = new AddCategoryWindow(_windowService);

		if (_windowService.Open(addCategoryWindow) == true)
		{
			_categoryService.AddCategory(addCategoryWindow.Result);
			Category = addCategoryWindow.Result;
		}
	}

	/// <summary>
	/// Subscribes to events
	/// </summary>
	private void SubscribeToEvents()
	{
		_categoryService.CategoryAdded += OnAddCategory;
		_categoryService.CategoryRemoved += OnRemoveCategory;
	}

	/// <summary>
	/// Adds category to the category collection
	/// </summary>
	/// <param name="category">The category instance to add</param>
	private void OnAddCategory(Category category)
	{
		Categories.Add(category);
	}

	/// <summary>
	/// Removes category from the category collection
	/// </summary>
	/// <param name="category">The category instance to remove</param>
	private void OnRemoveCategory(Category category)
	{
		Categories.Remove(category);
	}

	/// <summary>
	/// Add test data to the application
	/// </summary>
	private void PopulateCategoriesWithGeneratedTestData()
	{
		foreach (var category in _categoryService.Categories)
		{
			OnAddCategory(category);
		}
	}

	/// <summary>
	/// Checks if all fields are filled.
	/// </summary>
	/// <returns>True if all fields are filled, otherwise false</returns>
	private bool ValidateAllFieldsFilled()
	{
		return
			Date != null &&
			Amount != null &&
			Category != null &&
			!string.IsNullOrEmpty(Description);
	}

	/// <summary>
	/// Checks for invalid input in the UI
	/// </summary>
	/// <returns></returns>
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

	/// <summary>
	/// Clears the selected transactions filters
	/// </summary>
	private void ClearFilter_Click()
	{
		SelectedMonthFilter = null;
		SelectedYearFilter = null;
		UpdateUI();
	}

	/// <summary>
	/// Calls OnPropertyChanged for all UI elements.
	/// </summary>
	private void UpdateUI()
	{
		OnPropertyChanged(nameof(FilteredTransactions));

		OnPropertyChanged(nameof(FilterByAvailableYearsInTransactions));
		OnPropertyChanged(nameof(FilterByMonthsInSelectedYearFilter));

		OnPropertyChanged(nameof(SelectedYearFilter));
		OnPropertyChanged(nameof(SelectedMonthFilter));
		
		OnPropertyChanged(nameof(TotalRevenues));
		OnPropertyChanged(nameof(TotalExpenses));
		OnPropertyChanged(nameof(TotalCashFlow));
	}
}
