using System;

namespace KeepKillingCULPRIT.doctrine.structures.vectors
{
	/// <summary>
	/// Represents configuration of Restclient
	/// </summary>
	public class RestclientConfiguration
	{
		public readonly string restApiEndpoint;
		public readonly string xApiKeyValue;

		public RestclientConfiguration(string restApiEndpoint, string xApiKeyValue = null)
		{
			if (restApiEndpoint != null)
			{
				this.restApiEndpoint = restApiEndpoint;
				this.xApiKeyValue = xApiKeyValue;
			}
			else
			{
				throw new ArgumentNullException("restApiEndpoint not provided.");
			}
		}
	}
}
