using AliceAppraisal.Engine.Strategy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AliceAppraisal.Engine.Services {
	public class StrategyFactory : IStrategyFactory {
		private Dictionary<string, BaseStrategy> Stratagies { get; set; }

		public StrategyFactory(Dictionary<string, BaseStrategy> stratagies) {
			Stratagies = stratagies;
		}


		public BaseStrategy GetStrategy(string fullName) {
			Stratagies.TryGetValue(fullName, out var stratagy);
			return stratagy;
		}
	}
}
