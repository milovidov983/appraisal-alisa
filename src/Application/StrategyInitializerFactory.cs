using AliceAppraisal.Core.Engine;
using AliceAppraisal.Core.Engine.Services;
using AliceAppraisal.Core.Engine.Strategy;
using System.Collections.Generic;
using System.Linq;

namespace AliceAppraisal.Application {
	public class StrategyInitializerFactory {
		private IStrategyInitializer strategyInitializerInstance;

		public StrategyInitializerFactory(IServiceFactory serviceFactory) {
			strategyInitializerInstance = new StrategyInitializer(serviceFactory);
			
		}

		public IStrategyInitializer GetStrategyInitializer() {
			return strategyInitializerInstance;
		}

		private class StrategyInitializer : IStrategyInitializer {
			public IStrategyFactory StrategyFactory { get; private set; }
			public List<BaseStrategy> Strategies { get; private set; }


			private readonly IServiceFactory serviceFactory;
			
			public StrategyInitializer(IServiceFactory serviceFactory) {
				this.serviceFactory = serviceFactory;
				StartInitialization();
			}


			private void StartInitialization() {
				Strategies = ReflectiveEnumerator.GetEnumerableOfType<BaseStrategy>(serviceFactory).ToList();
				StrategyFactory = new StrategyFactory(Strategies.ToDictionary(
					x => x.GetType().FullName,
					x => x
				));
			}

		}
	}
}