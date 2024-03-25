using System;
using System.Collections.Generic;

using KeepKillingCULPRIT.doctrine.job.stacker;

namespace KeepKillingCULPRIT.doctrine.structures.abstracts
{
	public class HostRestclientCoupon
	{
		public DateTime createdAt { get; set; }
		public DateTime? invokedAt { get; set; }
		public Restclient restclient { get; set; }
		public List<string> error { get; set; }
		public DateTime? updatedAt { get; set; }
		public string result { get; set; }

		public HostRestclientCoupon(Restclient restclient)
		{
			if (restclient != null)
			{
				this.restclient = restclient;
				this.createdAt = DateTime.UtcNow;
				this.error = new List<string>();
				this.result = null;
			}
			else
			{
				throw new ArgumentNullException("Restclient not provided.");
			}
		}
	}
}
