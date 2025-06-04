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

	public decimal? TotalRevenues => FilteredTransactions
		.Where(x => x.Category?.Type == CategoryType.Revenue)
		.Sum(x => x.Amount);


	public decimal? TotalExpenses => FilteredTransactions
		.Where(x => x.Category?.Type == CategoryType.Expense)
		.Sum(x => x.Amount);

	public decimal? TotalCashFlow => TotalRevenues - TotalExpenses;

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

	public ObservableCollection<Transaction> AllTransactions => _transactionService.Transactions;

	public IEnumerable<Transaction> FilteredTransactions
	{
		get => AllTransactions
			.Where(x => (string.IsNullOrEmpty(SelectedYearFilter) || x.Date.Year.ToString() == SelectedYearFilter) &&
						(string.IsNullOrEmpty(SelectedMonthFilter) || x.Date.ToString("MMMM", CultureInfo.InvariantCulture) == SelectedMonthFilter));
	}

	public ObservableCollection<Category> Categories { get; set; } = [];

	public ObservableCollection<string> AvailableYearsInTransactionsToFilterBy =>
		[
			..AllTransactions
			.Select(x => x.Date.Year.ToString())
			.Distinct()
		];


	public ObservableCollection<string> AvailableMonthsInSelectedYear =>
		[
			.. AllTransactions
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

	private void InitializeCommands()
	{
		AddTransactionCommand = new RelayCommand(AddTransaction_Click);
		AddCategoryCommand = new RelayCommand(AddCategory_Click);
		ClearFiltersCommand = new RelayCommand(ClearFilter_Click);
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
			UpdateUI();	
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

	private void ClearFilter_Click()
	{
		SelectedMonthFilter = null;
		SelectedYearFilter = null;
		UpdateUI();
	}

	private void UpdateUI()
	{
		OnPropertyChanged(nameof(FilteredTransactions));

		OnPropertyChanged(nameof(AvailableYearsInTransactionsToFilterBy));
		OnPropertyChanged(nameof(AvailableMonthsInSelectedYear));

		OnPropertyChanged(nameof(SelectedYearFilter));
		OnPropertyChanged(nameof(SelectedMonthFilter));
		
		OnPropertyChanged(nameof(TotalRevenues));
		OnPropertyChanged(nameof(TotalExpenses));
		OnPropertyChanged(nameof(TotalCashFlow));
	}
}
