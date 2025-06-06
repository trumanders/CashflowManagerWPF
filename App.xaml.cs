﻿namespace Cash_Flow_Management;

public partial class App : Application
{
	public static IHost? AppHost { get; private set; }

	public App()
	{
		AppHost = Host.CreateDefaultBuilder()
			.ConfigureServices((services) =>
			{
				services.AddSingleton<MainWindow>();
				services.AddSingleton<MainWindowViewModel>();
				services.AddSingleton<ICategoryService, CategoryService>();
				services.AddSingleton<IWindowService, WindowService>();
				services.AddSingleton<ITransactionService, TransactionService>();
			})
			.Build();
	}

	protected override async void OnStartup(StartupEventArgs e)
	{
		await AppHost!.StartAsync();
		var viewModel = AppHost.Services.GetRequiredService<MainWindowViewModel>();
		var startupForm = AppHost.Services.GetRequiredService<MainWindow>();
		startupForm.DataContext = viewModel;
		startupForm.Show();
		base.OnStartup(e);
	}

	protected override async void OnExit(ExitEventArgs e)
	{
		await AppHost!.StopAsync();
		base.OnExit(e);
	}
}
