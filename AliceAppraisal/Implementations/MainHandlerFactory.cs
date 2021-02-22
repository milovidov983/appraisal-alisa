using AliceAppraisal.Core.Engine;

namespace AliceAppraisal.Application {
	public class MainHandlerFactory : IMainHandlerFactory {
		private static IMainHandlerFactory instanse;
		private readonly static object _lockInstanse = new object();
		private readonly IMainHandler mainHandler;
		private MainHandlerFactory(IServiceFactory serviceFactory) {
			var strategies = serviceFactory.Strategies;
			var strategyFactory = serviceFactory.StrategyFactory;
			var logger = serviceFactory.GetLoggerFactory().GetLogger();
			mainHandler = new MainHandler(strategyFactory, strategies, logger);
		}

		public IMainHandler GetHandler() { 
			return mainHandler;
		}

		public static IMainHandlerFactory Create(IServiceFactory serviceFactory) {
			if (instanse != null) {
				return instanse;
			}
			lock (_lockInstanse) {
				if (instanse is null) {
					instanse = new MainHandlerFactory(serviceFactory);
				}
			}
			return instanse;
		}
	}
}
