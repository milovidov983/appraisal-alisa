using AliceAppraisal.Controllers;
using AliceAppraisal.Engine;
using AliceAppraisal.Engine.Strategy;
using Serilog;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AliceAppraisal.Application {
	public class HandlerFactory : IHandlerFactory {
		private readonly IServiceFactory factory;
		private readonly ILogger logger;


		public HandlerFactory() {
			factory = new ServiceFactoryBuilder().GetServiceFactory();
			logger = factory.GetLogger();
		}
		public HandlerFactory(IServiceFactory serviceFactory) {
			factory = serviceFactory;
			logger = factory.GetLogger();
		}

		public IMainHandler Create() {
			var strategies = factory.Strategies;
			var strategyFactory = factory.StrategyFactory;

			return new MainHandler(strategyFactory, strategies, logger);
		}
	}
}
