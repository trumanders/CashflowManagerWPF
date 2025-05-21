

namespace Cash_Flow_Management
{
    public partial class MainWindow : Window
    {
		public MainWindow(MainWindowViewModel mainWindowViewModel)
        {
			this.DataContext = mainWindowViewModel;
            InitializeComponent();
        }
    }
}