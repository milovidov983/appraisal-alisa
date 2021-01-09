using AliceAppraisal.Engine.Strategy;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AliceAppraisal.Engine.Services {
	public class ServiceFactory : IServiceFactory {
		private static readonly IExternalService externalService = new ExternalService();
		public IExternalService GetExternalService() {
			return externalService;
		}

		private static readonly ITextGeneratorService textGeneratorService = new TextGenerator();
		public ITextGeneratorService GetTextGeneratorService() {
			return textGeneratorService;
		}

		private static IStrategyFactory strategyFactory;
		public IStrategyFactory GetStrategyFactory() {
			return strategyFactory;
		}

		private static ILogger logger;
		public ServiceFactory(ILogger log) {
			logger = log;
		}
		public ILogger GetLogger() {
			return logger;
		}


		public void InitStratagy(IEnumerable<BaseStrategy> all) {
			strategyFactory = new StrategyFactory(all.ToDictionary(
				x => x.GetType().FullName,
				x => x
				));
		}


	}
}
