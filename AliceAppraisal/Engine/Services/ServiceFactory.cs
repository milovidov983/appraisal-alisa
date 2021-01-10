using AliceAppraisal.Engine.Strategy;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AliceAppraisal.Engine.Services {
	public class ServiceFactory : IServiceFactory {
		private static readonly IExternalService externalService = new ExternalService();
		public IExternalService GetExternalService() {
			return externalService;
		}

		private static readonly ITextGeneratorService textGeneratorService = new TextGenerator();
		public ITextGeneratorService GetTextGeneratorService() {
			return textGeneratorService;
		}
		private static ILogger logger;
		public ServiceFactory(ILogger log) {
			logger = log;
			InitStratagy();
		}
		public ILogger GetLogger() {
			return logger;
		}


		private static IStrategyFactory strategyFactory;

		public IStrategyFactory GetStrategyFactory() {
			return StrategyFactory;
		}


		public static IStrategyFactory StrategyFactory { 
			get => strategyFactory; 
			set => strategyFactory = value; 
		}


		public void InitStratagy() {
			var strategies = ReflectiveEnumerator.GetEnumerableOfType<BaseStrategy>(this).ToList();
			StrategyFactory = new StrategyFactory(strategies.ToDictionary(
				x => x.GetType().FullName,
				x => x
				));
			Console.WriteLine();
		}


	}
}
