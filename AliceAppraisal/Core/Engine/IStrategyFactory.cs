using AliceAppraisal.Core.Engine.Strategy;
using System.Collections.Generic;

namespace AliceAppraisal.Core.Engine {
	public interface IStrategyFactory {
		BaseStrategy GetStrategy(string fullName);
		BaseStrategy GetDefaultStrategy();
	}
}