using System.Collections.Generic;
using System.Text.Json;
using ChatApp.Models;
using ChatApp.Services;
using Fleck;

namespace ChatApp
{
	public class ServerStrategy : IRunningStrategy
	{
		private readonly List<IWebSocketConnection> _clients = new();
		private IDisplay _display;

		public void Start(IDisplay display, int port)
		{
			_display = display;

			using var server = new WebSocketServer("ws://127.0.0.1:" + port);
			server.Start(socket => {
				socket.OnOpen = () => {
					_clients.Add(socket);
					ChatMessage chatMessage = new() {
						UserName = "Admin",
						Text = "Welcome " + GetUserName(socket) + "!",
					};
					SendMessageToClients(chatMessage);
					_display.WriteToOutput(chatMessage.ToString());
				};
				socket.OnClose = () => {
					_clients.Remove(socket);
					ChatMessage chatMessage = new() {
						UserName = "Admin",
						Text = "Goodbye " + GetUserName(socket) + "!",
					};
					SendMessageToClients(chatMessage);
					_display.WriteToOutput(chatMessage.ToString());
				};
				socket.OnMessage = message => {
					ChatMessage chatMessage = new() {
						UserName = GetUserName(socket),
						Text = message,
					};
					SendMessageToClients(chatMessage);
					_display.WriteToOutput(chatMessage.ToString());
				};
			});

			const string exitMessagePrompt = "Enter \"exit\" command to close server!";
			_display.WriteToOutput(exitMessagePrompt);
			var input = _display.ReadInput();
			while (input != "exit") {
				_display.WriteToOutput(exitMessagePrompt);
				input = _display.ReadInput();
			}
		}

		private void SendMessageToClients(ChatMessage chatMessage)
		{
			foreach (var client in _clients) {
				client.Send(JsonSerializer.Serialize(chatMessage));
			}
		}

		private static string GetUserName(IWebSocketConnection socket)
		{
			if (socket.ConnectionInfo.Headers.ContainsKey("userName")) {
				return socket.ConnectionInfo.Headers["userName"];
			}

			return "unknown";
		}
	}
}
