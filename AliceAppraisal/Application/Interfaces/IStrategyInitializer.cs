using AliceAppraisal.Core.Engine;
using AliceAppraisal.Core.Engine.Strategy;
using System.Collections.Generic;

namespace AliceAppraisal.Application {
	public interface IStrategyInitializer {
		IStrategyFactory StrategyFactory { get; }
		List<BaseStrategy> Strategies { get; }
	}
}