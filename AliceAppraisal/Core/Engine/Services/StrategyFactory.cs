using AliceAppraisal.Core.Engine.Strategy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AliceAppraisal.Core.Engine.Services {
	public class StrategyFactory : IStrategyFactory {
		private Dictionary<string, BaseStrategy> Stratagies { get; set; }

		public StrategyFactory(Dictionary<string, BaseStrategy> stratagies) {
			Stratagies = stratagies;
		}


		public BaseStrategy GetStrategy(string fullName) {
			if(fullName is null) {
				return default;
			}
			Stratagies.TryGetValue(fullName, out var stratagy);
			return stratagy;
		}
		public BaseStrategy GetDefaultStrategy() {
			Stratagies.TryGetValue(typeof(WhatCanYouDoStrategy).FullName, out var stratagy);
			return stratagy;
		}

	}
}
