namespace Cash_Flow_Management.Views;

public partial class AddCategoryWindow : Window
{
	public Category Result => ((AddCategoryViewModel)DataContext).Result;
	public AddCategoryWindow(IWindowService windowService)
	{
		var addCategoryWindowViewModel = new AddCategoryViewModel(windowService);
		this.DataContext = addCategoryWindowViewModel;
		InitializeComponent();
	}
}
