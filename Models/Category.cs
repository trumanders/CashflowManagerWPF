namespace Cash_Flow_Management.Models;

public record Category
{
	private string? _name;
	public string? Name
	{
		get => _name;
		set
		{
			if (!string.IsNullOrEmpty(value))
			{
				_name = value;
			}
		}
	}

	public CategoryType Type { get; set; }
}


