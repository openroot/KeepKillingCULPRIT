using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using KeepKillingCULPRIT.doctrine.structures.enums;
using KeepKillingCULPRIT.doctrine.structures.vectors;

namespace KeepKillingCULPRIT.doctrine.job.stacker
{
	/// <summary>
	/// REST client service
	/// </summary>
	public class Restclient
	{
		private readonly HttpVerb httpMethod;
		private readonly Uri endPoint;
		private readonly Dictionary<string, string> headers;
		private readonly string contentType;
		private string content { get; set; }
		private int? timeoutInMillisecond { get; set; }
		public readonly RestclientConfiguration restclientConfiguration;

		/// <summary>
		/// Constructs REST client
		/// </summary>
		/// <param name="restclientConfiguration">Configuration for REST client</param>
		/// <param name="httpMethod">Method to be used for REST call</param>
		/// <param name="apiPath">REST API path. It must be start with a Slash (/). E.g., /users</param>
		/// <param name="headers">HTTP request headers</param>
		/// <param name="contentType">Request content type. Defaulted to "application/json"</param>
		/// <param name="content">Request content</param>
		/// <param name="timeoutInMillisecond">Request timeout value in millisecond</param>
		public Restclient(RestclientConfiguration restclientConfiguration, HttpVerb httpMethod, string apiPath = null, Dictionary<string, string> headers = null, string contentType = null, string content = null, int? timeoutInMillisecond = null)
		{
			if (restclientConfiguration != null)
			{
				// Preparing headers
				string apiKeyName = "x-api-key";
				Dictionary<string, string> headersThis = null;
				if (headers == null)
				{
					headersThis = new Dictionary<string, string>() { { apiKeyName, restclientConfiguration.xApiKeyValue } };
				}
				else
				{
					headersThis = headers;
					if (!headersThis.ContainsKey(apiKeyName))
					{
						headersThis.Add(apiKeyName, restclientConfiguration.xApiKeyValue);
					}
				}

				this.restclientConfiguration = restclientConfiguration;
				this.httpMethod = httpMethod;
				this.endPoint = new Uri(restclientConfiguration.restApiEndpoint + apiPath, UriKind.Absolute);
				this.headers = headersThis;
				this.contentType = contentType ?? "application/json";
				this.content = content ?? string.Empty;
				this.timeoutInMillisecond = timeoutInMillisecond;
			}
			else
			{
				throw new ArgumentNullException("restclientConfiguration not provided.");
			}
		}

		/// <summary>
		/// Processes HTTP web request
		/// </summary>
		/// <param name="webRequest"></param>
		/// <returns>Response of the request</returns>
		private async Task<string> processWebRequestAsync(HttpWebRequest webRequest)
		{
			string responseValue = null;

			if (webRequest != null)
			{
				if (this.httpMethod != HttpVerb.GET)
				{
					// Write request content into web request stream
					try
					{
						using (Stream requestStream = await webRequest.GetRequestStreamAsync())
						{
							byte[] contentInBytes = Encoding.UTF8.GetBytes(this.content);
							requestStream.Write(contentInBytes, 0, contentInBytes.Length);
						}
					}
					catch (Exception exception)
					{
						throw new Exception("Web request content error. " + exception);
					}
				}

				// Make the web request and process the response
				HttpWebResponse webResponse = null;
				try
				{
					// Set a timer to abort the web requset on timeout (if any)
					CancellationTokenSource abortWebRequestTaskCancellationTokenSource = null;
					if (this.timeoutInMillisecond != null && this.timeoutInMillisecond > -1)
					{
						abortWebRequestTaskCancellationTokenSource = new CancellationTokenSource();
						this.abortWebRequestOnTimeoutAsync(webRequest, abortWebRequestTaskCancellationTokenSource.Token);
					}

					// Make the web request
					webResponse = (HttpWebResponse)await webRequest.GetResponseAsync();

					// Cancel the task for aborting web request now as it made the full cycle before the timeout timer got hit
					abortWebRequestTaskCancellationTokenSource?.Cancel();

					// Process the response stream
					using (Stream webResponseStream = webResponse.GetResponseStream())
					{
						if (webResponseStream != null)
						{
							// Read the response stream into string
							using (StreamReader reader = new StreamReader(webResponseStream))
							{
								responseValue = await reader.ReadToEndAsync();
							}
						}
					}
				}
				catch (WebException exception)
				{
					if (exception.Status == WebExceptionStatus.ProtocolError)
					{
						HttpStatusCode statusCode = ((HttpWebResponse)exception.Response).StatusCode;
						if (statusCode != HttpStatusCode.OK)
						{
							throw new Exception("Web request returned status was not 'OK' and status code was " + ((int)statusCode).ToString() + ".");
						}
					}
					else if (exception.Status == WebExceptionStatus.RequestCanceled)
					{
						throw new Exception("Web request was canceled or timed out to aborted.");
					}
					else
					{
						throw new Exception("Web request error in common and web-exception status was " + exception.Status.ToString() + ". ", exception);
					}
				}

				webResponse?.Dispose();
			}

			return responseValue;
		}

		/// <summary>
		/// Sets a timer to abort the web requset on timeout (if any)
		/// </summary>
		/// <param name="webRequest"></param>
		/// <param name="cancellationToken"></param>
		private async void abortWebRequestOnTimeoutAsync(HttpWebRequest webRequest, CancellationToken cancellationToken)
		{
			if (webRequest != null)
			{
				await Task.Run(async () =>
				{
					await Task.Delay(this.timeoutInMillisecond ?? 0); // Set timeout timer
					if (!cancellationToken.IsCancellationRequested)
					{
						webRequest.Abort(); // Abort the web request
					}
				}, cancellationToken); // Set it with cancellation token
			}
		}

		/// <summary>
		/// Makes HTTP request
		/// </summary>
		/// <returns>Response of the request (could be JSON, XML or HTML etc. serialized into string)</returns>
		public async Task<string> makeRequestAsync()
		{
			HttpWebRequest webRequest = null;

			// Create a HttpWebRequest instance for web request
			try
			{
				webRequest = (HttpWebRequest)WebRequest.Create(this.endPoint);
				webRequest.Method = this.httpMethod.ToString();
				webRequest.ContentType = this.contentType;
				if (this.headers?.Count > 0)
				{
					foreach (KeyValuePair<string, string> header in this.headers)
					{
						webRequest.Headers[header.Key] = header.Value;
					}
				}
			}
			catch (Exception exception)
			{
				throw new Exception("Web request handler 'HttpWebRequest' was not created. ", exception);
			}

			if (webRequest != null)
			{
				return await this.processWebRequestAsync(webRequest);
			}

			return null;
		}

		/// <summary>
		/// Updates request content
		/// </summary>
		/// <param name="content">Request content</param>
		/// <returns>Status</returns>
		public bool updateContent(string content)
		{
			if (!string.IsNullOrEmpty(content))
			{
				this.content = content;
				return true;
			}
			return false;
		}

		/// <summary>
		/// Updates request timeout
		/// </summary>
		/// <param name="timeoutInMillisecond">Timeout value in millisecond</param>
		/// <returns>Status</returns>
		public bool updateTimeout(int? timeoutInMillisecond)
		{
			if (timeoutInMillisecond != null && timeoutInMillisecond < 0)
			{
				return false;
			}
			this.timeoutInMillisecond = timeoutInMillisecond;
			return true;
		}

		/// <summary>
		/// Gets Endpoint of the REST client
		/// </summary>
		/// <returns>Absolute path</returns>
		public string getEndpoint()
		{
			return this.endPoint.AbsolutePath;
		}
	}
}
