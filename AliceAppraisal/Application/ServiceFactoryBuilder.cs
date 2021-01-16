using AliceAppraisal.Engine;
using AliceAppraisal.Engine.Services;
using Serilog;

namespace AliceAppraisal.Application {
	public static class ServiceFactoryBuilder {
		private static IServiceFactory serviceFactory;
		public static void InitServiceFactory(ILogger logger = null) {
			if(logger is null) {
				logger = new LoggerConfiguration()
					.WriteTo
					.Console()
					.MinimumLevel
					.Debug()
					.CreateLogger();
			}
			serviceFactory = new ServiceFactory(logger);
		}

		public static void InitServiceFactory(IServiceFactory serviceFactory) {
			ServiceFactoryBuilder.serviceFactory = serviceFactory;
		}
		private static readonly object _lock = new object();
		public static IServiceFactory GetServiceFactory() {
			if(serviceFactory is null) {
				lock (_lock) {
					if(serviceFactory is null) {
						InitServiceFactory();
					}
				}
			}
			return serviceFactory;
		}
	}
}
