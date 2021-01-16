using AliceAppraisal.Engine;
using AliceAppraisal.Engine.Services;
using AliceAppraisal.Engine.Strategy;
using Serilog;
using System;

namespace AliceAppraisal.Application {
	public sealed class ServiceFactoryBuilder {
		private IServiceFactory serviceFactory;
		private IExternalService externalService;

		public ServiceFactoryBuilder() { }

		public void SetExternalService(IExternalService externalService) {
			this.externalService = externalService;
		}

		public void InitServiceFactory(ILogger logger = null, IExternalService externalService = null) {
			if(logger is null) {
				logger = new LoggerConfiguration()
					.WriteTo
					.Console()
					.MinimumLevel
					.Debug()
					.CreateLogger();
			}
			externalService ??= this.externalService ?? new ExternalService();
			serviceFactory = new ServiceFactory(logger, externalService ?? this.externalService);
		}

		private static readonly object _lock = new object();
		public IServiceFactory GetServiceFactory() {
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
