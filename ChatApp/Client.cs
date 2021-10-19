using System;
using System.Net.WebSockets;
using System.Text.Json;
using ChatApp.Models;
using ChatApp.Services;
using Websocket.Client;

namespace ChatApp
{
	public class ClientStrategy : IRunningStrategy
	{
		private string _userName;
		private IDisplay _display;

		public void Start(IDisplay display, int port)
		{
			_display = display;

			_display.WriteToOutput("Enter an user name:");
			_userName = _display.ReadInput();

			var clientFactory = new Func<ClientWebSocket>(() => {
				var nativeClient = new ClientWebSocket();
				nativeClient.Options.SetRequestHeader("userName", _userName);
				return nativeClient;
			});
			using var client = new WebsocketClient(new Uri("ws://127.0.0.1:" + port), clientFactory) {
				ReconnectTimeout = null
			};
			client.MessageReceived.Subscribe(message => {
				ChatMessage chatMessage = JsonSerializer.Deserialize<ChatMessage>(message.Text);
				_display.WriteToOutput(chatMessage.ToString());
			});
			client.Start();

			const string helpMessagePrompt =
				"Enter your message and press ENTER to send it" +
				"\nIf you wish to close the app, enter the \"exit\" command and press ENTER.";
			_display.WriteToOutput(helpMessagePrompt);
			var input = _display.ReadInput();
			while (input != "exit") {
				switch (input) {
					case "help":
						_display.WriteToOutput(helpMessagePrompt);
						break;
					default:
						if (input != "") {
							client.Send(input);
						}
						break;
				}
				input = _display.ReadInput();
			}
		}
	}
}
