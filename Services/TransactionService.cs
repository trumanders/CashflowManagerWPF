namespace Cash_Flow_Management;

public class TransactionService : ITransactionService
{
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
}
