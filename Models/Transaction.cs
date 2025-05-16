namespace Cash_Flow_Management.Models;

public record Transaction
{
	DateTime Date { get; set; }
	decimal Amount { get; set; }
	Category Category { get; set; }
	string? Description { get; set; }
}
