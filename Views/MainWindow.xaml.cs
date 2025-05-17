

namespace Cash_Flow_Management
{
    public partial class MainWindow : Window
    {
        public MainWindow(ICategoryService categoryService, IWindowService windowService, ITransactionService transactionService)
        {
			var mainWindowViewModel = new MainWindowViewModel(categoryService, windowService, transactionService);
			this.DataContext = mainWindowViewModel;
            InitializeComponent();
        }
    }
}