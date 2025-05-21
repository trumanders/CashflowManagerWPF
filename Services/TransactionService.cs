namespace Cash_Flow_Management;

public class TransactionService : ITransactionService
{
	private CategoryService categoryService = new CategoryService();
	public ObservableCollection<Transaction> Transactions { get; set; } = [];

	public Dictionary<DateTime, List<Transaction>> _monthlyTransactions;
	public Action? TransactionAdded { get; set; }
	
	public void AddTransaction(Transaction transaction)
	{
		if (transaction != null)
		{
			Transactions.Add(transaction);
			TransactionAdded?.Invoke();
		}
	}

	public void AddTestData()
	{
		foreach (var transaction in new List<Transaction>()
		{
			new Transaction
			{
				Date = new DateTime(2023, 10, 1),
				Amount = 3000,
				Description = "New steering wheel",
				Category = categoryService.Categories.First(x => x.Name == "Car")
			},
			new Transaction
			{
				Date = new DateTime(2023, 12, 21),
				Amount = 2700,
				Description = "Christmas presents for the kids",
				Category = categoryService.Categories.First(x => x.Name == "Gifts")
			},
			new Transaction
			{
				Date = new DateTime(2023, 12, 31),
				Amount = 540,
				Description = "Fireworks",
				Category = categoryService.Categories.First(x => x.Name == "Entertainment")
			},
			new Transaction
			{
				Date = new DateTime(2024, 1, 12),
				Amount = 4500,
				Description = "New snowboard",
				Category = categoryService.Categories.First(x => x.Name == "Outdoor")
			},
			new Transaction
			{
				Date = new DateTime(2024, 4, 11),
				Amount = 360,
				Description = "Lots of eggs",
				Category = categoryService.Categories.First(x => x.Name == "Food")
			},
			new Transaction
			{
				Date = new DateTime(2024, 5, 25),
				Amount = 154000,
				Description = "Salary",
				Category = categoryService.Categories.First(x => x.Name == "Salary")
			},
			new Transaction
			{
				Date = new DateTime(2025, 6, 20),
				Amount = 200,
				Description = "Schnapps",
				Category = categoryService.Categories.First(x => x.Name == "Entertainment")
			},
			new Transaction
			{
				Date = new DateTime(2024, 6, 30),
				Amount = 149,
				Description = "Sun glasses",
				Category = categoryService.Categories.First(x => x.Name == "Clothing")
			},

			new Transaction
			{
				Date = new DateTime(2024, 10, 21),
				Amount = 430,
				Description = "Mobile phone",
				Category = categoryService.Categories.First(x => x.Name == "Phone")
			},
			new Transaction
			{
				Date = new DateTime(2024, 10, 24),
				Amount = 1500,
				Description = "New warm jacket",
				Category = categoryService.Categories.First(x => x.Name == "Clothing")
			}
		})
		{
			AddTransaction(transaction);
		}
	}
}
