using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using KeepKillingCULPRIT.doctrine.job.stacker;
using KeepKillingCULPRIT.doctrine.structures.abstracts;
using KeepKillingCULPRIT.doctrine.structures.enums;
using KeepKillingCULPRIT.doctrine.structures.sustains;
using KeepKillingCULPRIT.doctrine.transactions.derivatives;

namespace KeepKillingCULPRIT.doctrine.threads.grasses
{
	public class HostRestclient
	{
		private HostRestclientVoucher hostRestclientVoucher { get; set; }

		public HostRestclient()
		{
			try
			{
				Distributor distributor = new Distributor();

				HostRestclient savedHostRestclient = (HostRestclient)distributor.getTribute(Tribute.HostRestclient);
				if (savedHostRestclient == null)
				{
					// Value origins
					this.originCovers();

					distributor.addTribute(Tribute.HostRestclient, this);
				}
				else
				{
					this.hostRestclientVoucher = savedHostRestclient.hostRestclientVoucher;

					// Trial bypass
					if (!this.bypassCovers())
					{
						throw new Exception("Data not valid.");
					}
				}
			}
			catch (Exception exception)
			{
				throw new Exception("Constructor unsuccessful.", exception);
			}
		}

		private void originCovers()
		{
			this.hostRestclientVoucher = new HostRestclientVoucher();
		}

		private bool bypassCovers()
		{
			bool success = true;

			if (this.hostRestclientVoucher == null)
			{
				success = false;
			}

			return success;
		}

		public bool addVoucher(string voucherName, string seriesName, Restclient restclient)
		{
			bool success = false;

			try
			{
				HostRestclientCoupon coupon = new HostRestclientCoupon(restclient);
				if (this.getSeries(voucherName) == null)
				{
					Dictionary<string, HostRestclientCoupon> series = new Dictionary<string, HostRestclientCoupon>() { [seriesName] = coupon };
					this.hostRestclientVoucher.voucher.Add(voucherName, series);
					success = true;
				}
				else if (this.getCoupon(voucherName, seriesName) == null)
				{
					this.hostRestclientVoucher.voucher[voucherName].Add(seriesName, coupon);
					success = true;
				}
			}
			catch (Exception exception)
			{
				throw new Exception("Voucher or series addition unsuccessful.", exception);
			}

			return success;
		}

		public Dictionary<string, HostRestclientCoupon> getSeries(string voucherName)
		{
			if (this.hostRestclientVoucher.voucher.ContainsKey(voucherName))
			{
				return this.hostRestclientVoucher.voucher[voucherName];
			}
			return null;
		}

		public HostRestclientCoupon getCoupon(string voucherName, string seriesName)
		{
			if (this.getSeries(voucherName) != null)
			{
				if (this.hostRestclientVoucher.voucher[voucherName].ContainsKey(seriesName))
				{
					return this.hostRestclientVoucher.voucher[voucherName][seriesName];
				}
			}
			return null;
		}

		public async Task<string> seriesCheckinAsync(string voucherName, string seriesName)
		{
			string result = null;

			HostRestclientCoupon coupon = this.getCoupon(voucherName, seriesName);
			if (coupon != null)
			{
				string thisResult = null;
				try
				{
					coupon.invokedAt = DateTime.UtcNow;
					thisResult = await coupon.restclient.makeRequestAsync();
					coupon.updatedAt = DateTime.UtcNow;
					coupon.result = thisResult;
				}
				catch (Exception exception)
				{
					coupon.error.Add(exception.Message);
				}
				result = thisResult;
			}

			return result;
		}

		public bool seriesCheckoutContent(string voucherName, string seriesName, string content)
		{
			HostRestclientCoupon coupon = this.getCoupon(voucherName, seriesName);
			if (coupon != null)
			{
				return coupon.restclient.updateContent(content);
			}
			return false;
		}

		public bool seriesCheckoutTimeout(string voucherName, string seriesName, int? timeoutInMillisecond)
		{
			HostRestclientCoupon coupon = this.getCoupon(voucherName, seriesName);
			if (coupon != null)
			{
				return coupon.restclient.updateTimeout(timeoutInMillisecond);
			}
			return false;
		}

		public string seriesFetchEndpoint(string voucherName, string seriesName)
		{
			HostRestclientCoupon coupon = this.getCoupon(voucherName, seriesName);
			if (coupon != null)
			{
				return coupon.restclient.getEndpoint();
			}
			return null;
		}
	}
}
