using AliceAppraisal.Application.Configuration;
using AliceAppraisal.Application.Infrastructure;
using AliceAppraisal.Application.Infrastructure.Models;
using AliceAppraisal.Core;
using AliceAppraisal.Core.Engine;
using AliceAppraisal.Implementations.Infrastructure;
using AliceAppraisal.Infrastructure;
using System;
using System.Collections.Generic;

namespace AliceAppraisal.Application {

	public class ServiceFactory : IServiceFactory {
		private IAppraisalProvider _dataProviderService;
		public IAppraisalProvider GetDataProvider() {
			return _dataProviderService;
		}

		private static readonly object _lock = new object();

		public ServiceFactory(ILoggerFactory loggerFactory, IAppraisalProvider externalService) {
			_loggerFactory = loggerFactory ?? LoggerFactory.Create();
			_dataProviderService = externalService;

			InitStratagy();
			
		}



		#region logger
		private ILoggerFactory _loggerFactory; 
		public ILoggerFactory GetLoggerFactory() {
			return _loggerFactory;
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

		#region StepManager

		IStepManager stepManager;
		public IStepManager GetStepManager() {
			if(stepManager is null) {
				lock (_lock) {
					if(stepManager is null) {
						stepManager = new StepManager(this);
					}
				}
			}
			return stepManager;
		}

		#endregion;
		private static readonly object _storageLock = new object();
		private IStorageService storageService;
		public IStorageService GetStorageService() {
			if (storageService != null) {
				return storageService;
			}
			lock (_storageLock) {
				if (storageService is null) {
					storageService 
						= new TelegramBotStorge(
							new TelegramBotConfig(Settings.Instance), 
							_loggerFactory.GetLogger());
				}
			}
			return storageService;
			
		}
	}
}
