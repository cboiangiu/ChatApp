using System;
using ChatApp.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ChatApp
{
	public class App
	{
		private readonly int _port;
		private readonly IDisplay _display;

		public IRunningStrategy RunningStrategy { get; set; }

		public App(IServiceProvider services, IRunningStrategy runningStrategy, int port)
		{
			_display = services.GetService<IDisplay>();

			RunningStrategy = runningStrategy;
			_port = port;
		}

		public void Start()
		{
			RunningStrategy.Start(_display, _port);
		}
	}

	public interface IRunningStrategy
	{
		void Start(IDisplay _display, int port);
	}
}
