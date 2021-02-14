using AliceAppraisal.Engine.Strategy;
using System.Collections.Generic;

namespace AliceAppraisal.Engine {
	public interface IStrategyFactory {
		BaseStrategy GetStrategy(string fullName);
		BaseStrategy GetDefaultStrategy();
	}
}