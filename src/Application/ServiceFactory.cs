﻿using AliceAppraisal.Application.Configuration;
using AliceAppraisal.Application.Infrastructure;
using AliceAppraisal.Application.Infrastructure.Models;
using AliceAppraisal.Core;
using AliceAppraisal.Core.Engine;
using AliceAppraisal.Implementations.Infrastructure;
using AliceAppraisal.Infrastructure;
using System.Collections.Generic;

namespace AliceAppraisal.Application {

	public class ServiceFactory : IServiceFactory {
		private readonly IDataProvider _dataProviderService;
		public IDataProvider GetDataProvider() {
			return _dataProviderService;
		}

		private static readonly object _lock = new object();

		public ServiceFactory(ILoggerFactory loggerFactory, IDataProvider externalService) {
			_loggerFactory = loggerFactory ?? LoggerFactory.Create();
			_dataProviderService = externalService;

			InitStratagy();
			
		}

		#region Logger
		private ILoggerFactory _loggerFactory; 
		public ILoggerFactory GetLoggerFactory() {
			return _loggerFactory;
		}
		#endregion

		#region Strategy
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

		#region StepService

		IStepService stepService;
		public IStepService GetStepService() {
			if(stepService is null) {
				lock (_lock) {
					if(stepService is null) {
						stepService = new StepService(this);
					}
				}
			}
			return stepService;
		}

		#endregion;

		#region Storage
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

		#endregion

		#region VehicleModelService

		private readonly IVehicleModelService vehicleModelService = new VehicleModelService();

		public IVehicleModelService GetVehicleModelService() {
			return vehicleModelService;
		}

		#endregion

		#region ManufactureYearService

		private readonly IManufactureYearService manufactureYearService = new ManufactureYearService();

		public IManufactureYearService GetManufactureYearService() {
			return manufactureYearService;
		}

		#endregion

	}
}
