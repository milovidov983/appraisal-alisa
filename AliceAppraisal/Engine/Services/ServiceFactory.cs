using AliceAppraisal.Application;
using AliceAppraisal.Engine.Strategy;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AliceAppraisal.Engine.Services {
	public class ServiceFactory : IServiceFactory {
		private IExternalService _externalService;
		public IExternalService GetExternalService() {
			return _externalService;
		}

		private static readonly object _lock = new object();
		public ServiceFactory(ILogger logger, IExternalService externalService) {
			_logger = logger;
			_externalService = externalService;
			if (strategyFactory is null) {
				lock (_lock) {
					if (strategyFactory is null) {
						InitStratagy();
					}
				}
			}
		}



		#region logger
		private static ILogger _logger; 
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

		public void Setup(IExternalService externalService) {
			_externalService = externalService;
		}


		#endregion
	}
}
