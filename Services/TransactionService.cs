using Cash_Flow_Management.Models;

namespace Cash_Flow_Management;

public class TransactionService
{
	List<Transaction> _transactions = [];
	Dictionary<DateTime, List<Transaction>> _monthlyTransactions;
}
