using System;
using ChatApp.Services;
using ChatApp.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace ChatApp
{
	class MainClass
	{
		private static IServiceProvider serviceProvider;

		public static void Main(string[] args)
		{
			if (args.Length == 0 || !int.TryParse(args[0], out int port)) {
				throw new Exception("Port parameter is required!");
			}

			ConfigureServices();

			var app = new App(serviceProvider, new ClientStrategy(), port);
			if (Network.CheckIfPortIsOpen(port)) {
				app.RunningStrategy = new ServerStrategy();
			}

			app.Start();
		}

		private static void ConfigureServices()
		{
			var services = new ServiceCollection();

			services.AddSingleton<IDisplay, Display>();

			serviceProvider = services.BuildServiceProvider();
		}
	}
}
