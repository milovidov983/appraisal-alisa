using AliceAppraisal.Controllers;
using AliceAppraisal.Engine;
using AliceAppraisal.Engine.Strategy;
using Serilog;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AliceAppraisal.Application {
	public class HandlerFactory : IHandlerFactory {
		private readonly StrategyInitializerFactory strategyInitializerFactory;
		private readonly ILogger logger;

		public HandlerFactory() {
			IServiceFactory factory = ServiceFactoryBuilder.GetServiceFactory();
			logger = factory.GetLogger();
			strategyInitializerFactory = new StrategyInitializerFactory(factory);
		}

		public IMainHandler Create() {
			var strategyInitializer = strategyInitializerFactory.GetStrategyInitializer();
			var strategies = strategyInitializer.Strategies;
			var strategyFactory = strategyInitializer.StrategyFactory;
			
			return new Handler(strategyFactory, strategies, logger);
		}
	}
}
