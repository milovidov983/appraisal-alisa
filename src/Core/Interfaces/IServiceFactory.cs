using AliceAppraisal.Infrastructure;
using System.Collections.Generic;

namespace AliceAppraisal.Core.Engine {
	public interface IStrategyInitializer {
		IStrategyFactory StrategyFactory { get; }
		List<BaseStrategy> Strategies { get; }
	}
	public interface IServiceFactory : IStrategyInitializer {
		IAppraisalProvider GetDataProvider();
		ILoggerFactory GetLoggerFactory();
		IStorageService GetStorageService();
		IStepService GetStepService();
		IVehicleModelService GetVehicleModelService();
		IManufactureYearService GetManufactureYearService();
	}
} 