using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

using KeepKillingCULPRIT.doctrine.threads.grasses;

namespace KeepKillingCULPRIT
{
	public partial class App : Application
	{
		public App()
		{
			// Value origin
			this.originCovers();
		}

		private void originCovers()
		{
			new HostRestclient();
		}
	}
}
