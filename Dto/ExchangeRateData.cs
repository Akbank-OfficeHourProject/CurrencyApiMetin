using System.Collections.Generic;
using System;

namespace CurrencyApi.Dto
{
	public class ExchangeRateData
	{
		public DateTime Date { get; set; }
		public Dictionary<string, decimal> Rates { get; set; }
	}
}
