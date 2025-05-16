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

	public void RemoveCategory(Category category)
	{
		if (category != null)
		{
			Categories.Remove(category);
			CategoryRemoved?.Invoke(category);
		}
	}

	private void AddCategoryTestData()
	{
		AddCategory(new Category { Name = "Food", Type = CategoryType.Expense });
		AddCategory(new Category { Name = "Car", Type = CategoryType.Expense });
		AddCategory(new Category { Name = "Salary", Type = CategoryType.Revenue });
		AddCategory(new Category { Name = "Electricity", Type = CategoryType.Expense });
		AddCategory(new Category { Name = "Phone", Type = CategoryType.Expense });
		AddCategory(new Category { Name = "Child allowance", Type = CategoryType.Revenue });
	}
}
