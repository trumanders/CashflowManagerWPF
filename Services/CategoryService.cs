namespace Cash_Flow_Management;

public class CategoryService : ICategoryService
{
	public HashSet<Category> Categories { get; set; } = [];
	public event Action<Category>? CategoryAdded;
	public event Action<Category>? CategoryRemoved;

	public CategoryService()
	{
		AddCategoryTestData();
	}

	/// <summary>
	/// Adds the passed in category to the hash set of categories
	/// </summary>
	/// <param name="category">The Category instance to add.</param>
	public void AddCategory(Category category)
	{
		if (category != null)
		{
			var numberOfCategoriesBeforeAdding = Categories.Count;
			Categories.Add(category);

			if (Categories.Count > numberOfCategoriesBeforeAdding)
				CategoryAdded?.Invoke(category);
		}
	}

	/// <summary>
	/// Removes the passed in category from the hash set of categories
	/// </summary>
	/// <param name="category">The Category instance to remove.</param>
	public void RemoveCategory(Category category)
	{
		if (category != null)
		{
			Categories.Remove(category);
			CategoryRemoved?.Invoke(category);
		}
	}

	/// <summary>
	/// Adds category test data to the collection
	/// </summary>
	private void AddCategoryTestData()
	{
		AddCategory(new Category { Name = "Food", Type = CategoryType.Expense });
		AddCategory(new Category { Name = "Car", Type = CategoryType.Expense });
		AddCategory(new Category { Name = "Salary", Type = CategoryType.Revenue });
		AddCategory(new Category { Name = "Electricity", Type = CategoryType.Expense });
		AddCategory(new Category { Name = "Phone", Type = CategoryType.Expense });
		AddCategory(new Category { Name = "Child allowance", Type = CategoryType.Revenue });
		AddCategory(new Category { Name = "Gifts", Type = CategoryType.Expense });
		AddCategory(new Category { Name = "Entertainment", Type = CategoryType.Expense });
		AddCategory(new Category { Name = "Outdoor", Type = CategoryType.Expense });
		AddCategory(new Category { Name = "Clothing", Type = CategoryType.Expense });
	}
}
