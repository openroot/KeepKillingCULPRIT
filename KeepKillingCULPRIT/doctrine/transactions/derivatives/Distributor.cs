using System;
using System.Windows;

using KeepKillingCULPRIT.doctrine.structures.enums;

namespace KeepKillingCULPRIT.doctrine.transactions.derivatives
{
	public class Distributor
	{
		public Distributor(Tribute? tribute = null, object content = null)
		{
			if (tribute != null && content != null)
			{
				this.addTribute((Tribute)tribute, content);
			}
		}

		public bool addTribute(Tribute tribute, object content)
		{
			bool success = false;

			try
			{
				if (!Application.Current.Properties.Contains(tribute) && content != null)
				{
					Application.Current.Properties.Add(tribute, content);
					success = true;
				}
			}
			catch (Exception exception)
			{
				throw new Exception("'tribute' did not add.", exception);
			}

			return success;
		}

		public object getTribute(Tribute tribute)
		{
			object content = null;

			try
			{
				if (Application.Current.Properties.Contains(tribute))
				{
					content = Application.Current.Properties[tribute];
				}
			}
			catch (Exception exception)
			{
				throw new Exception("Unable to get 'tribute'.", exception);
			}

			return content;
		}
	}
}
