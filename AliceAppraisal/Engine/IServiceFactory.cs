using AliceAppraisal.Engine.Strategy;
using Serilog;
using System.Collections.Generic;

namespace AliceAppraisal.Engine {
	public interface IServiceFactory {
		IExternalService GetExternalService();
		IStrategyFactory GetStrategyFactory();
		ITextGeneratorService GetTextGeneratorService();
		ILogger GetLogger();
	}
} 