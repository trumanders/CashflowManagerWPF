namespace Cash_Flow_Management.Models;

public record Transaction
{
	public DateTime Date { get; set; }
	public decimal Amount { get; set; }
	public Category? Category { get; set; }
	public string? Description { get; set; }
}
