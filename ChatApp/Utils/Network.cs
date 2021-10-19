using System.Net.NetworkInformation;

namespace ChatApp.Utils
{
	public class Network
	{
		public static bool CheckIfPortIsOpen(int port)
		{
			var ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
			var ipEndpoints = ipGlobalProperties.GetActiveTcpListeners();

			foreach (var ipEndpoint in ipEndpoints) {
				if (ipEndpoint.Port == port) {
					return false;
				}
			}

			return true;
		}
	}
}
