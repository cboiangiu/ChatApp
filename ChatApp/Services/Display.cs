using System;

namespace ChatApp.Services
{
	public class Display : IDisplay
	{
		public string ReadInput()
		{
			return Console.ReadLine().Trim();
		}

		public void WriteToOutput(string message)
		{
			Console.WriteLine(message);
		}
	}

	public interface IDisplay
	{
		string ReadInput();
		void WriteToOutput(string message);
	}
}
