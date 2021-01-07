using AliceAppraisal.Engine.Stratagy;
using System;
using System.Collections.Generic;
using System.Text;

namespace AliceAppraisal.Engine.Services {
	public class ServiceFactory : IServiceFactory {
		private static readonly IExternalService externalService = new ExternalService();
		public IExternalService GetExternalService() {
			return externalService;
		}

		private static readonly ITextGeneratorService textGeneratorService = new TextGenerator(externalService);
		public ITextGeneratorService GetTextGeneratorService() {
			return textGeneratorService;
		}

	}
}
