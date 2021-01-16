using AliceAppraisal.Application;
using AliceAppraisal.Engine.Strategy;
using Serilog;
using System.Collections.Generic;

namespace AliceAppraisal.Engine {
	public interface IServiceFactory : IStrategyInitializer {
		IExternalService GetExternalService();
		ILogger GetLogger();
		void Setup(IExternalService externalService);
	}
} 