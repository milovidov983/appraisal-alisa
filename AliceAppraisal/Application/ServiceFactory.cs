using AliceAppraisal.Application;
using AliceAppraisal.Core.Engine;
using AliceAppraisal.Core.Engine.Strategy;
using AliceAppraisal.Infrastructure;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AliceAppraisal.Application {

	public class ServiceFactory : IServiceFactory {
		private IExternalService _externalService;
		public IExternalService GetExternalService() {
			return _externalService;
		}

		private static readonly object _lock = new object();
		public ServiceFactory(ILogger logger, IExternalService externalService) {
			_logger = logger;
			_externalService = externalService;

			InitStratagy();
			
		}



		#region logger
		private ILogger _logger; 
		public ILogger GetLogger() {
			return _logger;
		}
		#endregion

		#region strategy
		private IStrategyFactory strategyFactory;
		public IStrategyFactory StrategyFactory { 
			get => strategyFactory; 
			set => strategyFactory = value; 
		}
		public List<BaseStrategy> Strategies { get; private set; }
		public void InitStratagy() {
			var initFacotry = new StrategyInitializerFactory(this);
			var initializer = initFacotry.GetStrategyInitializer();
			StrategyFactory = initializer.StrategyFactory;
			Strategies = initializer.Strategies;
		}

		#endregion

		StepManager stepManager;
		public StepManager CreateStepManager() {
			if(stepManager is null) {
				lock (_lock) {
					if(stepManager is null) {
						stepManager = new StepManager(this);
					}
				}
			}
			return stepManager;
		}

		public IStorageService GetStorageService() {
			throw new NotImplementedException();
		}
	}
}
