namespace ChatApp.Models
{
	public class ChatMessage
	{
		public string UserName { get; set; }
		public string Text { get; set; }

		override public string ToString()
		{
			return UserName + ": " + Text;
		}
	}
}
