namespace Cash_Flow_Management.Interfaces;

public interface ITransactionService
{
	public ObservableCollection<Transaction> Transactions { get; set; }

	public Action? TransactionAdded { get; set; }

	public void AddTransaction(Transaction transaction);
}
