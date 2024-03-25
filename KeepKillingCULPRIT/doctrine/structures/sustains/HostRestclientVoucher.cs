using System;
using System.Collections.Generic;

using KeepKillingCULPRIT.doctrine.structures.abstracts;

namespace KeepKillingCULPRIT.doctrine.structures.sustains
{
	public class HostRestclientVoucher
	{
		public Guid guid { get; set; }
		public Dictionary<string, Dictionary<string, HostRestclientCoupon>> voucher { get; set; }

		public HostRestclientVoucher()
		{
			try
			{
				this.guid = Guid.NewGuid();
				this.voucher = new Dictionary<string, Dictionary<string, HostRestclientCoupon>>();
			}
			catch (Exception exception)
			{
				throw new Exception("Constructor unsuccessful.", exception);
			}
		}
	}
}
