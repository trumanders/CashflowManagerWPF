namespace Cash_Flow_Management.Interfaces;

public interface ICategoryService
{
	public HashSet<Category> Categories { get; set; }
	public void AddCategory(Category category);
	public void RemoveCategory(Category category);
	public event Action<Category>? CategoryAdded;
	public event Action<Category>? CategoryRemoved;
}