using AliceAppraisal.Application;
using AliceAppraisal.Core.Engine.Strategy;
using AliceAppraisal.Infrastructure;
using Serilog;
using System.Collections.Generic;

namespace AliceAppraisal.Core.Engine {
	public interface IServiceFactory : IStrategyInitializer {
		IExternalService GetExternalService();
		ILogger GetLogger();
		StepManager CreateStepManager();
		IStorageService GetStorageService();
	}
} 