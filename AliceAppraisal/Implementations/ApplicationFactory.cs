using AliceAppraisal.Application.Configuration;
using AliceAppraisal.Application.Infrastructure;
using AliceAppraisal.Core.Engine;
using AliceAppraisal.Implementations.Infrastructure;
using AliceAppraisal.Infrastructure;
using Serilog;

namespace AliceAppraisal.Application {
	public class ApplicationFactory : IApplicationFactory {
		private static readonly object _lock = new object();
		private readonly Settings settings = Settings.Instance;
		private readonly IServiceFactory factory;
		private readonly ILogger logger;

		private IStorageService storageService;


		public ApplicationFactory() {
			factory = new ServiceFactoryBuilder().GetServiceFactory();
			logger = factory.GetLogger();
		}
		public ApplicationFactory(IServiceFactory serviceFactory) {
			factory = serviceFactory;
			logger = factory.GetLogger();
		}

		public IMainHandler CreateHandler() {
			var strategies = factory.Strategies;
			var strategyFactory = factory.StrategyFactory;

			return new MainHandler(strategyFactory, strategies, logger);
		}



		public IStorageService GetStorage() {
			if (storageService is null) {
				lock (_lock) {
					if (storageService is null) {
						storageService = new SpreadsheetStorageService(settings.SheetConfig);
					}
				}
			}
			return storageService;

		}
	}
}
