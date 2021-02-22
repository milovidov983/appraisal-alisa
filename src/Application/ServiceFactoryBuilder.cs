using AliceAppraisal.Application.Infrastructure;
using AliceAppraisal.Core.Engine;
using AliceAppraisal.Core.Engine.Services;
using AliceAppraisal.Core.Engine.Strategy;
using Serilog;
using System;

namespace AliceAppraisal.Application {
	public sealed class ServiceFactoryBuilder {
		private static readonly ServiceFactoryBuilder instanse = new ServiceFactoryBuilder();
		private static readonly object _lock = new object();
		private IServiceFactory serviceFactory;

		private void InitServiceFactory(IAppraisalProvider appraisalProvider = null) {
			var loggerFactory = LoggerFactory.Create();
			try {
				appraisalProvider ??= new DataProviderService();
				serviceFactory = new ServiceFactory(loggerFactory, appraisalProvider);
			} catch(Exception e) {
				var logger = loggerFactory.GetLogger();
				logger.Error(e, $"{nameof(ServiceFactoryBuilder)}: {e.Message}");
			}
		}


		public IServiceFactory GetServiceFactory(IAppraisalProvider appraisalProvider = null) {
			if(serviceFactory is null) {
				lock (_lock) {
					if(serviceFactory is null) {
						InitServiceFactory(appraisalProvider);
					}
				}
			}
			return serviceFactory;
		}
		public static ServiceFactoryBuilder Create() {
			return instanse;
		}

	}
}
